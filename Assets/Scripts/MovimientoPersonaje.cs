using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    public float velocidad = 4f;

    [Header("Referencias")]
    public SpriteRenderer visualSR;            // Arrastra aqu? el SpriteRenderer del hijo "Visual"

    [Header("Escalas (solo Visual)")]
    public Vector3 escalaVisualEscondido = new Vector3(0.6f, 0.6f, 1f);

    private Rigidbody2D rb;
    private Vector2 movimiento;

    private bool puedeEsconderse = false;
    private bool estaEscondido = false;

    private Sprite spriteNormal;
    private Sprite spriteEsconditeActual;
    private Vector3 escalaVisualNormal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Si no lo asignas en el inspector, intenta encontrarlo en hijos
        if (visualSR == null)
            visualSR = GetComponentInChildren<SpriteRenderer>();

        spriteNormal = visualSR.sprite;
        escalaVisualNormal = visualSR.transform.localScale;
    }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");

        if (puedeEsconderse && Input.GetKeyDown(KeyCode.Space))
            Esconderse();
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

    public void Esconderse()
    {
        estaEscondido = !estaEscondido;

        if (estaEscondido && spriteEsconditeActual != null)
        {
            visualSR.sprite = spriteEsconditeActual;
            visualSR.transform.localScale = escalaVisualEscondido; // ? solo Visual
        }
        else
        {
            visualSR.sprite = spriteNormal;
            visualSR.transform.localScale = escalaVisualNormal;     // ? solo Visual
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Escondite"))
        {
            puedeEsconderse = true;

            SpriteRenderer srEscondite = other.GetComponent<SpriteRenderer>();
            if (srEscondite != null)
                spriteEsconditeActual = srEscondite.sprite;
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
            spriteEsconditeActual = null;
        }
    }
}
