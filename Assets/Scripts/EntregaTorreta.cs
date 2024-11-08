using System.Collections.Generic;
using UnityEngine;

public class EntregaTorreta : MonoBehaviour
{
    public List<ItemRequerido> itemsRequeridos; // Lista de objetos requeridos
    public Torreta torretaScript; // Referencia al script Torreta

    private HashSet<string> itemsEntregados = new HashSet<string>(); // Conjunto para controlar los items entregados
    private bool canDeliver = false; // Controla si el jugador puede entregar
    private GameObject currentPlayer; // Referencia al jugador dentro del collider

    private void Start()
    {
        if (torretaScript != null)
        {
            torretaScript.enabled = false; // Asegúrate de que el script Torreta esté desactivado al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra es el jugador
        {
            canDeliver = true; // Permite la entrega cuando el jugador entra
            currentPlayer = other.gameObject; // Almacena el jugador que entró
            Debug.Log("Jugador dentro del collider. Puede entregar objetos.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que sale es el jugador
        {
            canDeliver = false; // Desactiva la entrega cuando el jugador sale
            currentPlayer = null; // Limpia la referencia del jugador
            Debug.Log("Jugador fuera del collider. No puede entregar objetos.");
        }
    }

    private void Update()
    {
        if ((canDeliver && Input.GetKeyDown(KeyCode.X)) || (canDeliver && Input.GetKeyDown(KeyCode.I))) // Cambia la tecla según lo necesites
        {
            if (currentPlayer != null)
            {
                // Obtén el objeto en la mano del jugador actual
                Transform hand = currentPlayer.transform.Find("Hand"); // Encuentra el objeto 'hand'
                Transform handpoint = hand != null ? hand.Find("HandPoint") : null; // Encuentra el 'handpoint' dentro de 'hand'
                GameObject objetoEnMano = handpoint != null && handpoint.childCount > 0 ? handpoint.GetChild(0).gameObject : null;

                if (objetoEnMano != null)
                {
                    Item itemComponente = objetoEnMano.GetComponent<Item>(); // Obtén el componente Item
                    if (itemComponente != null)
                    {
                        string tipoPrefab = itemComponente.itemType; // Usa itemType en lugar del nombre
                        Debug.Log($"Intentando entregar el prefab de tipo: {tipoPrefab}");

                        // Llama a RemoverPrefabDelHandPoint en el jugador actual
                        if (currentPlayer.GetComponent<Player>() != null)
                        {
                            currentPlayer.GetComponent<Player>().RemoverPrefabDelHandPoint();
                        }
                        else if (currentPlayer.GetComponent<Player2>() != null)
                        {
                            currentPlayer.GetComponent<Player2>().RemoverPrefabDelHandPoint();
                        }

                        EntregarPrefab(tipoPrefab); // Verifica la entrega
                    }
                    else
                    {
                        Debug.Log("El objeto en la mano no tiene un componente Item.");
                    }
                }
                else
                {
                    Debug.Log("No hay objeto en la mano del jugador.");
                }
            }
        }
    }

    public void EntregarPrefab(string prefabTipo)
    {
        // Verifica si el tipo de prefab es uno de los requeridos y no ha sido entregado
        if (itemsRequeridos.Count > 0)
        {
            Debug.Log("Comenzando a verificar la entrega del prefab de tipo: " + prefabTipo);
            for (int i = itemsRequeridos.Count - 1; i >= 0; i--) // Iterar de atrás hacia adelante
            {
                Debug.Log("Comparando con objeto requerido de tipo: " + itemsRequeridos[i].tipo);
                if (itemsRequeridos[i].tipo == prefabTipo)
                {
                    if (!itemsEntregados.Contains(prefabTipo))
                    {
                        itemsEntregados.Add(prefabTipo); // Agrega el prefab a los entregados
                        Debug.Log($"Objeto entregado: {prefabTipo}"); // Mensaje de depuración

                        // Elimina el objeto de la lista de requeridos
                        itemsRequeridos.RemoveAt(i);
                        Debug.Log($"Objeto requerido eliminado: {prefabTipo}. Quedan {itemsRequeridos.Count} objetos requeridos."); // Mensaje de depuración

                        // Comprueba si todos los items requeridos han sido entregados
                        if (itemsRequeridos.Count == 0) // Si la lista de objetos requeridos está vacía
                        {
                            ActivarTorreta(); // Activa el script Torreta
                        }
                        break; // Salir del bucle una vez que se ha entregado correctamente
                    }
                    else
                    {
                        Debug.Log($"El objeto {prefabTipo} ya ha sido entregado.");
                    }
                }
            }
        }
        else
        {
            Debug.Log("No hay más objetos requeridos por entregar.");
        }
    }

    private void ActivarTorreta()
    {
        if (torretaScript != null)
        {
            torretaScript.enabled = true; // Activa el script Torreta
            Debug.Log("Todos los prefabs requeridos han sido entregados. Torreta activada.");
        }
    }
}

[System.Serializable]
public class ItemRequerido
{
    public string tipo; // Tipo de objeto requerido
}
