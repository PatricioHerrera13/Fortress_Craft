using UnityEngine;

// Clase auxiliar para vincular sprites de pedidos con prefabs
[System.Serializable]
public class OrderPrefabData
{
    public Sprite orderSprite;  // Imagen que representa el pedido
    public Sprite objectSprite;  // Imagen del objeto relacionado
    public GameObject orderPrefab;  // Prefab correspondiente al pedido
    public ItemSO itemData;
}