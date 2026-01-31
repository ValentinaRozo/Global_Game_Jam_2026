using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    private int direccion = 1; // 1 = derecha, -1 = izquierda
    private Rigidbody2D rb;

    [Header("Deteccion")]
    public Transform jugador;
    public float distanciaCerca;
    public float distanciaAtrapado;


    [Header("Peligro")]
    public GameObject peligro;


    void Start()
    {
        //peligro.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);
        DetectarJugador();
    }

    void DetectarJugador()
    {
        if (jugador == null) return;

        Vector2 haciaJugador = jugador.position - transform.position;

        bool estaDelante = Mathf.Sign(haciaJugador.x) == direccion;

        if (!estaDelante) return;

        float distancia = haciaJugador.magnitude;

        if (distancia <= distanciaAtrapado)
        {
            peligro.SetActive(false);
            Debug.Log("?? ATRAPADO (delante)");
        }
        else if (distancia <= distanciaCerca)
        {
            peligro.SetActive(true);
        }
        if (distancia > distanciaCerca)
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaCerca);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtrapado);

        // L?nea de direcci?n
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.right * direccion * distanciaCerca
        );
    }
}
