using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    private int direccion = 1; // 1 = derecha, -1 = izquierda
    private Rigidbody2D rb;

    [Header("Detecci?n")]
    public Transform jugador;
    public float distanciaCerca = 1.5f;     // brazo
    public float distanciaAtrapado = 0.7f;  // manos

    void Start()
    {
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

        // Vector hacia el jugador
        Vector2 haciaJugador = jugador.position - transform.position;

        // ?? ?Est? delante?
        bool estaDelante = Mathf.Sign(haciaJugador.x) == direccion;

        if (!estaDelante) return;

        float distancia = haciaJugador.magnitude;

        if (distancia <= distanciaAtrapado)
        {
            Debug.Log("?? ATRAPADO (delante)");
        }
        else if (distancia <= distanciaCerca)
        {
            Debug.Log("?? EST? CERCA (delante)");
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
