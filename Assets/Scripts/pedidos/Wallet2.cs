using UnityEngine;
using UnityEngine.UI;

public class Wallet2 : MonoBehaviour
{
    public Text textoMoney;
    private float money2 = 0;

    private void Start()
    {
        // Reiniciar el valor de money2 a 0 al iniciar el juego
        money2 = 0;
        PlayerPrefs.DeleteKey("Money1");  // Borra el valor almacenado en PlayerPrefs

        Debug.Log("Valor inicial de dinero en Wallet1 reiniciado a: " + money2);
        UpdateUI();
    }

    public void AddMoney(float amount)
    {
        if (amount > 0)
        {
            money2 += amount;
            GuardarDinero();
            Debug.Log("Dinero añadido en Wallet1: " + amount + ". Total: " + money2.ToString());
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Intento de añadir una cantidad negativa en Wallet1: " + amount);
        }
    }

    public void DeductFromWallet(float rest)
    {
        if (rest > 0)
        {
            money2 -= rest;
            GuardarDinero();
            Debug.Log("Dinero descontado en Wallet1: " + rest + ". Total: " + money2.ToString());
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Intento de restar una cantidad negativa en Wallet1: " + rest);
        }
    }

    private void GuardarDinero()
    {
        PlayerPrefs.SetFloat("Money1", money2);
        PlayerPrefs.Save();
    }

    public float GetMoney()
    {
        Debug.Log("Obteniendo dinero en Wallet1: " + money2.ToString());
        return money2;
    }

    public void SetTextComponent(Text newText)
    {
        textoMoney = newText;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (textoMoney != null)
        {
            textoMoney.text = money2.ToString("");
            Debug.Log("Texto de dinero en Wallet1 actualizado: " + textoMoney.text);
        }
        else
        {
            Debug.LogWarning("El componente de texto en Wallet1 no está asignado.");
        }
    }
}
