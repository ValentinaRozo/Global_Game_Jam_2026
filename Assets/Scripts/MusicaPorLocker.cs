using UnityEngine;
using UnityEngine.Audio;

public class MusicaPorLocker : MonoBehaviour
{
    [Header("Snapshots")]
    public AudioMixerSnapshot soloBateria;
    public AudioMixerSnapshot bateriaYBajo;

    [Header("Fade")]
    public float tiempoFade;

    private bool yaCambio = false;

    void Start()
    {
        if (soloBateria != null)
            soloBateria.TransitionTo(0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (yaCambio) return;

        if (other.CompareTag("Locker"))
        {
            yaCambio = true;
            if (bateriaYBajo != null)
                bateriaYBajo.TransitionTo(tiempoFade);
        }
    }
}
