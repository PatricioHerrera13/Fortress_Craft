using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PozoCombustible : MonoBehaviour
{
    public BidonComb bidon; // Referencia al bidón
    private bool isPlayerInside = false; // Booleano para indicar si el jugador está dentro del collider

    void Update()
    {
        // Si el jugador está dentro y mantiene la tecla E, llenar el bidón
        if ((isPlayerInside && Input.GetKey(KeyCode.X)) || (isPlayerInside && Input.GetKey(KeyCode.I)))
        {
            StartFilling();
        }
    }

    

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el jugador entra al trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true; // Activa el booleano
            Debug.Log("Jugador dentro del pozo.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del trigger, desactiva el estado
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false; // Desactiva el booleano
            StopFilling(); // Asegúrate de detener el llenado al salir
            Debug.Log("Jugador fuera del pozo.");
        }
    }

    private void StartFilling()
    {
        if (bidon != null && !bidon.IsFull())
        {
            float amountToAdd = Time.deltaTime * 0.1f; // Ajusta la tasa de llenado como desees
            bidon.AddFuel(amountToAdd);
            Debug.Log("Bidón llenándose. Combustible actual: " + bidon.GetCurrentFuel());

            // Detenemos el llenado si el bidón está lleno después de agregar combustible
            if (bidon.IsFull())
            {
                StopFilling();
            }
        }
        else
        {
            Debug.Log("El bidón ya está lleno o no se encontró referencia.");
        }
    }

    private void StopFilling()
    {
        // Lógica adicional si se necesita detener algún efecto visual o sonido
        Debug.Log("Deteniendo el llenado del bidón.");
    }
}
