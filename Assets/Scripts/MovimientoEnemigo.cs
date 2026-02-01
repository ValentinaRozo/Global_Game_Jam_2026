using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    private int direccion = 1;
    private Rigidbody2D rb;

    [Header("Deteccion")]
    public Transform jugador;
    public float distanciaCerca = 3f;
    public float distanciaAtrapado = 1f;

    [Header("Peligro")]
    public GameObject peligro;

    [Header("Sonido de pasos")]
    public AudioSource audioPasos;
    public AudioClip[] pasosEnemigo;
    public float intervaloPasos = 0.45f;
    public float velocidadMinimaParaPasos = 0.1f;

    private float temporizadorPasos;

    private MovimientoPersonaje jugadorScript;

    // ✅ GAME OVER por atrapar
    [Header("Game Over (si atrapa)")]
    public GameObject gameOverUI;
    public AudioSource audioGameOver;
    public AudioClip clipGameOver;
    private bool gameOverActivado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (audioPasos == null)
            audioPasos = GetComponent<AudioSource>();

        if (jugador != null)
            jugadorScript = jugador.GetComponent<MovimientoPersonaje>();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void FixedUpdate()
    {
        if (gameOverActivado) return;

        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);

        ManejarPasos();
        DetectarJugador();
    }

    void ManejarPasos()
    {
        bool seEstaMoviendo = Mathf.Abs(rb.velocity.x) > velocidadMinimaParaPasos;

        if (seEstaMoviendo)
        {
            temporizadorPasos -= Time.fixedDeltaTime;

            if (temporizadorPasos <= 0f)
            {
                ReproducirPaso();
                temporizadorPasos = intervaloPasos;
            }
        }
        else
        {
            temporizadorPasos = 0f;
        }
    }

    void ReproducirPaso()
    {
        if (audioPasos == null) return;
        if (pasosEnemigo == null || pasosEnemigo.Length == 0) return;

        int i = Random.Range(0, pasosEnemigo.Length);
        audioPasos.PlayOneShot(pasosEnemigo[i]);
    }

    void DetectarJugador()
    {
        if (jugador == null) return;

        // Si el jugador está escondido, no lo detecta
        if (jugadorScript != null && jugadorScript.estaEscondido)
        {
            if (peligro != null) peligro.SetActive(false);
            return;
        }

        Vector2 haciaJugador = jugador.position - transform.position;

        bool estaDelante = Mathf.Sign(haciaJugador.x) == direccion;
        if (!estaDelante)
        {
            if (peligro != null) peligro.SetActive(false);
            return;
        }

        float distancia = haciaJugador.magnitude;

        if (distancia <= distanciaAtrapado)
        {
            if (peligro != null) peligro.SetActive(false);
            Debug.Log("👿 ATRAPADO (delante)");
            ActivarGameOverPorEnemigo();
        }
        else if (distancia <= distanciaCerca)
        {
            if (peligro != null) peligro.SetActive(true);
            Debug.Log("👀 ESTA CERCA (delante)");
        }
        else
        {
            if (peligro != null) peligro.SetActive(false);
        }
    }

    void ActivarGameOverPorEnemigo()
    {
        if (gameOverActivado) return;
        gameOverActivado = true;

        // Mostrar UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Pausar juego
        Time.timeScale = 0f;

        // Sonido (que suene aunque esté pausado)
        if (audioGameOver != null)
        {
            audioGameOver.ignoreListenerPause = true;

            if (clipGameOver != null)
                audioGameOver.PlayOneShot(clipGameOver);
            else
                audioGameOver.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si ya hubo game over, no importa rebotar
        if (gameOverActivado) return;

        direccion *= -1;

        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}
