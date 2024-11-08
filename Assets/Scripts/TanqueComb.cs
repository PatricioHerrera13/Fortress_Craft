using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TanqueComb : MonoBehaviour
{
    // Referencias a los scripts de otros objetos
    public MonoBehaviour craftingTableScript; // Referencia a la mesa de crafteo
    public MonoBehaviour turretScript; // Referencia a la torreta
    public MonoBehaviour otherScript; // Otro script adicional

    // Variables actuales
    public GameObject QTECanvas; // Referencia al canvas del QTE
    public BidonComb Bidon; // Referencia al bidón
    public Transform player; // Referencia al Player
    public float maxCapacity = 5f; // Capacidad máxima del tanque
    public float currentFuel = 0f; // Combustible actual en el tanque
    public KeyCode activationKey = KeyCode.X; // Tecla de activación del QTE en el inspector
    public KeyCode activationKey1 = KeyCode.I;

    public Canvas targetCanvas; // Canvas en el que se generará la barra de progreso
    private GameObject fuelBar; // Referencia a la barra de progreso de combustible
    private RectTransform fuelBarRect; // RectTransform de la barra para ajustar su tamaño

    private bool isQTEActive = false;
    private bool playerInRange = false; // Nuevo booleano para verificar si el jugador está en el rango

    void Start()
    {
        CreateFuelBar();
        UpdateScriptsState(); // Configura el estado de los scripts al inicio
    }

    void Update()
    {
        // Descontar combustible cada segundo si es mayor a 0
        if (currentFuel > 0)
        {
            currentFuel = Mathf.Max(0, currentFuel - 0.01f * Time.deltaTime); // Ajustar la velocidad de consumo aquí
        }

        // Actualiza la barra de progreso para reflejar el combustible actual
        UpdateFuelBar();

        // Actualizar el estado de los scripts
        UpdateScriptsState();

        // Verificamos si el jugador está en el rango, sostiene el bidón, y el bidón no está vacío
        if ( (playerInRange && IsBidonHeld() && !Bidon.IsEmpty() && !isQTEActive && Input.GetKeyDown(activationKey)) || (playerInRange && IsBidonHeld() && !Bidon.IsEmpty() && !isQTEActive && Input.GetKeyDown(activationKey1)))
        {
            Debug.Log("QTE Activado");
            QTECanvas.SetActive(true); // Activa el canvas del QTE
            isQTEActive = true;
        }
        else if (!playerInRange || !IsBidonHeld() || Bidon.IsEmpty())
        {
            QTECanvas.SetActive(false);
            isQTEActive = false;
        }
    }

    // Método para actualizar el estado de los scripts según el combustible
    private void UpdateScriptsState()
    {
        bool hasFuel = currentFuel > 0;

        if (craftingTableScript != null)
            craftingTableScript.enabled = hasFuel; // Habilita/deshabilita la mesa de crafteo

        if (turretScript != null)
            turretScript.enabled = hasFuel; // Habilita/deshabilita la torreta

        if (otherScript != null)
            otherScript.enabled = hasFuel; // Habilita/deshabilita otro script adicional
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jugador en el rango del tanque.");
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Jugador fuera del rango del tanque.");
        }
    }

    // Método para verificar si el bidón está en la mano del jugador
    private bool IsBidonHeld()
    {
        Transform hand = player.Find("Hand/HandPoint"); // Encuentra el HandPoint en la jerarquía del Player
        if (hand != null)
        {
            foreach (Transform child in hand)
            {
                if (child.gameObject == Bidon.gameObject) // Verifica si el bidón es hijo de HandPoint
                {
                    Debug.Log(Bidon.gameObject);
                    return true;

                }
                Debug.Log(Bidon.gameObject);
            }

        }
        Debug.Log(Bidon.gameObject);
        return false;
    }

    public void CompleteQTE(bool success)
    {
        float fuelToTransfer = success ? Bidon.GetCurrentFuel() : Bidon.GetCurrentFuel() / 2;
        TransferFuel(fuelToTransfer);
        
        // Vaciar el bidón completamente después del QTE, independientemente del éxito o fallo
        Bidon.RemoveFuel(Bidon.GetCurrentFuel());

        QTECanvas.SetActive(false);
        isQTEActive = false;

        // Actualizar estado de scripts después del QTE
        UpdateScriptsState();
    }

    private void TransferFuel(float amount)
    {
        float transferableAmount = Mathf.Min(amount, maxCapacity - currentFuel);
        currentFuel += transferableAmount;
        currentFuel = Mathf.Round(currentFuel * 1000f) / 1000f; // Normalizar a 3 decimales
        Bidon.RemoveFuel(transferableAmount);

        Debug.Log("Combustible transferido al tanque: " + transferableAmount);
        Debug.Log("Combustible actual en el tanque: " + currentFuel);
    }

    public bool IsFull()
    {
        return currentFuel >= maxCapacity;
    }

    // Método para crear la barra de combustible en el canvas especificado
    private void CreateFuelBar()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Canvas de destino no asignado para la barra de combustible.");
            return;
        }

        // Crear el GameObject para la barra de combustible
        fuelBar = new GameObject("FuelBar");
        fuelBarRect = fuelBar.AddComponent<RectTransform>();
        fuelBar.transform.SetParent(targetCanvas.transform);

        // Ajustar las propiedades de RectTransform
        fuelBarRect.sizeDelta = new Vector2(1000 * (currentFuel / maxCapacity), 10); // Ancho de 50 cuando el tanque está lleno
        fuelBarRect.anchorMin = new Vector2(0.5f, 0f); // Ajustar para que esté debajo del objeto del tanque
        fuelBarRect.anchorMax = new Vector2(0.5f, 0f);
        fuelBarRect.pivot = new Vector2(0.5f, 1f);
        fuelBarRect.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, -20, 0); // Posición bajo el objeto

        // Añadir un componente Image y ajustar su color
        Image fuelBarImage = fuelBar.AddComponent<Image>();
        fuelBarImage.color = Color.green; // Cambia el color si es necesario
    }

    // Método para actualizar el tamaño de la barra de combustible
    private void UpdateFuelBar()
    {
        if (fuelBarRect != null)
        {
            fuelBarRect.sizeDelta = new Vector2(800 * (currentFuel / maxCapacity), 10); // Ancho máximo de 50
            fuelBarRect.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, -20, 0); // Actualiza la posición
        }
    }
}
