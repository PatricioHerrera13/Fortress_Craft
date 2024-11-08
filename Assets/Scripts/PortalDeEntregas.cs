using System.Collections.Generic;
using UnityEngine;

public class PortalDeEntregas : MonoBehaviour
{
    public OrderManager orderManager; // Referencia al script OrderManager
    public Collider jugadorCollider1; // Collider del jugador 1
    public Collider jugadorCollider2; // Collider del jugador 2
    public List<OrderPrefabData> itemsRequeridos; // Lista de datos de pedidos requeridos
    public float cantEntrega = 0; // Contador de entregas
    public Player player1; // Referencia al jugador 1
    public Player2 player2; // Referencia al segundo jugador

    private bool jugador1Dentro = false; // Variable que indica si el jugador 1 está dentro del área
    private bool jugador2Dentro = false; // Variable que indica si el jugador 2 está dentro del área

    private void OnTriggerEnter(Collider other)
    {
        if (other == jugadorCollider1)
        {
            jugador1Dentro = true; // El jugador 1 está dentro del área
        }
        else if (other == jugadorCollider2)
        {
            ////Debug.Log("2");
            jugador2Dentro = true; // El jugador 2 está dentro del área
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == jugadorCollider1)
        {
            jugador1Dentro = false; // El jugador 1 ha salido del área
        }
        else if (other == jugadorCollider2)
        {
            ////Debug.Log("-2");
            jugador2Dentro = false; // El jugador 2 ha salido del área
        }
    }

    private void Update()
    {
        if (jugador1Dentro && Input.GetKeyDown(KeyCode.X))
        {
            ProcesarEntrega(player1);
        }
        else if (jugador2Dentro && Input.GetKeyDown(KeyCode.I))
        {
            //Debug.Log("2!!");
            ProcesarEntrega(player2);
        }
    }

    private void ProcesarEntrega(Player jugador)
    {
        // Obtener el script PickUpItem del jugador 1
        var playerPickUp = jugador.GetComponentInChildren<PickUpItem>();
        if (playerPickUp != null)
        {
            GameObject prefabJugador = playerPickUp.GetPickedPrefab(); // Obtener el prefab recogido

            if (prefabJugador != null)
            {
                ItemSOHolder itemSOHolder = prefabJugador.GetComponent<ItemSOHolder>(); // Obtener el ItemSOHolder
                if (itemSOHolder != null)
                {
                    ItemSO itemSO = itemSOHolder.itemSO; // Obtener el ScriptableObject ItemSO desde el ItemSOHolder

                    if (itemSO != null)
                    {
                        OrderPrefabData orderData = FindOrderData(prefabJugador); // Buscar el pedido relacionado

                        if (orderData != null && orderManager.EliminarPedido(orderData.orderSprite))
                        {
                            // Eliminar el prefab de las manos del jugador
                            EliminarItemDeLasManos(jugador);

                            // Sumar dinero del valor del ItemSO
                            jugador.wallet.AddMoney(itemSO.valor); // Sumar el valor del ItemSO a la billetera del jugador actual
                            cantEntrega += 1;
                        }
                        else
                        {
                            // Entrega errónea. Elimina el ítem
                            EliminarItemDeLasManos(jugador);
                        }
                    }
                }
            }
        }
    }

    private void ProcesarEntrega(Player2 jugador2)
    {
        //Debug.Log("Entregando");
        // Obtener el script PickUpItem2 del jugador 2
        var playerPickUp = jugador2.GetComponentInChildren<PickUpItem2>();
        //Debug.Log(playerPickUp);
        if (playerPickUp != null)
        {
            GameObject prefabJugador = playerPickUp.GetPickedPrefab(); // Obtener el prefab recogido
            //Debug.Log(prefabJugador);
            if (prefabJugador != null)
            {
                ItemSOHolder itemSOHolder = prefabJugador.GetComponent<ItemSOHolder>(); // Obtener el ItemSOHolder
                //Debug.Log(itemSOHolder);
                if (itemSOHolder != null)
                {
                    ItemSO itemSO = itemSOHolder.itemSO; // Obtener el ScriptableObject ItemSO desde el ItemSOHolder
                    //Debug.Log(itemSO);
                    if (itemSO != null)
                    {
                        OrderPrefabData orderData = FindOrderData(prefabJugador); // Buscar el pedido relacionado
                        //Debug.Log(orderData);
                        if (orderData != null && orderManager.EliminarPedido(orderData.orderSprite))
                        {
                            // Eliminar el prefab de las manos del jugador
                            EliminarItemDeLasManos(jugador2);

                            // Sumar dinero del valor del ItemSO
                            jugador2.wallet.AddMoney(itemSO.valor); // Sumar el valor del ItemSO a la billetera del jugador actual
                            cantEntrega += 1;
                            //Debug.Log("Entregado!!!");
                        }
                        else
                        {
                            // Entrega errónea. Elimina el ítem
                            EliminarItemDeLasManos(jugador2);
                            //Debug.Log("Mal Entregado");
                        }
                    }
                }
            }
        }
    }

    private void EliminarItemDeLasManos(Player player)
    {
        Transform hand = player.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            Destroy(hand.GetChild(0).gameObject);
            FindObjectOfType<PickUpItem>().ReleaseItem();
        }
    }

    private void EliminarItemDeLasManos(Player2 jugador2)
    {
        Transform hand = jugador2.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            Destroy(hand.GetChild(0).gameObject);
            FindObjectOfType<PickUpItem2>().ReleaseItem();
        }
    }

    public void ActualizarItemsRequeridos()
    {
        itemsRequeridos.Clear(); // Limpiar la lista antes de actualizar
        var activeOrders = orderManager.GetActiveOrders(); // Obtener las órdenes activas

        foreach (OrderPrefabData pedido in activeOrders)
        {
            itemsRequeridos.Add(pedido); // Agregar el pedido completo (datos del sprite, prefab, etc.)
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
