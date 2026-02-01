using UnityEngine;
using UnityEngine.UI;

public class PausaManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject overlayPause;

    public Image iconoBotonPausa;

    public Sprite spritePausa;
    public Sprite spriteContinuar;

    public Timer timer;

    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip sonido;

    private bool estaPausado = false;

    void Awake()
    {
        if (iconoBotonPausa == null)
        {
            iconoBotonPausa = GetComponent<Image>();

            if (iconoBotonPausa == null)
                iconoBotonPausa = GetComponentInChildren<Image>(true);
        }
    }

    void Start()
    {
        AplicarEstado(false);
    }

    public void TogglePause()
    {
        AplicarEstado(!estaPausado);
        if (audioSource != null && sonido != null)
            audioSource.PlayOneShot(sonido);
    }

    void AplicarEstado(bool pausar)
    {
        estaPausado = pausar;

        Time.timeScale = estaPausado ? 0f : 1f;

        if (overlayPause != null)
            overlayPause.SetActive(estaPausado);

        if (iconoBotonPausa != null)
        {
            iconoBotonPausa.sprite = estaPausado ? spriteContinuar : spritePausa;
            iconoBotonPausa.SetAllDirty();
        }

        if (timer != null)
            timer.SetCorriendo(!estaPausado);
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
