using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrderManagerPlayer1 : MonoBehaviour
{
    public List<Sprite> possibleOrders; // Lista de imágenes posibles para los pedidos
    public List<Image> orderSlots; // Los slots donde se mostrarán los pedidos
    public RectTransform orderPanel; // El panel que contiene los pedidos
    public float orderInterval = 10f; // Tiempo en segundos entre la aparición de cada pedido
    public int maxOrders = 3; // Cantidad máxima de pedidos
    public List<OrderPrefabData> orderPrefabList; // Lista que vincula sprites de pedidos con prefabs
    //public Button back;

    // Nueva referencia a la billetera
    public Wallet1 wallet; // Asegúrate de arrastrar el componente Wallet en el Inspector

    private List<Order> activeOrders = new List<Order>();

    private void Start()
    {
        StartCoroutine(GenerateOrders());
        //back.onClick.AddListener(atras);
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
                FindObjectOfType<PortalVS1>().ActualizarItemsRequeridos();
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
        
        // Descontar 5 de la billetera
        wallet.DeductFromWallet(1f);

        AdjustOrderPositions();
    }

    private void AdjustOrderPositions()
    {
        float panelWidth = orderPanel.rect.width;
        float totalOrdersWidth = activeOrders.Count * 50f;
        float spacing = Mathf.Min((panelWidth - totalOrdersWidth) / (activeOrders.Count + 1), 50f);

        for (int i = 0; i < activeOrders.Count; i++)
        {
            RectTransform orderTransform = activeOrders[i].slot.GetComponent<RectTransform>();
            float newXPosition = spacing * (i + 1) + 50f * i;
            Vector3 newPosition = new Vector3(newXPosition, orderTransform.anchoredPosition.y, 0);
            orderTransform.anchoredPosition = newPosition;
            orderTransform.sizeDelta = new Vector2(50f, 50f);
        }
    }

    public bool EliminarPedido(Sprite pedidoSprite)
    {
        foreach (Order order in activeOrders)
        {
            if (order.slot.sprite == pedidoSprite)
            {
                order.slot.gameObject.SetActive(false);
                activeOrders.Remove(order);
                AdjustOrderPositions();
                
                FindObjectOfType<PortalVS1>().ActualizarItemsRequeridos();
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
            progressBar.sizeDelta = new Vector2(40f, 5f); // Tamaño inicial
            progressBar.anchoredPosition = new Vector2(0, -25f); // Posición debajo de la imagen
            progressBar.GetComponent<Image>().color = Color.green;

            initialWidth = progressBar.sizeDelta.x; // Ancho inicial
        }

        public IEnumerator StartOrderTimer(System.Action onTimeUp)
        {
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                // Actualizar el ancho de la barra de progreso
                float width = initialWidth * (timeRemaining / initialDuration);
                progressBar.sizeDelta = new Vector2(width, progressBar.sizeDelta.y);

                yield return null;
            }

            // Eliminar la barra de progreso y notificar el final del tiempo
            Object.Destroy(progressBar.gameObject);
            onTimeUp.Invoke();
        }
    }
}
