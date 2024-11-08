using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para usar UI como el Canvas y Text

public class ControlVictoria : MonoBehaviour
{
    public VidaFortaleza vidaFort1; // Referencia pública al script VidaFortaleza del jugador 1
    public VidaFortaleza2 vidaFort2; // Referencia pública al script VidaFortaleza del jugador 2

    public Canvas resultadoCanvas; // Canvas que mostrará el resultado de la partida
    public Text resultadoTexto; // Texto dentro del Canvas que indicará quién ganó o perdió

    private bool juegoTerminado = false; // Para evitar que el resultado se muestre múltiples veces

    void Start()
    {
        // Asegurarse de que el canvas de resultado esté desactivado al inicio
        resultadoCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        // Verificar si alguno de los dos jugadores ha perdido y mostrar el resultado solo una vez
        if (!juegoTerminado)
        {
            if (vidaFort1 != null && vidaFort2 != null)
            {
                // Si el script VidaFortaleza de Fort1 activa "Perdiste"
                if (vidaFort1.saludActual <= 0)
                {
                    MostrarResultado("Jugador 2 gana", "Jugador 1 pierde");
                    juegoTerminado = true;
                }
                // Si el script VidaFortaleza2 de Fort2 activa "Perdiste"
                else if (vidaFort2.saludActual <= 0)
                {
                    MostrarResultado("Jugador 1 gana", "Jugador 2 pierde");
                    juegoTerminado = true;
                }
            }
        }
    }

    // Método para mostrar el resultado en el Canvas
    void MostrarResultado(string ganador, string perdedor)
    {
        resultadoCanvas.gameObject.SetActive(true); // Activar el Canvas de resultado
        resultadoTexto.text = $"{ganador}\n{perdedor}"; // Actualizar el texto con los resultados
        Time.timeScale = 0;

    }
}
