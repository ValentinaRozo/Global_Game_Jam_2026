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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (visualSR == null)
            visualSR = GetComponentInChildren<SpriteRenderer>();

        spriteNormal = visualSR.sprite;
        escalaVisualNormal = visualSR.transform.localScale;

        // Posici?n local original del Visual
        visualLocalPosNormal = visualSR.transform.localPosition;
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

        if (estaEscondido && srEsconditeActual != null && srEsconditeActual.sprite != null)
        {

            visualSR.sprite = srEsconditeActual.sprite;

            Vector2 objetivoWorld = srEsconditeActual.bounds.size;
            Vector2 actualWorld = visualSR.bounds.size;

            float factorX = (actualWorld.x > 0f) ? (objetivoWorld.x / actualWorld.x) : 1f;
            float factorY = (actualWorld.y > 0f) ? (objetivoWorld.y / actualWorld.y) : 1f;

            Vector3 s = visualSR.transform.localScale;
            s.x *= factorX + 0.05f;
            s.y *= factorY + 0.05f ;
            visualSR.transform.localScale = s;

            Vector3 centroEscondite = srEsconditeActual.bounds.center;
            Vector2 offsetVisual = new Vector2(0f, 0.15f);
            Vector3 local = transform.InverseTransformPoint(centroEscondite);
            
            visualSR.transform.localPosition = new Vector3(local.x, local.y, visualLocalPosNormal.z);
            visualSR.transform.localPosition = new Vector3(local.x+offsetVisual.x, local.y+offsetVisual.y, visualLocalPosNormal.z);
        }
        else
        {
            // Volver a normal
            visualSR.sprite = spriteNormal;
            visualSR.transform.localScale = escalaVisualNormal;
            visualSR.transform.localPosition = visualLocalPosNormal;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Escondite"))
        {
            puedeEsconderse = true;
            srEsconditeActual = other.GetComponent<SpriteRenderer>();
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

            srEsconditeActual = null;
        }
    }
}
