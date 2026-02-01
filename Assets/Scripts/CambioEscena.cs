using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip sonido;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

   
    //Esta función permite dado un nombre vinculado en Unity cargar una nueva escena
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void CargarEscenaSonido(string nombreEscena)
    {
        audioSource.PlayOneShot(sonido);
        SceneManager.LoadScene(nombreEscena);

    }

}