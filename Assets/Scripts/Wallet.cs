using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour // Asegúrate de que Wallet herede de MonoBehaviour
{
    public Text textoMoney; // Hacerlo privado y establecerlo a través de un método
    private float money;

    public void AddMoney(float amount)
    {
        money += amount;
        Debug.Log("Dinero añadido: " + amount + ". Total: " + money.ToString("F2")); // Mensaje de depuración
        UpdateUI(); // Actualiza la UI después de añadir dinero
    }

    public void DeductFromWallet(float amount) // Método para descontar dinero de la billetera
    {
        money -= amount; // Permitir que money sea negativo
        Debug.Log("Dinero descontado: " + amount + ". Total: " + money.ToString("F2")); // Mensaje de depuración
        UpdateUI(); // Actualiza la UI después de descontar
    }

    public float GetMoney()
    {
        return money;
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
            textoMoney.text = money.ToString("F2"); // Actualiza el texto con el nuevo valor
            Debug.Log("Texto de dinero actualizado: " + money.ToString("F2")); // Mensaje de depuración
        }
        else
        {
            Debug.LogWarning("El componente de texto no está asignado."); // Mensaje de advertencia si no hay texto asignado
        }
    }
}