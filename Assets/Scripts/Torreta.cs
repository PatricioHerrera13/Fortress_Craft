using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Torreta : MonoBehaviour
{
    public Collider zonaRecarga;
    public float tiempoEntreDisparos = 0.5f;
    public GameObject proyectil;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 20f;
    public GameObject prefabRequerido;

    public int municionActual = 0;
    public int maximoMunicion = 50;  // Límite absoluto de munición
    public int limiteRecarga = 30;   // Límite mínimo para permitir recarga
    public int municionPorPrefab = 10;

    public Image barraMunicion;
    public float anchoMaximoBarra = 200f;

    private bool enZonaRecarga = false;
    private bool disparando = false;

    private void Start()
    {
        Debug.Log("Torreta lista para disparar.");
        ActualizarBarraMunicion();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enZonaRecarga = true;
            Debug.Log("Jugador ha entrado en la zona de recarga.");
        }
    }

    void OnTriggerExit(Collider other)
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
            StartCoroutine(DispararAutomaticamente());
        }
    }

    private void AgregarMunicion()
    {
        if (municionActual < limiteRecarga)
        {
            municionActual = Mathf.Min(municionActual + municionPorPrefab, maximoMunicion);
            ActualizarBarraMunicion();
            // Aquí se asume que el prefab se elimina del handPoint del jugador que interactúa
            Transform handPoint = GetCurrentPlayerHandPoint();
            if (handPoint != null && handPoint.childCount > 0)
            {
                Destroy(handPoint.GetChild(0).gameObject);
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
        Transform handPoint = GetCurrentPlayerHandPoint();
        if (handPoint != null && handPoint.childCount > 0)
        {
            GameObject objetoEnManos = handPoint.GetChild(0).gameObject;

            PrefabSprite spriteEnManos = objetoEnManos.GetComponent<PrefabSprite>();
            PrefabSprite spriteRequerido = prefabRequerido.GetComponent<PrefabSprite>();

            if (spriteEnManos != null && spriteRequerido != null)
            {
                if (spriteEnManos.sprite == spriteRequerido.sprite)
                {
                    Debug.Log("El sprite en las manos coincide con el prefab requerido.");
                    return true;
                }
            }
        }
        return false;
    }

    private Transform GetCurrentPlayerHandPoint()
    {
        Player player1 = FindObjectOfType<Player>();
        Player2 player2 = FindObjectOfType<Player2>();

        // Devuelve el handPoint del jugador que está en la zona de recarga
        if (player1 != null && player1.GetComponent<Collider>().bounds.Intersects(zonaRecarga.bounds))
        {
            return player1.handpoint; // Asegúrate de que el nombre sea correcto (handpoint en lugar de handPoint)
        }
        else if (player2 != null && player2.GetComponent<Collider>().bounds.Intersects(zonaRecarga.bounds))
        {
            return player2.handpoint; // Asegúrate de que el nombre sea correcto (handpoint en lugar de handPoint)
        }

        return null; // No se encontró un jugador en la zona
    }

    IEnumerator DispararAutomaticamente()
    {
        disparando = true;

        while (municionActual > 0)
        {
            Debug.Log("Disparando proyectil automáticamente...");
            municionActual--;
            ActualizarBarraMunicion();

            GameObject proyectilInstanciado = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);
            SpriteRenderer renderer = proyectilInstanciado.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }

            Rigidbody rb = proyectilInstanciado.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.forward * fuerzaDisparo, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(tiempoEntreDisparos);
        }

        disparando = false;
    }
}
