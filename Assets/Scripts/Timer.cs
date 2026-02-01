using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Tiempo")]
    public float tiempoInicial = 60f;
    private float tiempoActual;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;   // Panel/UI que se muestra al perder

    [Header("Audio Game Over")]
    public AudioSource audioGameOver;
    public AudioClip clipGameOver;

    private bool corriendo = true;
    private bool gameOverActivado = false;

    void Start()
    {
        tiempoActual = tiempoInicial;
        ActualizarTexto();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (!corriendo || gameOverActivado) return;

        if (tiempoActual > 0f)
        {
            tiempoActual -= Time.deltaTime;

            if (tiempoActual <= 0f)
            {
                tiempoActual = 0f;
                ActualizarTexto();
                ActivarGameOver();
                return;
            }

            ActualizarTexto();
        }
        else
        {
            // Por si arranca en 0 o cae aquí por alguna razón
            tiempoActual = 0f;
            ActualizarTexto();
            ActivarGameOver();
        }
    }

    void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoActual / 60);
        int segundos = Mathf.FloorToInt(tiempoActual % 60);
        if (timerText != null)
            timerText.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    void ActivarGameOver()
    {
        gameOverActivado = true;
        corriendo = false;

        // Mostrar UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Pausar juego
        Time.timeScale = 0f;

        // Sonido (para que suene aunque timeScale=0)
        if (audioGameOver != null)
        {
            audioGameOver.ignoreListenerPause = true;

            // Recomendado: pon el AudioSource en "Update Mode: Unscaled Time" desde el Inspector
            // (Audio -> DSP no depende de timeScale, pero esto ayuda si usas AudioListener.pause)
            if (clipGameOver != null)
                audioGameOver.PlayOneShot(clipGameOver);
            else
                audioGameOver.Play();
        }
    }

    // 🔹 para que PauseManager lo controle
    public void SetCorriendo(bool estado)
    {
        corriendo = estado;
    }

    // (Opcional) Si luego tienes botón de "Reintentar"
    public void ReiniciarTimer()
    {
        Time.timeScale = 1f;
        gameOverActivado = false;
        corriendo = true;
        tiempoActual = tiempoInicial;
        ActualizarTexto();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }
}
