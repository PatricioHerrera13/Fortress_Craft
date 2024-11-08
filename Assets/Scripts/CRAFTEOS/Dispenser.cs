using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private GameObject itemToDispensePrefab; // Ítem que el dispensador va a expulsar

    public void Interact(MonoBehaviour player )
    {
        // Verifica que el jugador tenga el método GetPickedItemType
        var pickedItemTypeMethod = player.GetType().GetMethod("GetPickedItemType");
        var grabItemMethod = player.GetType().GetMethod("GrabItemFromDispenser");

        if (pickedItemTypeMethod != null && grabItemMethod != null)
        {
            string pickedItemType = (string)pickedItemTypeMethod.Invoke(player, null);
            
            if (pickedItemType == "")
            {
                Item newItem = CreateItem();
                if (newItem != null)
                {
                    grabItemMethod.Invoke(player, new object[] { newItem });
                    Debug.Log($"Dispensed: {newItem.name}");
                }
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
}