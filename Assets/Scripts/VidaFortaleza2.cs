using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortaleza2 : MonoBehaviour
{
    public Canvas VidasFort2; // Referencia al canvas de Fort2
    public RectTransform fort2Image; // RectTransform de la imagen dentro del panel Fort2
    private float initialPosX; // Guardar la posición inicial en X
    private float saludMaxima = 100f; // Salud máxima
    public float saludActual; // Salud actual
    public CameraShake cameraShake;

    void Start()
    {
        // Iniciar la salud actual como la salud máxima
        saludActual = saludMaxima;

        // Asegúrate de obtener la referencia a la imagen dentro del panel Fort2
        if (VidasFort2 != null)
        {
            fort2Image = VidasFort2.transform.Find("Fort2/Image")?.GetComponent<RectTransform>();
            if (fort2Image == null)
            {
                Debug.LogError("No se encontró la imagen dentro del panel Fort2.");
            }
            else
            {
                // Guardar la posición inicial en X
                initialPosX = fort2Image.anchoredPosition.x;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil"))
        {
            Debug.Log("Nashe en 2");

            if (fort2Image != null)
            {
                // Reducir la salud actual en 20
                saludActual -= 20f;
                Destroy(other.gameObject);
                cameraShake.TriggerShake();

                // Asegurar que la salud no baje de 0
                saludActual = Mathf.Clamp(saludActual, 0f, saludMaxima);

                // Calcular el nuevo ancho basado en el porcentaje de salud restante
                float healthPercentage = saludActual / saludMaxima;
                float newWidth = fort2Image.sizeDelta.x * healthPercentage;

                // Actualizar el ancho de la barra de vida
                fort2Image.sizeDelta = new Vector2(newWidth, fort2Image.sizeDelta.y);

                // Mantener la posición inicial en X
                fort2Image.anchoredPosition = new Vector2(initialPosX, fort2Image.anchoredPosition.y);

                // Si la salud es 0 o menos, eliminar la imagen y mostrar mensaje
                if (saludActual <= 0)
                {
                    fort2Image.gameObject.SetActive(false); // Desactivar la imagen
                    Debug.Log("Perdiste");
                }
            }
        }
    }
}
