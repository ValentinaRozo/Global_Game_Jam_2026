// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class TypeWriterEffect : MonoBehaviour
// {
//     public TextMeshProUGUI textComponent;
//     public float typingSpeed = 0.05f;

//     //New
//     private int lineIndex;
//     private bool isTypingDone = false;

//     [TextArea]
//     public string fullText;

//     public string[] dialogueLines;
//     public GameObject dialoguePanel;

//     void Start()
//     {
//         StartCoroutine(TypeText());
//     }

//     void Update()
//     {
//         if (isTypingDone && Input.GetKeyDown(KeyCode.Space))
//         {
//             NextDialogueLine();
//         }
//     }

//     IEnumerator TypeText()
//     {
//         textComponent.text = "";
//         isTypingDone = false;

//         foreach (char letter in fullText)
//         {
//             textComponent.text += letter;
//             yield return new WaitForSeconds(typingSpeed);
//         }

//         // Espera un poco y muestra todo el texto de golpe
//         yield return new WaitForSeconds(0.5f);
//         textComponent.text = fullText;
//         isTypingDone = true;
//     }

//     private void NextDialogueLine()
//     {
//         // Lógica para avanzar a la siguiente línea de diálogo
//         lineIndex++;
//         if (lineIndex < dialogueLines.Length)
//         {
//             fullText = dialogueLines[lineIndex];
//             StartCoroutine(TypeText());
//         }
//         else
//         {
//             // Fin del diálogo
//             dialoguePanel.SetActive(false);
//         }
//     }
// }


using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float typingSpeed = 0.05f;

    private int lineIndex = 0;
    private bool isTypingDone = false;

    // [TextArea]
    public string[] dialogueLines;
    public GameObject dialoguePanel;
    
    // Indicador de tecla
    public GameObject keyIndicator; // Arrastra aquí un UI Image o Text (ej: "Presiona ESPACIO")

    public TextMeshProUGUI keyIndicatorText;
    public float offsetBelowText = 20f; // Espacio debajo del texto

    void Start()
    {

        lineIndex = 0;
        if (keyIndicator != null)
            keyIndicator.SetActive(false);
            
        StartCoroutine(TypeText());
    }

    void Update()
    {
        if (isTypingDone && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogueLine();
        }
    }

    IEnumerator TypeText()
    {

        textComponent.text = "";
        isTypingDone = false;
        
        // Ocultar indicador mientras se escribe
        if (keyIndicator != null)
            keyIndicator.SetActive(false);


        // // Escribir letra por letra
        // string currentLine = dialogueLines[lineIndex]; // Aquí es donde falla
        foreach (char letter in dialogueLines[lineIndex])
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Texto completado
        isTypingDone = true;

        // AGREGADO: Llamar a las funciones que creaste
        UpdateKeyIndicator();
        PositionKeyIndicator();
        
        // Mostrar indicador de tecla
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

    private void PositionKeyIndicator()
    {
        if (keyIndicator == null || textComponent == null) return;

        // Forzar actualización del layout del texto
        Canvas.ForceUpdateCanvases();
        textComponent.ForceMeshUpdate();

        // Obtener la altura real del texto
        float textHeight = textComponent.preferredHeight;

        // Posicionar el indicador debajo del texto
        RectTransform textRect = textComponent.GetComponent<RectTransform>();
        RectTransform indicatorRect = keyIndicator.GetComponent<RectTransform>();

        // Calcular posición Y debajo del texto
        float newY = textRect.anchoredPosition.y - (textHeight / 2) - offsetBelowText - (indicatorRect.rect.height / 2);

        indicatorRect.anchoredPosition = new Vector2(
            indicatorRect.anchoredPosition.x, // Mantener X
            newY // Nueva Y
        );
    }
    // New code end

    private void NextDialogueLine()
    {
        lineIndex++;
        
        if (lineIndex < dialogueLines.Length)
        {
            // Hay más líneas, mostrar la siguiente
            StartCoroutine(TypeText());
        }
        else
        {
            // No hay más líneas, cerrar el diálogo
            dialoguePanel.SetActive(false);
        }
    }
}
