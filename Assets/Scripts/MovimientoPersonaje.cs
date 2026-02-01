using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    public float velocidad = 4f;

    [Header("Referencias")]
    public SpriteRenderer visualSR;

    private Rigidbody2D rb;
    private Vector2 movimiento;

    private bool puedeEsconderse = false;
    public bool estaEscondido = false;

    private Sprite spriteNormal;
    private Vector3 escalaVisualNormal;
    private Vector3 visualLocalPosNormal;

    private SpriteRenderer srEsconditeActual;

    private Color colorVisualNormal;

    [Header("Sonido de pasos")]
    public AudioSource audioPasos;
    public AudioClip[] pasos;
    public float intervaloPasos = 0.4f;

    [Header("Sonido de esconderse")]
    public AudioSource audioEsconderse;
    private EsconditeAudio esconditeAudioActual;

    private float temporizadorPasos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (visualSR == null)
            visualSR = GetComponentInChildren<SpriteRenderer>();

        if (audioPasos == null)
            audioPasos = GetComponent<AudioSource>();

        spriteNormal = visualSR.sprite;
        escalaVisualNormal = visualSR.transform.localScale;
        visualLocalPosNormal = visualSR.transform.localPosition;

        colorVisualNormal = visualSR.color;
    }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");

        if (puedeEsconderse && Input.GetKeyDown(KeyCode.Space))
            Esconderse();

        ManejarPasos();
    }

    void FixedUpdate()
    {
        if (estaEscondido)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = movimiento.normalized * velocidad;
    }

    void ManejarPasos()
    {
        bool seEstaMoviendo = Mathf.Abs(movimiento.x) > 0.1f;

        if (seEstaMoviendo && !estaEscondido)
        {
            temporizadorPasos -= Time.deltaTime;

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
        if (pasos.Length == 0) return;

        int indice = Random.Range(0, pasos.Length);
        audioPasos.PlayOneShot(pasos[indice]);
    }

    public void Esconderse()
    {
        bool vaAEsconderse = !estaEscondido;
        estaEscondido = vaAEsconderse;

        if (audioEsconderse != null && esconditeAudioActual != null)
        {
            AudioClip clip = vaAEsconderse
                ? esconditeAudioActual.GetEntrar()
                : esconditeAudioActual.GetSalir();

            if (clip != null)
                audioEsconderse.PlayOneShot(clip);
        }

        if (estaEscondido && srEsconditeActual != null && srEsconditeActual.sprite != null)
        {
  
            visualSR.sprite = srEsconditeActual.sprite;

            visualSR.color = Color.black;


            Vector2 objetivoWorld = srEsconditeActual.bounds.size;
            Vector2 actualWorld = visualSR.bounds.size;

            float factorX = (actualWorld.x > 0f) ? (objetivoWorld.x / actualWorld.x) : 1f;
            float factorY = (actualWorld.y > 0f) ? (objetivoWorld.y / actualWorld.y) : 1f;

            Vector3 s = visualSR.transform.localScale;
            s.x *= factorX + 0.05f;
            s.y *= factorY + 0.05f;
            visualSR.transform.localScale = s;

            Vector3 centroEscondite = srEsconditeActual.bounds.center;
            Vector2 offsetVisual = new Vector2(0f, 0.15f);
            Vector3 local = transform.InverseTransformPoint(centroEscondite);

            visualSR.transform.localPosition = new Vector3(
                local.x + offsetVisual.x,
                local.y + offsetVisual.y,
                visualLocalPosNormal.z
            );
        }
        else
        {
            visualSR.sprite = spriteNormal;
            visualSR.transform.localScale = escalaVisualNormal;
            visualSR.transform.localPosition = visualLocalPosNormal;

            visualSR.color = colorVisualNormal;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Escondite"))
        {
            puedeEsconderse = true;
            srEsconditeActual = other.GetComponent<SpriteRenderer>();
            esconditeAudioActual = other.GetComponent<EsconditeAudio>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Escondite"))
        {
            puedeEsconderse = false;
            estaEscondido = false;

            visualSR.sprite = spriteNormal;
            visualSR.transform.localScale = escalaVisualNormal;
            visualSR.transform.localPosition = visualLocalPosNormal;

            visualSR.color = colorVisualNormal;

            srEsconditeActual = null;
            esconditeAudioActual = null;
        }
    }
}
