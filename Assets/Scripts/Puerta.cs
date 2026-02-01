using UnityEngine;

public class Puerta : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrada;

    [Header("Animator")]
    public string triggerAbrir = "Abrir";

    private bool abierta = false;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void IntentarAbrir()
    {
        if (abierta) return;

        // Solo abre si ya visitó/abrió el locker
        if (!JuegoProgreso.lockerAbierto)
        {
            if (audioSource != null && sonidoCerrada != null)
                audioSource.PlayOneShot(sonidoCerrada);

            Debug.Log("La puerta está cerrada. Debes abrir el locker primero.");
            return;
        }

        // Abrir
        abierta = true;

        if (animator != null && !string.IsNullOrEmpty(triggerAbrir))
            animator.SetTrigger(triggerAbrir);

        if (audioSource != null && sonidoAbrir != null)
            audioSource.PlayOneShot(sonidoAbrir);
    }
}
