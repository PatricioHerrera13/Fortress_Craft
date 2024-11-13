using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{
    public List<Sprite> possibleOrders; // Lista de imágenes posibles para los pedidos en fase 1
    public List<Image> orderSlots; // Los slots donde se mostrarán los pedidos en fase 1
    public RectTransform orderPanel; // El panel que contiene los pedidos
    public float orderInterval = 6f; // Tiempo en segundos entre la aparición de cada pedido
    public int maxOrders = 5; // Cantidad máxima de pedidos
    public List<OrderPrefabData> orderPrefabList; // Lista que vincula sprites de pedidos con prefabs en fase 1
    public Button back; // Botón para regresar al menú

    public List<Sprite> possibleOrders2; // Lista de imágenes posibles para los pedidos en fase 2
    public List<Image> orderSlots2; // Los slots donde se mostrarán los pedidos en fase 2
    public List<OrderPrefabData> orderPrefabList2; // Lista que vincula sprites de pedidos con prefabs en fase 2

    public Wallet wallet; 

    public Collider worldColliderFase2;
    public Collider worldColliderFase3;
    public Collider worldColliderFase4;

    private List<Order> activeOrders = new List<Order>();
    public bool fase2 = false;
    public bool fase3 = false;
    public bool fase4 = false;

    //public float rest = 1f;

    private void Start()
    {
        StartCoroutine(GenerateOrders());
        back.onClick.AddListener(atras);
    }

    public void SetPhase(int phase)
    {
        // Resetea los valores de fase para evitar conflictos
        fase2 = false;
        fase3 = false;
        fase4 = false;

        // Activa la fase correspondiente según el valor de 'phase'
        if (phase == 2)
        {
            fase2 = true;
        }
        else if (phase == 3)
        {
            fase3 = true;
        }
        else if (phase == 4)
        {
            fase4 = true;
        }

        // Actualiza las listas de pedidos de acuerdo a la fase
        UpdateOrderListVisibility();
    }

    // Método auxiliar para activar o desactivar las listas de pedidos
    private void UpdateOrderListVisibility()
    {
        // Desactiva todos los slots de fase 1
        foreach (var slot in orderSlots)
        {
            slot.gameObject.SetActive(!fase2 && !fase3 && !fase4); // Activo solo en fase 1
        }

        // Desactiva todos los slots de fase 2
        foreach (var slot in orderSlots2)
        {
            slot.gameObject.SetActive(fase2 || fase3 || fase4); // Activo en fase 2 o superior
        }
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
        List<Image> currentSlots;

        if (fase2)
        {
            Debug.Log("Generando pedido de la Fase 2");
            currentSlots = orderSlots2;
        }
        else if (fase3 || fase4)
        {
            Debug.Log("Generando pedido de la Fase 3 o 4");
            currentSlots = new List<Image>(orderSlots);
            currentSlots.AddRange(orderSlots2);
        }
        else
        {
            Debug.Log("Generando pedido de la Fase 1");
            currentSlots = orderSlots;
        }

        if (activeOrders.Count >= currentSlots.Count)
        {
            return;
        }

        OrderPrefabData newOrderData;
        Sprite newOrderSprite;

        if (fase2)
        {
            newOrderData = orderPrefabList2[Random.Range(0, orderPrefabList2.Count)];
            newOrderSprite = newOrderData.orderSprite;

            for (int i = 0; i < orderSlots2.Count; i++)
            {
                if (!IsSlotOccupied(orderSlots2[i]))
                {
                    orderSlots2[i].sprite = newOrderSprite;
                    orderSlots2[i].gameObject.SetActive(true);

                    Order newOrder = new Order(orderSlots2[i], newOrderData, 30f);
                    activeOrders.Add(newOrder);
                    StartCoroutine(newOrder.StartOrderTimer(() => RemoveOrder(newOrder)));

                    AdjustOrderPositions();
                    FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
                    break;
                }
            }
        }
        else if (fase3 || fase4)
        {
            List<OrderPrefabData> combinedOrderList = new List<OrderPrefabData>();
            combinedOrderList.AddRange(orderPrefabList);
            combinedOrderList.AddRange(orderPrefabList2);

            newOrderData = combinedOrderList[Random.Range(0, combinedOrderList.Count)];
            newOrderSprite = newOrderData.orderSprite;

            List<Image> combinedSlots = new List<Image>(orderSlots);
            combinedSlots.AddRange(orderSlots2);

            for (int i = 0; i < combinedSlots.Count; i++)
            {
                if (!IsSlotOccupied(combinedSlots[i]))
                {
                    combinedSlots[i].sprite = newOrderSprite;
                    combinedSlots[i].gameObject.SetActive(true);

                    Order newOrder = new Order(combinedSlots[i], newOrderData, 30f);
                    activeOrders.Add(newOrder);
                    StartCoroutine(newOrder.StartOrderTimer(() => RemoveOrder(newOrder)));

                    AdjustOrderPositions();
                    FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
                    break;
                }
            }
        }
        else
        {
            newOrderData = orderPrefabList[Random.Range(0, orderPrefabList.Count)];
            newOrderSprite = newOrderData.orderSprite;

            for (int i = 0; i < orderSlots.Count; i++)
            {
                if (!IsSlotOccupied(orderSlots[i]))
                {
                    orderSlots[i].sprite = newOrderSprite;
                    orderSlots[i].gameObject.SetActive(true);

                    Order newOrder = new Order(orderSlots[i], newOrderData, 100f);
                    activeOrders.Add(newOrder);
                    StartCoroutine(newOrder.StartOrderTimer(() => RemoveOrder(newOrder)));

                    AdjustOrderPositions();
                    FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
                    break;
                }
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

    public void RemoveOrder(Order order)
    {
        order.slot.gameObject.SetActive(false);
        activeOrders.Remove(order);
        
        // Deducir dinero solo si el pedido no se cumplió en el tiempo
        if (order.timeRemaining <= 0)
        {
            //wallet.DeductFromWallet(1f); // Deduce dinero solo si el pedido falló
            Debug.Log("Pedido fallido. Dinero descontado.");
        }
        else
        {
            Debug.Log("Pedido completado. No se descontó dinero.");
        }

        AdjustOrderPositions();
    }

    private void AdjustOrderPositions()
    {
        float panelWidth = orderPanel.rect.width;
        float totalOrdersWidth = activeOrders.Count * 50f;
        float spacing = Mathf.Min((panelWidth - totalOrdersWidth) / (activeOrders.Count + 1), 150f);

        for (int i = 0; i < activeOrders.Count; i++)
        {
            RectTransform orderTransform = activeOrders[i].slot.GetComponent<RectTransform>();
            float newXPosition = spacing * (i + 1) + 100f * i;
            Vector3 newPosition = new Vector3(newXPosition, orderTransform.anchoredPosition.y, 0);
            orderTransform.anchoredPosition = newPosition;
            orderTransform.sizeDelta = new Vector2(35f, 35f);
        }
    }

    public bool EliminarPedido(Sprite pedidoSprite)
    {
        foreach (Order order in activeOrders)
        {
            if (order.slot.sprite == pedidoSprite)
            {
                RemoveOrder(order);
                FindObjectOfType<PortalDeEntregas>().ActualizarItemsRequeridos();
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

    void atras()
    {
        SceneManager.LoadScene("MENU");
    }

    public void ActivatePhase1Lists()
    {
        foreach (var slot in orderSlots)
        {
            slot.gameObject.SetActive(true);
        }
        foreach (var slot in orderSlots2)
        {
            slot.gameObject.SetActive(false);
        }

        // Eliminar pedidos de la fase 2 si existen
        RemoveOrdersFromSlots(orderSlots2);
        
        AdjustOrderPositions();
    }

    public void ActivatePhase2Lists()
    {
        foreach (var slot in orderSlots)
        {
            slot.gameObject.SetActive(false);
        }
        foreach (var slot in orderSlots2)
        {
            slot.gameObject.SetActive(true);
        }

        // Eliminar pedidos de la fase 1 si existen
        RemoveOrdersFromSlots(orderSlots);
        
        AdjustOrderPositions();
    }

    private void RemoveOrdersFromSlots(List<Image> slotsToRemove)
    {
        List<Order> ordersToRemove = new List<Order>();

        foreach (Order order in activeOrders)
        {
            if (slotsToRemove.Contains(order.slot))
            {
                ordersToRemove.Add(order);
            }
        }

        foreach (Order order in ordersToRemove)
        {
            RemoveOrder(order);
        }
    }

    public class Order
    {
        public Image slot;
        public OrderPrefabData orderData;
        public float timeRemaining;
        private RectTransform progressBar;
        private float initialDuration;
        private float initialWidth;

        public Order(Image slot, OrderPrefabData orderData, float duration)
        {
            this.slot = slot;
            this.orderData = orderData;
            this.timeRemaining = duration;
            this.initialDuration = duration;

            progressBar = new GameObject("ProgressBar").AddComponent<Image>().rectTransform;
            progressBar.SetParent(slot.transform);
            progressBar.sizeDelta = new Vector2(90f, 15f);
            progressBar.anchoredPosition = new Vector2(0, -20f);
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
