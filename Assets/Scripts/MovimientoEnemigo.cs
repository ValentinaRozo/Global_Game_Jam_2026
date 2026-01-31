using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    private int direccion = 1;
    private Rigidbody2D rb;

    [Header("Deteccion")]
    public Transform jugador;
    public float distanciaCerca;
    public float distanciaAtrapado;

    [Header("Peligro")]
    public GameObject peligro;

    [Header("Sonido de pasos")]
    public AudioSource audioPasos;
    public AudioClip[] pasosEnemigo;
    public float intervaloPasos = 0.45f;
    public float velocidadMinimaParaPasos = 0.1f;

    private float temporizadorPasos;

    // 👉 referencia al script del jugador
    private MovimientoPersonaje jugadorScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (audioPasos == null)
            audioPasos = GetComponent<AudioSource>();

        if (jugador != null)
            jugadorScript = jugador.GetComponent<MovimientoPersonaje>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);

        ManejarPasos();
        DetectarJugador();
    }

    // ================= PASOS =================
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

    // ================= DETECCIÓN =================
    void DetectarJugador()
    {
        if (jugador == null) return;

        // 🔒 SI EL JUGADOR ESTÁ OCULTO → NO LO VE
        if (jugadorScript != null && jugadorScript.estaEscondido)
        {
            peligro.SetActive(false);
            return;
        }

        Vector2 haciaJugador = jugador.position - transform.position;

        bool estaDelante = Mathf.Sign(haciaJugador.x) == direccion;
        if (!estaDelante)
        {
            peligro.SetActive(false);
            return;
        }

        float distancia = haciaJugador.magnitude;

        if (distancia <= distanciaAtrapado)
        {
            peligro.SetActive(false);
            Debug.Log("👿 ATRAPADO (delante)");
        }
        else if (distancia <= distanciaCerca)
        {
            peligro.SetActive(true);
            Debug.Log("👀 ESTA CERCA (delante)");
        }
        else
        {
            peligro.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        direccion *= -1;

        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}
