using UnityEngine;
using UnityEngine.UI;

public class Tienda2 : MonoBehaviour
{
    public Canvas tiendaCanvas;
    public Image objetoImagen;
    public Text precio;
    //public Text nombreObjetoText;
    public Sprite[] objetosDisponibles;
    public GameObject[] prefabsDisponibles;
    public ItemSO[] itemsSO;
    public Transform spawnPoint;
    public Wallet2 wallet; // Referencia al Wallet1

    private int indiceActual = 0;
    private bool tiendaAbierta = false;
    private bool jugadorEnRango = false;
    private MonoBehaviour jugadorActualScript;

    private void Start()
    {
        tiendaCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.I))
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

        if (tiendaAbierta)
        {
            if ((jugadorActualScript is PlayerVS2 && Input.GetKeyDown(KeyCode.RightArrow)) || (jugadorActualScript is Player2 && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                CambiarObjeto(1);
            }
            else if ((jugadorActualScript is PlayerVS2 && Input.GetKeyDown(KeyCode.LeftArrow)) || (jugadorActualScript is Player2 && Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                CambiarObjeto(-1);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                ComprarObjeto();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorActualScript = other.GetComponent<PlayerVS2>();
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
        MostrarObjetoActual();

        // Desactiva el script de movimiento del jugador
        if (jugadorActualScript != null)
        {
            jugadorActualScript.enabled = false;
        }
    }

    private void CerrarTienda()
    {
        tiendaAbierta = false;
        tiendaCanvas.gameObject.SetActive(false);

        // Reactiva el script de movimiento del jugador
        if (jugadorActualScript != null)
        {
            jugadorActualScript.enabled = true;
        }
    }

    private void CambiarObjeto(int cambio)
    {
        indiceActual = (indiceActual + cambio + objetosDisponibles.Length) % objetosDisponibles.Length;
        MostrarObjetoActual();
    }

    private void MostrarObjetoActual()
    {
        if (objetosDisponibles.Length > 0 && itemsSO.Length > 0 && indiceActual < itemsSO.Length)
        {
            Sprite objetoActual = objetosDisponibles[indiceActual];
            ItemSO itemActualSO = itemsSO[indiceActual];

            objetoImagen.sprite = objetoActual;
            precio.text = "$" + itemActualSO.valor.ToString("F2");
            //nombreObjetoText.text = itemActualSO.itemName;
        }
    }

    private void ComprarObjeto()
    {
        if (indiceActual < itemsSO.Length) // Verifica que el índice esté dentro del rango de itemsSO
        {
            ItemSO itemActualSO = itemsSO[indiceActual];
            float precioObjeto = itemActualSO.valor;

            // Verifica si el jugador tiene suficiente dinero en el Wallet
            if (wallet.GetMoney() >= precioObjeto)
            {
                // Si tiene suficiente dinero, descuenta y compra
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
                // Aquí podrías mostrar un mensaje en la UI indicando falta de dinero
            }
        }
    }
}
