using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipeSO : ScriptableObject {
    public Sprite sprite; // Imagen de la receta
    public List<ItemSO> inputItemSOList; // Ítems necesarios para elaborar
    public ItemSO outputItemSO; // Objeto resultante de la elaboración
    public float craftingTime; // Tiempo de crafting en segundos
    public int requiredPulses; // Pulsaciones necesarias para el crafteo
}
