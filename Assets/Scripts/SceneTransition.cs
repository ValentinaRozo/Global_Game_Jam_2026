using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [Header("Configuración de Transición")]
    public string nextSceneName;
    public float transitionDuration = 1f;
    public Color fadeColor = Color.black;
    
    public Image fadePanel;
    
    private static SceneTransition instance;
    
    void Awake()
    {
        // Singleton para evitar duplicados
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Crear panel de fade si no existe
        if (fadePanel == null)
        {
            CreateFadePanel();
        }
        
        // Iniciar con panel transparente
        SetFadeAlpha(0);
    }
    
    void CreateFadePanel()
    {
        // Crear Canvas si no existe
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("TransitionCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            canvas.sortingOrder = 9999;
        }
        
        // Crear panel
        GameObject panelObj = new GameObject("FadePanel");
        panelObj.transform.SetParent(canvas.transform, false);
        
        fadePanel = panelObj.AddComponent<Image>();
        fadePanel.color = fadeColor;
        
        // Hacer que cubra toda la pantalla
        RectTransform rect = panelObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        // Desactivar raycast
        fadePanel.raycastTarget = false;
    }
    
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }
    
    public void TransitionToNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("¡No se especificó el nombre de la siguiente escena!");
            return;
        }
        StartCoroutine(FadeAndLoadScene(nextSceneName));
    }
    
    IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade out (oscurecer)
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / transitionDuration);
            SetFadeAlpha(alpha);
            yield return null;
        }
        
        SetFadeAlpha(1);
        
        // Cargar la escena
        SceneManager.LoadScene(sceneName);
    }
    
    void SetFadeAlpha(float alpha)
    {
        if (fadePanel != null)
        {
            Color color = fadePanel.color;
            color.a = alpha;
            fadePanel.color = color;
        }
    }
    
    // Método estático para llamar desde cualquier parte
    public static void LoadScene(string sceneName)
    {
        if (instance != null)
        {
            instance.TransitionToScene(sceneName);
        }
    }
}