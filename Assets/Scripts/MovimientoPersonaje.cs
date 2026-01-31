using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float velocidad = 4f;
    private Rigidbody2D rb;
    private Vector2 movimiento;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        rb.velocity = movimiento.normalized * velocidad;
    }

    
}
