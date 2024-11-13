using System.Collections;
using UnityEngine;

public class VidaJefe : MonoBehaviour
{
    public Canvas vidajEFE; // Referencia al canvas de la barra de vida del jefe
    public RectTransform HP; // RectTransform de la barra de vida dentro del panel
    private float saludMaxima = 100f; // Salud máxima
    public float saludActual; // Salud actual
    private float initialHeight; // Altura inicial de la barra de vida
    private float initialPosY; // Posición inicial en Y de la barra de vida

    public GameObject impactoPrefab; // Prefab del sprite de impacto

    // Variables para personalizar la posición de aparición del impacto
    public float impactoOffsetX = 10f;
    public float impactoOffsetY = -90f;
    public float impactoOffsetZ = 75f;

    void Start()
    {
        // Iniciar la salud actual como la salud máxima
        saludActual = saludMaxima;

        // Asegurarse de obtener la referencia a la barra de vida
        if (vidajEFE != null)
        {
            HP = vidajEFE.transform.Find("Panel/HP")?.GetComponent<RectTransform>();
            if (HP == null)
            {
                Debug.LogError("No se encontró la imagen dentro del panel Fort1.");
            }
            else
            {
                // Guardar la altura inicial de la barra de vida y su posición en Y
                initialHeight = HP.sizeDelta.y;
                initialPosY = HP.anchoredPosition.y;
            }
        }
    }

    void Update()
    {
        // Verificar si la salud es mayor que 0 para mostrar la barra de vida
        vidajEFE.enabled = saludActual > 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil"))
        {
            if (HP != null)
            {
                // Reducir la salud actual en 10
                saludActual -= 5f;

                // Asegurar que la salud no baje de 0
                saludActual = Mathf.Clamp(saludActual, 0f, saludMaxima);

                // Calcular el porcentaje de salud restante
                float healthPercentage = saludActual / saludMaxima;

                // Ajustar la altura de la barra de vida en función de la salud restante y la altura inicial
                float newHeight = initialHeight * healthPercentage;
                HP.sizeDelta = new Vector2(HP.sizeDelta.x, newHeight);

                // Mantener la barra ajustada hacia la parte superior
                HP.anchoredPosition = new Vector2(HP.anchoredPosition.x, initialPosY - (initialHeight - newHeight) / 2f);
            }

            // Obtener la posición de impacto con desplazamiento
            Vector3 impactoPos = other.transform.position;
            StartCoroutine(MostrarImpacto(impactoPos));
        }
    }

    private IEnumerator MostrarImpacto(Vector3 posicion)
    {
        // Ajustar la posición usando los valores de desplazamiento en el sistema global
        Vector3 posicionAjustada = posicion + transform.TransformVector(new Vector3(impactoOffsetX, impactoOffsetY, impactoOffsetZ));

        // Crear una instancia del GameObject en la posición ajustada
        GameObject impacto = Instantiate(impactoPrefab, posicionAjustada, Quaternion.identity);

        // Esperar 0.3 segundos
        yield return new WaitForSeconds(0.3f);

        // Destruir el GameObject de impacto
        Destroy(impacto);
    }
}
