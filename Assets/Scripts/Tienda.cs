using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{
    public Canvas tiendaCanvas;
    public Image objetoImagen;
    public Text precio;
    
    public Sprite[] objetosDisponibles;
    public GameObject[] prefabsDisponibles; // Lista de prefabs disponibles para comprar
    public ItemSO[] itemsSO;
    public Transform spawnPoint; // Nuevo transform designado para el objeto comprado
    private int indiceActual = 0;
    private bool tiendaAbierta = false;
    private bool jugadorEnRango = false;
    private MonoBehaviour jugadorActualScript; // Referencia al script del jugador actual
    public Wallet wallet; // Referencia al Wallet1

    private void Start()
    {
        tiendaCanvas.gameObject.SetActive(false); // La tienda comienza cerrada
    }

    private void Update()
    {
        if (jugadorEnRango && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.X)))
        {
            if (!tiendaAbierta)
            {
                AbrirTienda();
            }
            else
            {
                CerrarTienda();
            }
        }

        // Si la tienda está abierta, cambiar de objeto con teclas de movimiento correspondientes
        if (tiendaAbierta)
        {
            if ((jugadorActualScript is Player && Input.GetKeyDown(KeyCode.D)) || (jugadorActualScript is Player2 && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                CambiarObjeto(1);
            }
            else if ((jugadorActualScript is Player && Input.GetKeyDown(KeyCode.A)) || (jugadorActualScript is Player2 && Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                CambiarObjeto(-1);
            }

            // Si el jugador presiona espacio, "comprar" el objeto
            if ((jugadorActualScript is Player && Input.GetKeyDown(KeyCode.C)) || (jugadorActualScript is Player2 && Input.GetKeyDown(KeyCode.O)))
            {
                ComprarObjeto();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                jugadorActualScript = playerScript;
                Debug.Log("El jugador que entró es de tipo Player.");
            }
            else
            {
                var player2Script = other.GetComponent<Player2>();
                if (player2Script != null)
                {
                    jugadorActualScript = player2Script;
                    Debug.Log("El jugador que entró es de tipo Player2.");
                }
            }
        
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }

    private void AbrirTienda()
    {
        tiendaAbierta = true;
        tiendaCanvas.gameObject.SetActive(true);
        if (jugadorActualScript != null)
        {
            jugadorActualScript.enabled = false;
        }
        MostrarObjetoActual();
    }

    private void CerrarTienda()
    {
        tiendaAbierta = false;
        tiendaCanvas.gameObject.SetActive(false);

        if (jugadorActualScript != null)
        {
            jugadorActualScript.enabled = true;
        }
    }

    private void CambiarObjeto(int cambio)
    {
        indiceActual = (indiceActual + cambio + objetosDisponibles.Length) % objetosDisponibles.Length;
        MostrarObjetoActual();
        Debug.Log("Objeto cambiado a índice: " + indiceActual);
    }

    private void MostrarObjetoActual()
    {
        // Verificar que los índices estén dentro de rango y los arrays tengan contenido
        if (objetosDisponibles.Length > 0 && itemsSO.Length > 0 && indiceActual < objetosDisponibles.Length && indiceActual < itemsSO.Length)
        {
            Sprite objetoActual = objetosDisponibles[indiceActual];
            ItemSO itemActualSO = itemsSO[indiceActual];

            objetoImagen.sprite = objetoActual;
            precio.text = "$" + itemActualSO.valor.ToString("F2");

            Debug.Log("Mostrando objeto: " + itemActualSO.itemName + " con precio: $" + itemActualSO.valor);
        }
        else
        {
            Debug.LogError("Error al mostrar el objeto: Verifica que los arrays objetosDisponibles y itemsSO están correctamente configurados y tienen contenido.");
        }
    }

    private void ComprarObjeto()
    {
        if (indiceActual < itemsSO.Length) // Verifica que el índice esté dentro del rango de itemsSO
        {
            ItemSO itemActualSO = itemsSO[indiceActual];
            float precioObjeto = itemActualSO.valor;

            if (wallet.GetMoney() >= precioObjeto)
            {
                wallet.DeductFromWallet(precioObjeto);

                if (indiceActual < prefabsDisponibles.Length && spawnPoint != null)
                {
                    GameObject objetoComprado = Instantiate(prefabsDisponibles[indiceActual], spawnPoint.position, Quaternion.identity);
                    objetoComprado.transform.SetParent(spawnPoint);

                    Debug.Log("Objeto comprado: " + itemActualSO.itemName);
                    CerrarTienda();
                }
                else
                {
                    Debug.LogError("No se ha asignado un spawnPoint o prefabsDisponibles no contiene el objeto.");
                }
            }
            else
            {
                Debug.Log("No tienes suficiente dinero para comprar este objeto.");
            }
        }
    }
}
