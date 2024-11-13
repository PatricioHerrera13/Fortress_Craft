using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TanqueComb : MonoBehaviour
{
    public MonoBehaviour craftingTableScript;
    public MonoBehaviour turretScript;
    public MonoBehaviour otherScript;

    public GameObject QTECanvas;
    public BidonComb Bidon;
    public Transform player1; // Referencia al Player 1
    public Transform player2; // Referencia al Player 2
    public float maxCapacity = 2f;
    public float currentFuel = 0f;

    public Canvas targetCanvas;
    public GameObject fuelBar;
    private RectTransform fuelBarRect;
    public Sprite bidonLLeno;

    public RectTransform imagenCombustible; // Imagen a la que se ajustará la barra de combustible
    public Vector3 offsetBarra = new Vector3(0, 0, 0); // Ajuste de posición para la barra al lado de la imagen

    private bool isQTEActive = false;
    private bool playerInRange = false;
    private Transform currentPlayer; // Referencia al jugador actual en rango

    void Start()
    {
        if (fuelBar == null)
        {
            Debug.LogError("Fuel bar image no asignada en el inspector.");
            return;
        }

        fuelBarRect = fuelBar.GetComponent<RectTransform>(); // Obtén el RectTransform de la barra de progreso
        fuelBar.gameObject.SetActive(false); // Activa la barra si está desactivada inicialmente
        UpdateFuelBar(); // Inicializa el tamaño de la barra
        UpdateScriptsState();
    }

    void Update()
    {
        if (currentFuel > 0)
        {
            currentFuel = Mathf.Max(0, currentFuel - 0.02f * Time.deltaTime);
        }

        UpdateFuelBar();
        UpdateScriptsState();

        // Detectar si el jugador en rango sostiene el bidón y no está vacío
        if (playerInRange && IsBidonHeld(currentPlayer) && !Bidon.IsEmpty() && !isQTEActive)
        {
            if ((currentPlayer == player1 && Input.GetKeyDown(KeyCode.X)) || 
                (currentPlayer == player2 && Input.GetKeyDown(KeyCode.I)))
            {
                Debug.Log("QTE Activado");
                QTECanvas.SetActive(true);
                isQTEActive = true;
            }
        }
        else if (!playerInRange || !IsBidonHeld(currentPlayer) || Bidon.IsEmpty())
        {
            QTECanvas.SetActive(false);
            isQTEActive = false;
        }
    }

    private void UpdateScriptsState()
    {
        bool hasFuel = currentFuel > 0;

        if (craftingTableScript != null)
            craftingTableScript.enabled = hasFuel;

        if (turretScript != null)
            turretScript.enabled = hasFuel;

        if (otherScript != null)
            otherScript.enabled = hasFuel;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Verificar si es Player 1 o Player 2 y establecer el jugador en rango
            if (other.transform == player1)
            {
                currentPlayer = player1;
                Debug.Log("Player 1 en el rango del tanque.");
            }
            else if (other.transform == player2)
            {
                currentPlayer = player2;
                Debug.Log("Player 2 en el rango del tanque.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Jugador fuera del rango del tanque.");
            currentPlayer = null; // Resetear el jugador en rango
        }
    }

    private bool IsBidonHeld(Transform player)
    {
        if (player == null) return false;

        Transform hand = player.Find("Hand/HandPoint");
        if (hand != null)
        {
            foreach (Transform child in hand)
            {
                if (child.gameObject == Bidon.gameObject)
                {
                    Debug.Log(Bidon.gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    public void CompleteQTE(bool success)
    {
        float fuelToTransfer = success ? Bidon.GetCurrentFuel() : Bidon.GetCurrentFuel() / 2;
        
        TransferFuel(fuelToTransfer);
        fuelBar.gameObject.SetActive(true);

        Bidon.RemoveFuel(Bidon.GetCurrentFuel());

        QTECanvas.SetActive(false);
        isQTEActive = false;

        UpdateScriptsState();
    }

    private void TransferFuel(float amount)
    {
        float transferableAmount = Mathf.Min(amount, maxCapacity - currentFuel);
        currentFuel += transferableAmount;
        currentFuel = Mathf.Round(currentFuel * 1000f) / 1000f;
        Bidon.RemoveFuel(transferableAmount);

        Debug.Log("Combustible transferido al tanque: " + transferableAmount);
        Debug.Log("Combustible actual en el tanque: " + currentFuel);
    }

    public bool IsFull()
    {
        return currentFuel >= maxCapacity;
    }

    private void UpdateFuelBar()
    {
        if (fuelBarRect != null)
        {
            // Ajustar el ancho de la barra en función del combustible actual
            fuelBarRect.sizeDelta = new Vector2(800 * (currentFuel / maxCapacity), 100); // Ancho máximo de 800

            // Posiciona la barra relativa a la imagen y el offset
            fuelBarRect.position = imagenCombustible.position + offsetBarra;
        }
        
        // Activa o desactiva la barra según el nivel de combustible
        fuelBar.gameObject.SetActive(currentFuel > 0);
    }
}
