using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    public Text textoMoney;
    private float money;

    private void Start()
    {
        // Cargar el valor de money desde PlayerPrefs al iniciar
        money = PlayerPrefs.GetFloat("Money", 0);
        Debug.Log("Valor inicial de dinero: " + money);
        UpdateUI();
    }

    public void AddMoney(float amount)
    {
        if (amount > 0)
        {
            money += amount;
            GuardarDinero();
            Debug.Log("Dinero añadido: " + amount + ". Total: " + money.ToString());
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Intento de añadir una cantidad negativa: " + amount);
        }
    }

    public void DeductFromWallet(float rest)
    {
        if (rest > 0)
        {
            money -= rest;
            GuardarDinero();
            Debug.Log("Dinero descontado: " + rest + ". Total: " + money.ToString());
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Intento de restar una cantidad negativa: " + rest);
        }
    }

    private void GuardarDinero()
    {
        PlayerPrefs.SetFloat("Money", money);
        PlayerPrefs.Save(); // Llamada explícita para guardar PlayerPrefs inmediatamente
    }

    public float GetMoney()
    {
        Debug.Log("Obteniendo dinero: " + money.ToString());
        return money;
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
            textoMoney.text = money.ToString("");
            Debug.Log("Texto de dinero actualizado: " + textoMoney.text);
        }
        else
        {
            Debug.LogWarning("El componente de texto no está asignado.");
        }
    }
}
