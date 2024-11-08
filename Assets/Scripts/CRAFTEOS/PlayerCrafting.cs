using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour {
    
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private float interactDistance = 3f; // Distancia de interacción
    private CraftingAnvil currentCraftingAnvil;

    public BoxCollider handCollider; // Referencia al BoxCollider de la mano

    // Asigna controles por jugador
    public KeyCode interactKeysPlayer1 = (KeyCode.X); // Tecla para Player 1
    public KeyCode interactKeysPlayer2 = (KeyCode.I); // Tecla para Player 2
    public bool isPlayer;// Indica si este script corresponde a Player 1 o Player 2
    public bool isPlayer2;// Indica si este script corresponde a Player 1 o Player 2

    private void Update() {
        // Verifica si hay colisión entre la mano y el CraftingAnvil
        if (IsHandCollidingWithCraftingAnvil() && currentCraftingAnvil != null) {
            // Verifica si hay ítems en el área de elaboración
            bool hasItemsInCraftingArea = HasItemsInCraftingArea(currentCraftingAnvil);

            // Detecta la tecla según el jugador
            KeyCode interactKeys = isPlayer ? interactKeysPlayer1 : interactKeysPlayer2;

            // Verifica si se presiona la tecla correspondiente al jugador
            if (Input.GetKeyDown(interactKeys)) {
                if (!hasItemsInCraftingArea) {
                    currentCraftingAnvil.NextRecipe(); // Cambia la receta si no hay ítems
                } else {
                    currentCraftingAnvil.Craft(isPlayer); // Intenta elaborar si hay ítems
                }
            }
        }

        // Busca el objeto CraftingAnvil más cercano al inicio
        FindNearestCraftingAnvil();
    }

    private void FindNearestCraftingAnvil() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactDistance, interactLayerMask);
        float closestDistance = float.MaxValue;

        // Busca el CraftingAnvil más cercano
        foreach (var collider in colliders) {
            if (collider.TryGetComponent(out CraftingAnvil craftingAnvil)) {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    currentCraftingAnvil = craftingAnvil; // Actualiza el CraftingAnvil actual
                }
            }
        }

        // Si no hay CraftingAnvil cercano, establece a null
        if (closestDistance == float.MaxValue) {
            currentCraftingAnvil = null;
        }
    }

    private bool IsHandCollidingWithCraftingAnvil() {
        // Verifica si el BoxCollider de la mano está colisionando con el CraftingAnvil
        Collider[] colliders = Physics.OverlapBox(handCollider.bounds.center, handCollider.bounds.extents, Quaternion.identity);
        foreach (var collider in colliders) {
            if (collider.TryGetComponent(out CraftingAnvil _)) {
                return true; // Hay colisión con el CraftingAnvil
            }
        }
        return false; // No hay colisión
    }

    private bool HasItemsInCraftingArea(CraftingAnvil craftingAnvil) {
        Collider[] colliderArray = Physics.OverlapBox(
            craftingAnvil.transform.position + craftingAnvil.placeItemsAreaBoxCollider.center,
            craftingAnvil.placeItemsAreaBoxCollider.size,
            craftingAnvil.placeItemsAreaBoxCollider.transform.rotation);

        foreach (Collider collider in colliderArray) {
            if (collider.CompareTag("Item")) {
                return true; // Hay al menos un ítem en el área
            }
        }
        return false; // No hay ítems en el área
    }
}
