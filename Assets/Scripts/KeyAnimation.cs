using System.Collections;
using UnityEngine;

public class KeyPromptAnimation : MonoBehaviour
{
    [Header("Configuración de Animación")]
    public float scaleAmount = 1.2f; // Cuánto se agranda (1.2 = 20% más grande)
    public float pulseSpeed = 1f; // Velocidad del pulso
    public AnimationType animationType = AnimationType.PulseLoop;

    private Vector3 originalScale;
    private Coroutine animationCoroutine;

    public enum AnimationType
    {
        PulseLoop,      // Pulsa continuamente
        PulseOnce,      // Pulsa una vez al activarse
        Bounce,         // Rebota
        Float           // Flota arriba y abajo
    }

    void OnEnable()
    {
        originalScale = transform.localScale;
        
        // Iniciar animación cuando se active
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
            
        switch (animationType)
        {
            case AnimationType.PulseLoop:
                animationCoroutine = StartCoroutine(PulseLoop());
                break;
            case AnimationType.PulseOnce:
                animationCoroutine = StartCoroutine(PulseOnce());
                break;
            case AnimationType.Bounce:
                animationCoroutine = StartCoroutine(Bounce());
                break;
            case AnimationType.Float:
                animationCoroutine = StartCoroutine(FloatAnimation());
                break;
        }
    }

    void OnDisable()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
            
        transform.localScale = originalScale;
    }

    // Pulso continuo
    IEnumerator PulseLoop()
    {
        while (true)
        {
            // Agrandar
            float elapsed = 0f;
            while (elapsed < pulseSpeed / 2)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(1f, scaleAmount, elapsed / (pulseSpeed / 2));
                transform.localScale = originalScale * scale;
                yield return null;
            }

            // Achicar
            elapsed = 0f;
            while (elapsed < pulseSpeed / 2)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(scaleAmount, 1f, elapsed / (pulseSpeed / 2));
                transform.localScale = originalScale * scale;
                yield return null;
            }
        }
    }

    // Pulso una sola vez
    IEnumerator PulseOnce()
    {
        // Agrandar
        float elapsed = 0f;
        while (elapsed < 0.2f)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, scaleAmount, elapsed / 0.2f);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        // Achicar
        elapsed = 0f;
        while (elapsed < 0.2f)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(scaleAmount, 1f, elapsed / 0.2f);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    // Rebote
    IEnumerator Bounce()
    {
        while (true)
        {
            float time = 0f;
            while (time < pulseSpeed)
            {
                time += Time.deltaTime;
                float bounce = Mathf.Abs(Mathf.Sin(time * Mathf.PI / pulseSpeed));
                float scale = 1f + (bounce * (scaleAmount - 1f));
                transform.localScale = originalScale * scale;
                yield return null;
            }
        }
    }

    // Flotación
    IEnumerator FloatAnimation()
    {
        Vector3 originalPos = transform.localPosition;
        float floatAmount = 10f;
        
        while (true)
        {
            float time = 0f;
            while (time < pulseSpeed * 2)
            {
                time += Time.deltaTime;
                float offset = Mathf.Sin(time * Mathf.PI / pulseSpeed) * floatAmount;
                transform.localPosition = originalPos + new Vector3(0, offset, 0);
                yield return null;
            }
        }
    }
}
