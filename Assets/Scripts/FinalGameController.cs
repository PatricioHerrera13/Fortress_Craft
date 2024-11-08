using UnityEngine;
using UnityEngine.UI;

public class FinalGameController : MonoBehaviour
{
    public VidaJefe vidaJefe; // Referencia al script VidaJefe
    public Timer timer;       // Referencia al script Timer
    public Canvas resultadoCanvas; // Canvas de resultado
    public Text texto1;       // Primer texto para mostrar el resultado
    public Text texto2;       // Segundo texto para mostrar el resultado

    private bool finalTriggered = false; // Para evitar que el final se active varias veces

    void Update()
    {
        // Verificar si ya se activó el final
        if (finalTriggered) return;

        // Verificar si el tiempo llegó a 0 y el jefe aún tiene vida
        if (timer.timer <= 0 && vidaJefe.saludActual > 0)
        {
            ActivarDerrota();
        }
        // Verificar si el jefe ha sido derrotado antes de que termine el tiempo
        else if (vidaJefe.saludActual <= 0)
        {
            ActivarVictoria();
        }
    }

    void ActivarVictoria()
    {
        finalTriggered = true;
        resultadoCanvas.gameObject.SetActive(true);
        texto1.text = "Ustedes Ganan";
        texto2.text = "Dragon Murio";
        Time.timeScale = 0; // Pausar el juego
    }

    void ActivarDerrota()
    {
        finalTriggered = true;
        resultadoCanvas.gameObject.SetActive(true);
        texto1.text = "Dragon Gano";
        texto2.text = "Ustedes Perdieron";
        Time.timeScale = 0; // Pausar el juego
    }
}
