using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Torreta2 : MonoBehaviour
{
    public Collider zonaRecarga;
    public float tiempoEntreDisparos = 0.5f;
    public GameObject proyectil;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 20f;
    public GameObject prefabRequerido;

    public int municionActual = 0;
    public int maximoMunicion = 50;
    public int limiteRecarga = 30;
    public int municionPorPrefab = 10;


    public Image barraMunicion;
    public float anchoMaximoBarra = 200f;

    private bool enZonaRecarga = false;
    public bool disparando = false;

    public Animator torretaAnimator;

    private void Start()
    {
        if (torretaAnimator == null)
        {
            torretaAnimator = GetComponentInChildren<Animator>();
            if (torretaAnimator == null)
            {
                Debug.LogWarning("No se encontró el Animator en el objeto ni en sus hijos.");
            }
            else
            {
                Debug.Log("Animator encontrado y asignado correctamente.");
            }
        }
        else
        {
            Debug.Log("Animator correctamente.");
        }

        Debug.Log("Torreta lista para disparar.");
        ActualizarBarraMunicion();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enZonaRecarga = true;
            Debug.Log("Jugador ha entrado en la zona de recarga.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enZonaRecarga = false;
            Debug.Log("Jugador ha salido de la zona de recarga.");
        }
    }

    private void Update()
    {
        if ((enZonaRecarga && Input.GetKey(KeyCode.X) && VerificarPrefabEnManos()) || (enZonaRecarga && Input.GetKey(KeyCode.I) && VerificarPrefabEnManos()))
        {
            AgregarMunicion();
        }

        if (municionActual > 0 && !disparando)
        {
            Debug.Log("Iniciando disparo automático...");
            StartCoroutine(DispararAutomaticamente());
        }
    }

    private void AgregarMunicion()
    {
        if (municionActual < limiteRecarga)
        {
            municionActual = Mathf.Min(municionActual + municionPorPrefab, maximoMunicion);
            ActualizarBarraMunicion();

            Transform handpoint = GetCurrentPlayerHandPoint();
            if (handpoint != null && handpoint.childCount > 0)
            {
                Destroy(handpoint.GetChild(0).gameObject);
            }
            Debug.Log("Munición agregada. Munición actual: " + municionActual);
        }
        else
        {
            Debug.LogWarning("La munición está por encima del límite de recarga. Munición actual: " + municionActual);
        }
    }

    private void ActualizarBarraMunicion()
    {
        if (barraMunicion != null)
        {
            float anchoActual = (float)municionActual / maximoMunicion * anchoMaximoBarra;
            barraMunicion.rectTransform.sizeDelta = new Vector2(anchoActual, barraMunicion.rectTransform.sizeDelta.y);
        }
    }

    private bool VerificarPrefabEnManos()
    {
        Transform handpoint = GetCurrentPlayerHandPoint();
        if (handpoint != null)
        {
            Debug.Log("HandPoint encontrado con éxito.");
            if (handpoint.childCount > 0)
            {
                GameObject objetoEnManos = handpoint.GetChild(0).gameObject;
                Debug.Log("Objeto en mano encontrado: " + objetoEnManos.name);

                Item itemEnManos = objetoEnManos.GetComponent<Item>();
                Item itemRequerido = prefabRequerido.GetComponent<Item>();

                if (itemEnManos != null && itemRequerido != null)
                {
                    if (itemEnManos.itemType == itemRequerido.itemType)
                    {
                        Debug.Log("El itemType en las manos coincide con el prefab requerido.");
                        return true;
                    }
                    else
                    {
                        Debug.LogWarning("El itemType en las manos no coincide con el prefab requerido.");
                    }
                }
                else
                {
                    Debug.LogWarning("El objeto en manos o el prefab requerido no tienen un componente Item.");
                }
            }
            else
            {
                Debug.LogWarning("El HandPoint no tiene hijos.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró HandPoint.");
        }
        return false;
    }

    private Transform GetCurrentPlayerHandPoint()
    {
        Player player1 = FindObjectOfType<Player>();
        Player2 player2 = FindObjectOfType<Player2>();

        if (player1 != null && player1.GetComponent<Collider>().bounds.Intersects(zonaRecarga.bounds))
        {
            Debug.Log("Player1 está en la zona de recarga.");
            return ObtenerHandPoint(player1.transform);
        }
        else if (player2 != null && player2.GetComponent<Collider>().bounds.Intersects(zonaRecarga.bounds))
        {
            Debug.Log("Player2 está en la zona de recarga.");
            return ObtenerHandPoint(player2.transform);
        }

        Debug.Log("No se encontró un jugador en la zona de recarga.");
        return null;
    }

    private Transform ObtenerHandPoint(Transform playerTransform)
    {
        Transform hand = playerTransform.Find("Hand");
        if (hand != null)
        {
            Transform handpoint = hand.Find("HandPoint");
            if (handpoint != null)
            {
                Debug.Log("HandPoint encontrado.");
                return handpoint;
            }
            else
            {
                Debug.LogWarning("HandPoint no encontrado en Hand.");
            }
        }
        else
        {
            Debug.LogWarning("Hand no encontrado en el jugador.");
        }
        return null;
    }

    IEnumerator DispararAutomaticamente()
    {
        disparando = true;

        if (torretaAnimator != null)
        {
            torretaAnimator.SetBool("isFiring", true);
            Debug.Log("Animator isFiring activado.");
        }

        while (municionActual > 0)
        {
            Debug.Log("Disparando proyectil automáticamente...");
            municionActual--;
            ActualizarBarraMunicion();

            GameObject proyectilInstanciado = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);

            // Desactivar el SpriteRenderer del proyectil
            SpriteRenderer spriteRenderer = proyectilInstanciado.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            Rigidbody rb = proyectilInstanciado.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.forward * fuerzaDisparo, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(tiempoEntreDisparos);
        }

        disparando = false;

        if (torretaAnimator != null)
        {
            torretaAnimator.SetBool("isFiring", false);
            Debug.Log("Animator isFiring desactivado.");
        }
    }
}
