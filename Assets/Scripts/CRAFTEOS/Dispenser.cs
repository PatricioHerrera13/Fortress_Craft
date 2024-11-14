using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private GameObject itemToDispensePrefab; // Ítem que el dispensador va a expulsar
    [SerializeField] private float cooldownTime = 1f; // Tiempo de enfriamiento en segundos, editable en el Inspector
    private bool isCoolingDown = false; // Flag para verificar si está en enfriamiento

    // Variables para manejar los sonidos
    public AudioSource audioSource; // El AudioSource que reproducirá los sonidos
    public AudioClip sfxDispenseItem; // Sonido para cuando se dispensa un ítem

    // Método que se llama cuando el jugador interactúa con el dispensador
    public void Interact(MonoBehaviour player)
    {
        // Verifica que el jugador tenga el método GetPickedItemType y GrabItemFromDispenser
        var pickedItemTypeMethod = player.GetType().GetMethod("GetPickedItemType");
        var grabItemMethod = player.GetType().GetMethod("GrabItemFromDispenser");

        if (pickedItemTypeMethod != null && grabItemMethod != null)
        {
            string pickedItemType = (string)pickedItemTypeMethod.Invoke(player, null);
            
            // Verifica si el jugador no tiene un ítem y si no está en enfriamiento
            if (pickedItemType == "" && !isCoolingDown)
            {
                Item newItem = CreateItem();
                if (newItem != null)
                {
                    grabItemMethod.Invoke(player, new object[] { newItem });
                    Debug.Log($"Dispensed: {newItem.name}");

                    // Reproducir sonido de dispensado
                    if (sfxDispenseItem != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(sfxDispenseItem);
                    }

                    // Inicia el enfriamiento
                    StartCoroutine(StartCooldown());
                }
            }
            else if (isCoolingDown)
            {
                Debug.Log("El dispensador está en enfriamiento. Espera un poco.");
            }
            else
            {
                Debug.Log("No puedes recoger un ítem porque ya tienes uno.");
            }
        }
        else
        {
            Debug.LogError("El jugador no tiene los métodos requeridos.");
        }
    }

    // Crea el ítem a ser dispensado
    private Item CreateItem()
    {
        if (itemToDispensePrefab == null)
        {
            Debug.LogError("itemToDispensePrefab no está asignado.");
            return null;
        }

        GameObject newItem = Instantiate(itemToDispensePrefab, transform.position, Quaternion.identity);
        return newItem.GetComponent<Item>();
    }

    // Coroutine para manejar el tiempo de enfriamiento
    private IEnumerator StartCooldown()
    {
        isCoolingDown = true; // Establece el estado de enfriamiento a verdadero
        yield return new WaitForSeconds(cooldownTime); // Espera el tiempo de enfriamiento configurado
        isCoolingDown = false; // Finaliza el enfriamiento
        Debug.Log("El dispensador ya está listo para usar.");
    }
}
