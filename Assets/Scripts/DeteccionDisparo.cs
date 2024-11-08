using System.Collections;
using UnityEngine;

public class DeteccionDisparo : MonoBehaviour
{
    public Collider zonaRecarga;
    public GameObject cañon1;  // Cañón izquierdo para player 1
    public Collider p1;  // Collider del jugador 1
    public float VelLen = 1f;  // Velocidad lenta (movimiento hacia atrás)
    public float VelRap = 3f;  // Velocidad rápida (movimiento hacia adelante)
    public float TiempoCarga = 2f;  // Tiempo de carga antes de disparar
    private Vector3 dirIzquierda = Vector3.left;  // Movimiento hacia atrás para cañón izquierdo
    private Vector3 posInicialCañon;  // Posición inicial del cañón izquierdo
    public GameObject proyectil;
    public Transform puntoDisparo1;  // Punto de salida del proyectil para cañón
    public float fuerzaDisparo = 20f;  // Fuerza con la que el proyectil será disparado
    public Transform handPoint;  // Punto en la mano del jugador donde se comprueba el prefab
    public GameObject prefabRequerido;  // Prefab que debe estar en el HandPoint

    private bool haDisparado = false;  // Flag para evitar múltiples disparos
    private bool enZonaRecarga = false;  // Flag para saber si el jugador está en la zona

    void Start()
    {
        Debug.Log("Pos. Inicial Lista para Cañón 1");
        posInicialCañon = cañon1.transform.position;
    }

    // Se detecta cuando el jugador entra en la zona de recarga
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other == p1)  // Verificamos que sea el jugador 1
        {
            enZonaRecarga = true;  // El jugador está en la zona de recarga
            Debug.Log("Player 1 ha entrado en la zona de recarga");
        }
    }

    // Se detecta cuando el jugador sale de la zona de recarga
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other == p1)
        {
            enZonaRecarga = false;  // El jugador ha salido de la zona de recarga
            Debug.Log("Player 1 ha salido de la zona de recarga");
        }
    }

    void Update()
    {
        if (enZonaRecarga && Input.GetKey(KeyCode.X) && !haDisparado && VerificarPrefabEnManos())
        {
            Debug.Log("Player 1 interactúa con el cañón y tiene el prefab requerido en las manos");
            StartCoroutine(MoverCañon(cañon1, dirIzquierda, posInicialCañon, puntoDisparo1, Vector3.left));
        }
    }

    // Verifica si el jugador tiene el prefab requerido en el HandPoint
    bool VerificarPrefabEnManos()
    {
        Debug.Log("Verificando objeto en el HandPoint...");

        if (handPoint.childCount > 0)  // Verifica si hay algún objeto en el handPoint
        {
            GameObject objetoEnManos = handPoint.GetChild(0).gameObject;  // Obtiene el objeto en las manos
            Debug.Log("Objeto encontrado en el HandPoint: " + objetoEnManos.name);

            SpriteRenderer spriteEnManos = objetoEnManos.GetComponent<SpriteRenderer>();
            SpriteRenderer spriteRequerido = prefabRequerido.GetComponent<SpriteRenderer>();

            if (spriteEnManos != null && spriteRequerido != null)
            {
                // Compara los sprites del objeto en manos con el prefab requerido
                if (spriteEnManos.sprite == spriteRequerido.sprite)
                {
                    Debug.Log("El objeto en las manos tiene el sprite requerido.");
                    Destroy(handPoint.GetChild(0).gameObject);  // Elimina el objeto en las manos
                    FindObjectOfType<PickUpItem>().ReleaseItem();
                    Debug.Log("Prefab requerido eliminado de las manos del jugador.");
                    return true;
                }
                else
                {
                    Debug.LogWarning("El objeto en las manos NO tiene el sprite requerido.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró SpriteRenderer en el objeto en manos o en el prefab requerido.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto en el HandPoint.");
            return false;
        }

        return false;
    }


    IEnumerator MoverCañon(GameObject cañon, Vector3 direccion, Vector3 posInicial, Transform puntoDisparo, Vector3 direccionDisparo)
    {
        Debug.Log("Preparando Cañón: " + cañon.name);
        
        haDisparado = true;  // Evitar múltiples disparos

        // Movimiento hacia atrás (lento)
        float tiempo = 0f;
        Vector3 objetivo = cañon.transform.position + direccion;

        while (tiempo < TiempoCarga)
        {
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, objetivo, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelLen;
            yield return null;
        }

        Debug.Log("Pausa...");
        // Pausa en la posición de disparo
        yield return new WaitForSeconds(0.5f);

        // Instanciar el proyectil en la punta del cañón
        DispararProyectil(puntoDisparo, direccionDisparo);

        // Movimiento hacia adelante (rápido)
        tiempo = 0f;
        while (tiempo < TiempoCarga)
        {
            cañon.transform.position = Vector3.Lerp(cañon.transform.position, posInicial, tiempo / TiempoCarga);
            tiempo += Time.deltaTime * VelRap;
            yield return null;
        }

        // Permitir que el cañón vuelva a disparar
        haDisparado = false;
    }

    void DispararProyectil(Transform puntoDisparo, Vector3 direccionDisparo)
    {
        Debug.Log("Disparando proyectil...");
        GameObject proyectilInstanciado = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = proyectilInstanciado.GetComponent<Rigidbody>();
        

        if (rb != null)
        {
            rb.AddForce(-direccionDisparo * fuerzaDisparo, ForceMode.Impulse);
        }
    }
}
