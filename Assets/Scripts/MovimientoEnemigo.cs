using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public float velocidad;
    private int direccion = 1;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        direccion *= -1;

        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}

