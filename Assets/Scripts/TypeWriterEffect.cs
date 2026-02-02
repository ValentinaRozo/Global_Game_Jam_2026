using System.Collections;
using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;

public class TypeWriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float typingSpeed = 0.05f;

    private int lineIndex = 0;
    private bool isTypingDone = false;
    private bool isTyping = false; // NUEVO - para saber si está escribiendo

    // [TextArea]
    public string[] dialogueLines;
    public GameObject dialoguePanel;
    
    // Indicador de tecla
    public GameObject keyIndicator; // Arrastra aquí un UI Image o Text (ej: "Presiona ESPACIO")

    public TextMeshProUGUI keyIndicatorText;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip[] voiceClips; // uno por cada línea

    private Coroutine typingCoroutine; // NUEVO - referencia a la corrutina
    // public float offsetBelowText = 20f; // Espacio debajo del texto

    // Para la transición de escena
    [Header("Transición de Escena")]
    public bool enableSceneTransition = true;
    public string nextSceneName = ""; // Escribe aquí el nombre de tu siguiente escena
    public float delayBeforeTransition = 0.5f;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;

    public GameObject boton;

    private CanvasGroup fadeCanvasGroup;

    void Start()
    {

        lineIndex = 0;
        if (keyIndicator != null)
            keyIndicator.SetActive(false);

        if (enableSceneTransition)
        {
            CreateFadePanel();
        }

        // typingCoroutine = StartCoroutine(TypeText());
        typingCoroutine = StartCoroutine(DelayedStart());
            
        // StartCoroutine(TypeText());
    }

    // NUEVA FUNCIÓN
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f); // Cambia 2f por los segundos que quieras
        typingCoroutine = StartCoroutine(TypeText());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Completar el texto instantáneamente
                CompleteText();
            }
            else if (isTypingDone)
            {
                // SI ES LA ÚLTIMA LÍNEA → CAMBIAR DE ESCENA
                if (IsLastLine())
                {
                    StartCoroutine(EndDialogueAndTransition());
                }
                else
                {
                    NextDialogueLine();
                }
            }
        }
    }

    private bool IsLastLine()
    {
        return lineIndex >= dialogueLines.Length - 1;
    }




    IEnumerator TypeText()
    {

        textComponent.text = "";
        isTypingDone = false;
        isTyping = true; // NUEVO - actualizar estado de escritura

        // Reproducir audio si existe
        // Reproducir voz de la línea actual
        if (voiceSource != null && voiceClips.Length > lineIndex && voiceClips[lineIndex] != null)
        {
            voiceSource.Stop();
            voiceSource.clip = voiceClips[lineIndex];
            voiceSource.Play();
        }

        
        // Ocultar indicador mientras se escribe
        if (keyIndicator != null)
            keyIndicator.SetActive(false);

        // Guardar el texto completo
        string fullText = dialogueLines[lineIndex];


        // // Escribir letra por letra
        // string currentLine = dialogueLines[lineIndex]; // Aquí es donde falla
        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Texto completado
        isTyping = false; // NUEVO - marca que terminó de escribir
        isTypingDone = true;

        // AGREGADO: Llamar a las funciones que creaste
        UpdateKeyIndicator();
        // PositionKeyIndicator();
        
        // Mostrar indicador de tecla
        if (keyIndicator != null)
            keyIndicator.SetActive(true);
    }

    // NUEVA FUNCIÓN - Completar texto instantáneamente
    private void CompleteText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Detener la animación
        }

        // Mostrar el texto completo
        textComponent.text = dialogueLines[lineIndex];

        if (voiceSource != null && voiceSource.isPlaying)
        {
            voiceSource.Stop();
        }
        
        isTyping = false;
        isTypingDone = true;
        
        UpdateKeyIndicator();
        
        if (keyIndicator != null)
            keyIndicator.SetActive(true);
    }

    // New code
    private void UpdateKeyIndicator()
    {
        if (keyIndicatorText == null) return;

        if (lineIndex >= dialogueLines.Length - 1)
        {
            keyIndicatorText.text = "↵ CERRAR";
        }
        else
        {
            keyIndicatorText.text = "↵ CONTINUAR";
        }
    }


    private void NextDialogueLine()
    {
        if (voiceSource != null && voiceSource.isPlaying)
        {
            voiceSource.Stop();
        }

        lineIndex++;

        if (lineIndex < dialogueLines.Length)
        {
            typingCoroutine = StartCoroutine(TypeText());
            boton.SetActive(true);
        }
    }


    IEnumerator EndDialogueAndTransition()
    {
        boton.SetActive(true);
        // Cerrar el panel de diálogo
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);
        
        // Esperar un momento
        yield return new WaitForSeconds(delayBeforeTransition);
        
        // Hacer transición
        if (enableSceneTransition)
        {

            yield return StartCoroutine(FadeAndLoadScene());
        }
    }

    void CreateFadePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No se encontró Canvas para el fade.");
            return;
        }

        GameObject fadePanel = new GameObject("FadePanel");
        fadePanel.transform.SetParent(canvas.transform, false);

        RectTransform rect = fadePanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        UnityEngine.UI.Image image = fadePanel.AddComponent<UnityEngine.UI.Image>();
        image.color = fadeColor;
        image.raycastTarget = false;

        fadeCanvasGroup = fadePanel.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.blocksRaycasts = false;

        fadePanel.transform.SetAsLastSibling();
    }

    IEnumerator FadeAndLoadScene()
    {
        boton.SetActive(true);
        if (fadeCanvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1;
        }
        else
        {
            yield return new WaitForSeconds(fadeDuration);
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("¡No se especificó el nombre de la siguiente escena!");
        }
    }
}

