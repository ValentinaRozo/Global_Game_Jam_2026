using UnityEngine;

public class EsconditeAudio : MonoBehaviour
{
    [Header("Sonidos al esconderse")]
    public AudioClip[] entrar;
    public AudioClip[] salir;

    public AudioClip GetEntrar()
    {
        if (entrar == null || entrar.Length == 0) return null;
        return entrar[Random.Range(0, entrar.Length)];
    }

    public AudioClip GetSalir()
    {
        if (salir == null || salir.Length == 0) return null;
        return salir[Random.Range(0, salir.Length)];
    }
}
