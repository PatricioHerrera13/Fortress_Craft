using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortaleza : MonoBehaviour
{
    public Canvas VidasFort1; // Referencia al canvas de Fort1
    public RectTransform fort1Image; // RectTransform de la imagen dentro del panel Fort1
    private float initialPosX; // Guardar la posición inicial en X
    private float saludMaxima = 100f; // Salud máxima
    public float saludActual; // Salud actual
    public CameraShake cameraShake;

    void Start()
    {
        // Iniciar la salud actual como la salud máxima
        saludActual = saludMaxima;

        // Asegúrate de obtener la referencia a la imagen dentro del panel Fort1
        if (VidasFort1 != null)
        {
            fort1Image = VidasFort1.transform.Find("Fort1/Image")?.GetComponent<RectTransform>();
            if (fort1Image == null)
            {
                Debug.LogError("No se encontró la imagen dentro del panel Fort1.");
            }
            else
            {
                // Guardar la posición inicial en X
                initialPosX = fort1Image.anchoredPosition.x;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil"))
        {
            Debug.Log("Nashe en 1");

            if (fort1Image != null)
            {
                // Reducir la salud actual en 20
                saludActual -= 20f;
                Destroy(other.gameObject);
                cameraShake.TriggerShake();

                // Asegurar que la salud no baje de 0
                saludActual = Mathf.Clamp(saludActual, 0f, saludMaxima);

                // Calcular el nuevo ancho basado en el porcentaje de salud restante
                float healthPercentage = saludActual / saludMaxima;
                float newWidth = fort1Image.sizeDelta.x * healthPercentage;

                // Actualizar el ancho de la barra de vida
                fort1Image.sizeDelta = new Vector2(newWidth, fort1Image.sizeDelta.y);

                // Mantener la posición inicial en X
                fort1Image.anchoredPosition = new Vector2(initialPosX, fort1Image.anchoredPosition.y);

                // Si la salud es 0 o menos, eliminar la imagen y mostrar mensaje
                if (saludActual <= 0)
                {
                    fort1Image.gameObject.SetActive(false); // Desactivar la imagen
                    Debug.Log("Perdiste");
                }
            }
        }
    }
}
