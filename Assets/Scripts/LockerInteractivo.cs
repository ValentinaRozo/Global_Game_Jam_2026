using UnityEngine;

public class LockerInteractivo : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteCerrado;
    public Sprite spriteAbierto;

    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip sonidoAbrir;

    public bool soloUnaVez = true;

    private SpriteRenderer sr;
    private bool jugadorDentro = false;
    private bool estaAbierto = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null && spriteCerrado != null)
            sr.sprite = spriteCerrado;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!jugadorDentro) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CambiarSprite();
        }
    }

    void CambiarSprite()
    {
        if (sr == null) return;
        if (soloUnaVez && estaAbierto) return;

        estaAbierto = !estaAbierto;
        sr.sprite = estaAbierto ? spriteAbierto : spriteCerrado;

        if (estaAbierto && sonidoAbrir != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoAbrir);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = false;
    }
}
