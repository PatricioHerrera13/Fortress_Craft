using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManagerPlayer2 : MonoBehaviour
{
    public List<OrderPrefabData> orderPrefabList; // Lista que vincula sprites de pedidos con prefabs y objetos requeridos
    public List<Image> orderSlots; // Slots donde se mostrarán los pedidos
    public RectTransform orderPanel; // Panel que contiene los pedidos
    public float orderInterval = 10f; // Tiempo en segundos entre pedidos
    public int maxOrders = 3; // Cantidad máxima de pedidos

    private List<Order> activeOrders = new List<Order>();

    private void Start()
    {
        StartCoroutine(GenerateOrders());
    }

    private IEnumerator GenerateOrders()
    {
        while (true)
        {
            yield return new WaitForSeconds(orderInterval);
            if (activeOrders.Count < maxOrders)
            {
                AddNewOrder();
            }
        }
    }

    private void AddNewOrder()
    {
        if (activeOrders.Count >= orderSlots.Count)
        {
            return; // Si ya se alcanzó el máximo de pedidos, no hacer nada
        }

        // Selecciona una nueva orden aleatoria de los prefabs
        OrderPrefabData newOrderData = orderPrefabList[Random.Range(0, orderPrefabList.Count)];
        Sprite newOrderSprite = newOrderData.orderSprite;

        // Encuentra el primer slot disponible
        for (int i = 0; i < orderSlots.Count; i++)
        {
            if (!IsSlotOccupied(orderSlots[i]))
            {
                orderSlots[i].sprite = newOrderSprite;
                orderSlots[i].gameObject.SetActive(true);
                
                // Crear nuevo pedido y comenzar su temporizador
                Order newOrder = new Order(orderSlots[i], newOrderData, 30f);
                activeOrders.Add(newOrder);
                StartCoroutine(newOrder.StartOrderTimer(() => RemoveOrder(newOrder)));

                AdjustOrderPositions();
                // Llama a un método que actualiza los elementos requeridos en el portal para cada jugador
                FindObjectOfType<PortalVS2>().ActualizarItemsRequeridos();
                break;
            }
        }
    }

    private bool IsSlotOccupied(Image slot)
    {
        foreach (Order order in activeOrders)
        {
            if (order.slot == slot)
            {
                return true;
            }
        }
        return false;
    }

    private void RemoveOrder(Order order)
    {
        order.slot.gameObject.SetActive(false);
        activeOrders.Remove(order);
        AdjustOrderPositions();
    }

    private void AdjustOrderPositions()
    {
        float panelWidth = orderPanel.rect.width;
        float totalOrdersWidth = activeOrders.Count * 30f;
        float spacing = Mathf.Min((panelWidth - totalOrdersWidth) / (activeOrders.Count + 1), 20f);

        for (int i = 0; i < activeOrders.Count; i++)
        {
            RectTransform orderTransform = activeOrders[i].slot.GetComponent<RectTransform>();
            float newXPosition = spacing * (i + 1) + 20f * i;
            Vector3 newPosition = new Vector3(newXPosition, orderTransform.anchoredPosition.y, 0);
            orderTransform.anchoredPosition = newPosition;
            orderTransform.sizeDelta = new Vector2(65f, 65f);
        }
    }

    public bool EliminarPedido(Sprite pedidoSprite, ItemSO itemSO)
    {
        foreach (Order order in activeOrders)
        {
            if (order.slot.sprite == pedidoSprite && order.orderData.itemData == itemSO)
            {
                order.slot.gameObject.SetActive(false);
                activeOrders.Remove(order);
                AdjustOrderPositions();
                FindObjectOfType<PortalVS2>().ActualizarItemsRequeridos();
                return true;
            }
        }
        return false;
    }

    public List<OrderPrefabData> GetActiveOrders()
    {
        List<OrderPrefabData> activeOrdersList = new List<OrderPrefabData>();
        foreach (Order order in activeOrders)
        {
            if (order.slot.gameObject.activeSelf)
            {
                activeOrdersList.Add(order.orderData);
            }
        }
        return activeOrdersList;
    }

    private class Order
    {
        public Image slot;
        public OrderPrefabData orderData;
        private float timeRemaining;
        private RectTransform progressBar;
        private float initialDuration;
        private float initialWidth;

        public Order(Image slot, OrderPrefabData orderData, float duration)
        {
            this.slot = slot;
            this.orderData = orderData;
            this.timeRemaining = duration;
            this.initialDuration = duration;

            // Configuración de la barra de progreso
            progressBar = new GameObject("ProgressBar").AddComponent<Image>().rectTransform;
            progressBar.SetParent(slot.transform);
            progressBar.sizeDelta = new Vector2(100f, 20f);
            progressBar.anchoredPosition = new Vector2(0, -35f);
            progressBar.GetComponent<Image>().color = Color.green;

            initialWidth = progressBar.sizeDelta.x;
        }

        public IEnumerator StartOrderTimer(System.Action onTimeUp)
        {
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                float width = initialWidth * (timeRemaining / initialDuration);
                progressBar.sizeDelta = new Vector2(width, progressBar.sizeDelta.y);

                yield return null;
            }

            Object.Destroy(progressBar.gameObject);
            onTimeUp.Invoke();
        }
    }
}
