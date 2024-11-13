using System.Collections.Generic;
using UnityEngine;

public class PortalVS2 : MonoBehaviour
{
    public OrderManagerPlayer2 orderManager; // Referencia al script OrderManager del jugador 2
    public Collider jugadorCollider; // Collider del jugador 2
    public List<OrderPrefabData> itemsRequeridos; // Lista de datos de pedidos requeridos
    public float cantEntrega = 0; // Contador de entregas
    public PlayerVS2 player2; // Referencia al jugador 2

    private bool jugadorDentro = false; // Variable que indica si el jugador está dentro del área

    private void OnTriggerEnter(Collider other)
    {
        if (other == jugadorCollider)
        {
            jugadorDentro = true; // El jugador está dentro del área
            Debug.Log("Jugador 2 ha entrado en el área de entrega.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == jugadorCollider)
        {
            jugadorDentro = false; // El jugador ha salido del área
            Debug.Log("Jugador 2 ha salido del área de entrega.");
        }
    }

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Tecla I presionada para procesar entrega.");
            ProcesarEntrega();
        }
    }

    private void ProcesarEntrega()
    {
        var playerPickUp = player2.GetComponentInChildren<PickUpItem2>();
        if (playerPickUp != null)
        {
            Debug.Log("Se encontró el componente PickUpItem2 en el jugador 2.");

            GameObject prefabJugador = playerPickUp.GetPickedPrefab();
            if (prefabJugador != null)
            {
                Debug.Log("Jugador 2 tiene un objeto en sus manos para entregar: " + prefabJugador.name);

                ItemSOHolder itemSOHolder = prefabJugador.GetComponent<ItemSOHolder>();
                if (itemSOHolder != null)
                {
                    ItemSO itemSO = itemSOHolder.itemSO;
                    if (itemSO != null)
                    {
                        Debug.Log("Se encontró ItemSO: " + itemSO.name);

                        OrderPrefabData orderData = FindOrderData(prefabJugador);
                        if (orderData != null)
                        {
                            Debug.Log("Se encontró un pedido coincidente para el objeto: " + orderData.orderSprite.name);

                            bool pedidoEliminado = orderManager.EliminarPedido(orderData.orderSprite, itemSO);
                            if (pedidoEliminado) 
                            {
                                Debug.Log("Pedido eliminado correctamente. Añadiendo dinero y aumentando contador de entregas.");
                                EliminarItemDeLasManos(player2);
                                player2.wallet.AddMoney(itemSO.valor);
                                cantEntrega += 1;
                                Debug.Log("Entrega completada. Cantidad total de entregas: " + cantEntrega);
                            }
                            else
                            {
                                Debug.LogWarning("Error al intentar eliminar el pedido en OrderManagerPlayer2.");
                                EliminarItemDeLasManos(player2); // Entrega errónea
                            }
                        }
                        else
                        {
                            Debug.LogWarning("No se encontró un pedido coincidente para el objeto en itemsRequeridos.");
                            EliminarItemDeLasManos(player2); // Entrega errónea
                        }
                    }
                    else
                    {
                        Debug.LogWarning("El itemSO en itemSOHolder es nulo.");
                    }
                }
                else
                {
                    Debug.LogWarning("El prefab en manos del jugador no contiene un componente ItemSOHolder.");
                }
            }
            else
            {
                Debug.LogWarning("El jugador 2 no tiene ningún objeto en manos.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el componente PickUpItem2 en el jugador 2.");
        }
    }

    private void EliminarItemDeLasManos(PlayerVS2 player)
    {
        Transform hand = player.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            Debug.Log("Eliminando el objeto de las manos del jugador 2.");
            Destroy(hand.GetChild(0).gameObject);
            FindObjectOfType<PickUpItem2>().ReleaseItem();
        }
        else
        {
            Debug.LogWarning("No se encontró objeto en las manos del jugador para eliminar.");
        }
    }

    public void ActualizarItemsRequeridos()
    {
        itemsRequeridos.Clear();
        var activeOrders = orderManager.GetActiveOrders();
        foreach (OrderPrefabData pedido in activeOrders)
        {
            itemsRequeridos.Add(pedido);
            Debug.Log("Pedido actualizado en itemsRequeridos: " + pedido.orderSprite.name);
        }
    }

    private OrderPrefabData FindOrderData(GameObject prefab)
    {
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("El objeto en manos del jugador no tiene un SpriteRenderer.");
            return null;
        }

        Sprite objetoSprite = spriteRenderer.sprite;
        foreach (OrderPrefabData order in itemsRequeridos)
        {
            if (order.objectSprite == objetoSprite)
            {
                Debug.Log("Pedido coincidente encontrado para el objeto sprite: " + objetoSprite.name);
                return order;
            }
        }
        Debug.LogWarning("No se encontró un pedido coincidente para el sprite del objeto.");
        return null;
    }
}
