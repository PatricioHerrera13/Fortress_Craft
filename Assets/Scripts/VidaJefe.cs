using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaJefe : MonoBehaviour
{
    public Canvas vidajEFE; // Referencia al canvas de Fort1
    public RectTransform HP; // RectTransform de la imagen dentro del panel Fort1
    private float initialPosX; // Guardar la posición inicial en X
    private float saludMaxima = 100f; // Salud máxima
    public float saludActual; // Salud actual

    void Start()
    {
        // Iniciar la salud actual como la salud máxima
        saludActual = saludMaxima;

        // Asegúrate de obtener la referencia a la imagen dentro del panel Fort1
        if (vidajEFE != null)
        {
            HP = vidajEFE.transform.Find("Panel/HP")?.GetComponent<RectTransform>();
            if (HP == null)
            {
                Debug.LogError("No se encontró la imagen dentro del panel Fort1.");
            }
            else
            {
                // Guardar la posición inicial en X
                initialPosX = HP.anchoredPosition.x;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil"))
        {
            Debug.Log("Nashe en 1");

            if (HP != null)
            {
                // Reducir la salud actual en 20
                saludActual -= 5f;

                // Asegurar que la salud no baje de 0
                saludActual = Mathf.Clamp(saludActual, 0f, saludMaxima);

                // Calcular el nuevo ancho basado en el porcentaje de salud restante
                float healthPercentage = saludActual / saludMaxima;
                float newWidth = HP.sizeDelta.x * healthPercentage;

                // Actualizar el ancho de la barra de vida
                HP.sizeDelta = new Vector2(newWidth, HP.sizeDelta.y);

                // Mantener la posición inicial en X
                HP.anchoredPosition = new Vector2(initialPosX, HP.anchoredPosition.y);

                
            }
        }
    }
}

