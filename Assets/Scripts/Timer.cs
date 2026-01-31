using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float tiempoInicial = 60f;
    private float tiempoActual;

    public TextMeshProUGUI timerText;

    void Start()
    {
        tiempoActual = tiempoInicial;
        ActualizarTexto();
    }

    void Update()
    {
        if (tiempoActual > 0)
        {
            tiempoActual -= Time.deltaTime;
            ActualizarTexto();
        }
        else
        {
            tiempoActual = 0;
            ActualizarTexto();
        }
    }

    void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoActual / 60);
        int segundos = Mathf.FloorToInt(tiempoActual % 60);

        timerText.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }
}
