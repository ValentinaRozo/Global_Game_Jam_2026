using UnityEngine;

public class TriggerPuerta : MonoBehaviour
{
    public Puerta puerta;
    private bool jugadorCerca;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) jugadorCerca = false;
    }

    void Update()
    {
        if (jugadorCerca && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            puerta.IntentarAbrir();
        }
    }
}
