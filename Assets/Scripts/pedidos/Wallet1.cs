using UnityEngine;
using UnityEngine.UI;

public class Wallet1 : MonoBehaviour // Asegúrate de que Wallet herede de MonoBehaviour
{
    public Text textoMoney; // Hacerlo privado y establecerlo a través de un método
    public float money1;

    public void AddMoney(float amount)
    {
        money1 += amount;
        Debug.Log("Dinero añadido: " + amount + ". Total: " + money1.ToString("F2")); // Mensaje de depuración
        UpdateUI(); // Actualiza la UI después de añadir dinero
    }

    public void DeductFromWallet(float amount) // Método para descontar dinero de la billetera
    {
        money1 -= amount; // Permitir que money1 sea negativo
        Debug.Log("Dinero descontado: " + amount + ". Total: " + money1.ToString("F2")); // Mensaje de depuración
        UpdateUI(); // Actualiza la UI después de descontar
    }

    public float GetMoney()
    {
        return money1;
    }

    // Método para asignar el Text desde otro script
    public void SetTextComponent(Text newText)
    {
        textoMoney = newText;
        UpdateUI(); // Actualizar el texto en la UI cuando se asigne el componente
    }

    // Método para actualizar el texto cuando se inicializa o cambia la referencia
    private void UpdateUI()
    {
        if (textoMoney != null)
        {
            textoMoney.text = money1.ToString("F2"); // Actualiza el texto con el nuevo valor
            Debug.Log("Texto de dinero actualizado: " + money1.ToString("F2")); // Mensaje de depuración
        }
        else
        {
            Debug.LogWarning("El componente de texto no está asignado."); // Mensaje de advertencia si no hay texto asignado
        }
    }
}