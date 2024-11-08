using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingAnvil : MonoBehaviour
{
    [SerializeField] private Image recipeImage;
    [SerializeField] private List<CraftingRecipeSO> craftingRecipeSOList;
    [SerializeField] public BoxCollider placeItemsAreaBoxCollider;
    [SerializeField] private Transform itemSpawnPoint;

    public CraftingProgressBar craftingProgressBar; // Barra de tiempo
    public CraftingPulsationBar craftingPulsationBar; // Nueva barra de pulsaciones
    private CraftingRecipeSO craftingRecipeSO;

    private bool isCrafting = false; // Estado de elaboración
    public enum CraftingMode { Time, Pulses }
    public CraftingMode craftingMode; // Selección de modo de crafteo

    private void Awake() {
        NextRecipe(); // Inicializa la receta al comenzar
        craftingProgressBar.OnCraftingComplete += HandleCraftingComplete; // Suscribirse al evento
        craftingPulsationBar.OnCraftingComplete += HandleCraftingComplete; // Suscribirse al evento
    }

    public void NextRecipe() {
        // Verifica si ya hay un proceso de elaboración en curso
        if (isCrafting) return;
        
        // Verifica si hay ítems en el área de elaboración
        Collider[] colliderArray = Physics.OverlapBox(
            transform.position + placeItemsAreaBoxCollider.center,
            placeItemsAreaBoxCollider.size,
            placeItemsAreaBoxCollider.transform.rotation);

        // Verifica si hay algún objeto con el tag "Item"
        foreach (Collider collider in colliderArray) {
            if (collider.CompareTag("Item")) {
                Debug.Log("No puedes cambiar la receta mientras hay ítems en el área.");
                return;
            }
        }

        // Cambia a la siguiente receta en la lista
        if (craftingRecipeSO == null) {
            craftingRecipeSO = craftingRecipeSOList[0]; // Primera receta
        } else {
            int index = craftingRecipeSOList.IndexOf(craftingRecipeSO);
            index = (index + 1) % craftingRecipeSOList.Count; // Cicla a la siguiente receta
            craftingRecipeSO = craftingRecipeSOList[index];
        }

        recipeImage.sprite = craftingRecipeSO.sprite; // Actualiza la imagen de la receta
    }

    public void Craft(bool isPlayer) {
        // Verifica si ya hay un proceso de elaboración en curso
        if (isCrafting) return;

        // Verifica ítems en el área de elaboración
        Collider[] colliderArray = Physics.OverlapBox(
            transform.position + placeItemsAreaBoxCollider.center, 
            placeItemsAreaBoxCollider.size, 
            placeItemsAreaBoxCollider.transform.rotation);
        
        List<ItemSO> inputItemList = new List<ItemSO>(craftingRecipeSO.inputItemSOList);
        List<GameObject> consumeItemGameObjectList = new List<GameObject>();

        foreach (Collider collider in colliderArray) { 
            if (collider.TryGetComponent(out ItemSOHolder itemSOHolder)) {
                if (inputItemList.Contains(itemSOHolder.itemSO)) {
                    inputItemList.Remove(itemSOHolder.itemSO); // Remueve ítem de la lista
                    consumeItemGameObjectList.Add(collider.gameObject); // Añade a la lista de consumibles
                }
            }
        }

        // Si se tienen todos los ítems necesarios
        if (inputItemList.Count == 0) {
            Debug.Log("Iniciando elaboración.");
            isCrafting = true; // Cambia el estado a en elaboración

        // Iniciar según el modo de crafteo
            if (craftingMode == CraftingMode.Time) {
                craftingProgressBar.StartCrafting(craftingRecipeSO.craftingTime);
                StartCoroutine(ConsumeItems(consumeItemGameObjectList)); // Consume los ítems
            } else if (craftingMode == CraftingMode.Pulses) {
                craftingPulsationBar.StartCrafting(craftingRecipeSO.requiredPulses, isPlayer); // Asegúrate de que requiredPulses esté en CraftingRecipeSO
                StartCoroutine(ConsumeItems(consumeItemGameObjectList)); // Consume los ítems
            }
        }
    }

    private IEnumerator ConsumeItems(List<GameObject> consumeItemGameObjectList) {
        // Esperar si es modo de tiempo
        if (craftingMode == CraftingMode.Time) {
            yield return new WaitForSeconds(craftingRecipeSO.craftingTime); 
        }
        // Esperar si es modo de pulsaciones hasta que se complete el crafteo
        else if (craftingMode == CraftingMode.Pulses) {
            while (craftingPulsationBar.isCrafting) {
                yield return null; // Espera hasta que el crafteo se complete
            }
        }

        foreach (GameObject consumeItemGameObject in consumeItemGameObjectList) {
            Destroy(consumeItemGameObject); // Destruye los ítems consumidos
        }

        // Al finalizar la elaboración, reinicia el estado
        isCrafting = false; 
    }

    private void HandleCraftingComplete()
    {
        // Instanciar el ítem solo cuando el crafting se complete
        GameObject spawnedItemGameObject = Instantiate(craftingRecipeSO.outputItemSO.prefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        Debug.Log("Ítem fabricado: " + spawnedItemGameObject.name);
    }
}