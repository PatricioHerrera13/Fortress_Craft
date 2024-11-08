using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public VidaFortaleza vidaFortaleza1;
    public VidaFortaleza2 vidaFortaleza2;
    public Canvas endGameCanvas;
    public Text player1Text;
    public Text player2Text;

    private bool gameEnded = false;

    void Update()
    {
        // Comprobar si ya terminó el juego para evitar ejecutar código innecesario
        if (gameEnded) return;

        // Verificar si la salud de cualquiera de los jugadores ha llegado a cero
        if (vidaFortaleza1.saludActual <= 0 || vidaFortaleza2.saludActual <= 0)
        {
            gameEnded = true;
            endGameCanvas.gameObject.SetActive(true);

            if (vidaFortaleza1.saludActual <= 0)
            {
                player1Text.text = "Player 1: Perdiste";
                player2Text.text = "Player 2: Ganaste";
            }
            else if (vidaFortaleza2.saludActual <= 0)
            {
                player1Text.text = "Player 1: Ganaste";
                player2Text.text = "Player 2: Perdiste";
            }
        }
    }
}
