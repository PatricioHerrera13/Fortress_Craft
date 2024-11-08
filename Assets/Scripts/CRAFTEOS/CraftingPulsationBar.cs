using UnityEngine;
using UnityEngine.UI;

public class CraftingPulsationBar : MonoBehaviour
{
    public Image progressBar; // Asigna la imagen de la barra de pulsaciones
    private int requiredPulses; // Pulsaciones requeridas
    private int currentPulses = 0;
    public bool isCrafting = false;

    // Teclas para cada jugador
    public KeyCode craftingKeyPlayer1 = KeyCode.X; // Tecla para Player 1
    public KeyCode craftingKeyPlayer2 = KeyCode.I; // Tecla para Player 2
    public bool isPlayer; // Indica si este script es para Player 1
    public bool isPlayer2; // Indica si este script es para Player 2

    public delegate void CraftingComplete();
    public event CraftingComplete OnCraftingComplete; // Evento para la finalización del crafteo

    void Update()
    {
        if (isCrafting)
        {   
            // Usar la tecla correspondiente según el jugador
            KeyCode craftingKey = isPlayer ? craftingKeyPlayer1 : craftingKeyPlayer2;

            if (Input.GetKeyDown(craftingKey))
            {
                currentPulses++;
                UpdateProgressBar();

                if (currentPulses >= requiredPulses)
                {
                    CompleteCrafting();
                }
            }
        }
    }

    public void StartCrafting(int pulses, bool isPlayer)
    {
        requiredPulses = pulses; // Asigna las pulsaciones requeridas
        this.isPlayer = isPlayer; // Asigna el estado del jugador
        isCrafting = true;
        currentPulses = 0;
        progressBar.fillAmount = 0f; // Reiniciar la barra
    }

    private void UpdateProgressBar()
    {
        progressBar.fillAmount = (float)currentPulses / requiredPulses; // Actualiza la barra
    }

    private void CompleteCrafting()
    {
        isCrafting = false;
        progressBar.fillAmount = 1f; // Asegura que la barra esté llena
        OnCraftingComplete?.Invoke(); // Llama al evento cuando se complete
    }
}
