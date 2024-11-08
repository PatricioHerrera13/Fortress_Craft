using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{
    public Canvas tiendaCanvas;
    public Image objetoImagen;
    public Text nombreObjetoText;
    public Sprite[] objetosDisponibles;
    public GameObject[] prefabsDisponibles; // Lista de prefabs disponibles para comprar
    public Transform spawnPoint; // Nuevo transform designado para el objeto comprado
    private int indiceActual = 0;
    private bool tiendaAbierta = false;
    private bool jugadorEnRango = false;
    private MonoBehaviour jugadorActualScript; // Referencia al script del jugador actual

    private void Start()
    {
        tiendaCanvas.gameObject.SetActive(false); // La tienda comienza cerrada
    }

    private void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
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

            
            if ((jugadorActualScript is PlayerVS1 && Input.GetKeyDown(KeyCode.D)))
            {
                CambiarObjeto(1);
            }
            else if ((jugadorActualScript is PlayerVS1 && Input.GetKeyDown(KeyCode.A)))
            {
                CambiarObjeto(-1);
            }


            // Si el jugador presiona espacio, "comprar" el objeto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ComprarObjeto();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorActualScript = other.GetComponent<Player>();
            jugadorEnRango = true;
            //Debug.Log("Jugador entró en rango de la tienda.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            //Debug.Log("Jugador salió del rango de la tienda.");
        }
    }

    private void AbrirTienda()
    {
        tiendaAbierta = true;
        tiendaCanvas.gameObject.SetActive(true);
        MostrarObjetoActual();

        // Desactivar movimiento del jugador actual
        if (jugadorActualScript != null)
        {
            jugadorActualScript.enabled = false;
        }

        //Debug.Log("Tienda abierta.");
    }

    private void CerrarTienda()
    {
        tiendaAbierta = false;
        tiendaCanvas.gameObject.SetActive(false);

        // Reactivar movimiento del jugador actual
        if (jugadorActualScript != null)
        {
            jugadorActualScript.enabled = true;
        }

        //Debug.Log("Tienda cerrada.");
    }

    private void CambiarObjeto(int cambio)
    {
        indiceActual = (indiceActual + cambio + objetosDisponibles.Length) % objetosDisponibles.Length;
        MostrarObjetoActual();
    }

    private void MostrarObjetoActual()
    {
        if (objetosDisponibles.Length > 0)
        {
            Sprite objetoActual = objetosDisponibles[indiceActual];
            objetoImagen.sprite = objetoActual;
            nombreObjetoText.text = objetoActual.name;
            //Debug.Log("Mostrando objeto: " + objetoActual.name);
        }
    }

    private void ComprarObjeto()
    {
        if (indiceActual < prefabsDisponibles.Length)
        {
            if (spawnPoint != null)
            {
                GameObject objetoComprado = Instantiate(prefabsDisponibles[indiceActual], spawnPoint.position, Quaternion.identity);
                objetoComprado.transform.SetParent(spawnPoint); // Asignar el objeto al transform designado
                
                CerrarTienda(); // Cerrar la tienda después de comprar
                //Debug.Log("Objeto comprado: " + prefabsDisponibles[indiceActual].name);
            }
            else
            {
                //Debug.LogError("No se ha asignado un spawnPoint para el objeto comprado.");
            }
        }
    }
}
