using System.Collections.Generic;
using UnityEngine;

public class PortalVS1 : MonoBehaviour
{
    public OrderManagerPlayer1 orderManager; // Referencia al script OrderManager del jugador 1
    public Collider jugadorCollider; // Collider del jugador 1
    public List<OrderPrefabData> itemsRequeridos; // Lista de datos de pedidos requeridos
    public float cantEntrega = 0; // Contador de entregas
    public PlayerVS1 player1; // Referencia al jugador 1

    private bool jugadorDentro = false; // Variable que indica si el jugador está dentro del área

    private void OnTriggerEnter(Collider other)
    {
        if (other == jugadorCollider)
        {
            jugadorDentro = true; // El jugador está dentro del área
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == jugadorCollider)
        {
            jugadorDentro = false; // El jugador ha salido del área
        }
    }

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.X))
        {
            ProcesarEntrega();
        }
    }

    private void ProcesarEntrega()
    {
        var playerPickUp = player1.GetComponentInChildren<PickUpItem>();
        if (playerPickUp != null)
        {
            GameObject prefabJugador = playerPickUp.GetPickedPrefab();
            if (prefabJugador != null)
            {
                ItemSOHolder itemSOHolder = prefabJugador.GetComponent<ItemSOHolder>();
                if (itemSOHolder != null)
                {
                    ItemSO itemSO = itemSOHolder.itemSO;
                    if (itemSO != null)
                    {
                        OrderPrefabData orderData = FindOrderData(prefabJugador);
                        if (orderData != null && orderManager.EliminarPedido(orderData.orderSprite, itemSO)) // Pasando el ItemSO adicional
                        {
                            EliminarItemDeLasManos(player1);
                            player1.wallet.AddMoney(itemSO.valor);
                            cantEntrega += 1;
                        }
                        else
                        {
                            EliminarItemDeLasManos(player1); // Entrega errónea
                        }
                    }
                }
            }
        }
    }


    private void EliminarItemDeLasManos(PlayerVS1 player)
    {
        Transform hand = player.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            Destroy(hand.GetChild(0).gameObject);
            FindObjectOfType<PickUpItem>().ReleaseItem();
        }
    }

    public void ActualizarItemsRequeridos()
    {
        itemsRequeridos.Clear();
        var activeOrders = orderManager.GetActiveOrders();

        foreach (OrderPrefabData pedido in activeOrders)
        {
            itemsRequeridos.Add(pedido);
        }
    }

    private OrderPrefabData FindOrderData(GameObject prefab)
    {
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return null;

        Sprite objetoSprite = spriteRenderer.sprite;

        foreach (OrderPrefabData order in itemsRequeridos)
        {
            if (order.objectSprite == objetoSprite)
            {
                return order;
            }
        }
        return null;
    }
}
