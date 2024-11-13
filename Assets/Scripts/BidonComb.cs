using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BidonComb : MonoBehaviour
{
    public float maxCapacity = 1f; // Capacidad máxima del bidón
    public float fillAmount = 0f; // Combustible actual en el bidón
    private bool isHeldByPlayer; // Estado de si el bidón está siendo sostenido

    public Canvas targetCanvas; // Canvas en el que se generará la barra de progreso
    private RectTransform fuelBarRect; // RectTransform de la barra para ajustar su tamaño
    private Image fuelBarImage; // Imagen de la barra de combustible

    public Sprite fullSprite; // Sprite del bidón lleno
    public Sprite normalSprite;
    private SpriteRenderer spriteRenderer; // SpriteRenderer del bidón

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer del bidón
        CreateFuelBar(); // Crear la barra de combustible al inicio
    }

    void Update()
    {
        UpdateFuelBar(); // Actualizar la barra de combustible

        if (IsFull()) 
        {
            fuelBarImage.enabled = false; // Ocultar la barra cuando el bidón esté lleno
            spriteRenderer.sprite = fullSprite; // Cambiar el sprite al bidón lleno
        }
        if (IsEmpty()) 
        {
            fuelBarImage.enabled = true; // Ocultar la barra cuando el bidón esté lleno
            spriteRenderer.sprite = normalSprite; // Cambiar el sprite al bidón lleno
        }
    }

    // Método para remover combustible
    public float RemoveFuel(float amount)
    {
        if (fillAmount >= amount)
        {
            fillAmount -= amount;
            return amount; // Devuelve la cantidad removida
        }
        else
        {
            float temp = fillAmount; // Guarda la cantidad restante
            fillAmount = 0; // Resetea a 0
            return temp; // Devuelve la cantidad que se pudo remover
        }
    }

    // Método para añadir combustible
    public void AddFuel(float amount)
    {
        if (amount <= 0) return; // No se permite añadir cantidades negativas

        fillAmount += amount; // Añade la cantidad
        if (fillAmount > maxCapacity)
        {
            fillAmount = maxCapacity; // Limita a la capacidad máxima
        }
    }

    // Método para verificar si el bidón está lleno
    public bool IsFull()
    {
        return fillAmount >= maxCapacity;
    }

    // Método para verificar si el bidón está vacío
    public bool IsEmpty()
    {
        return fillAmount <= 0;
    }

    // Método para obtener la cantidad actual de combustible
    public float GetCurrentFuel()
    {
        return fillAmount;
    }

    // Método para establecer si el bidón está siendo sostenido
    public void SetHeldByPlayer(bool held)
    {
        isHeldByPlayer = held;
    }

    // Método para verificar si el bidón está siendo sostenido
    public bool IsHeldByPlayer()
    {
        return isHeldByPlayer;
    }

    // Método para crear la barra de combustible en el canvas especificado
    private void CreateFuelBar()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Canvas de destino no asignado para la barra de combustible.");
            return;
        }

        // Crear una imagen directamente como barra de combustible en el canvas
        GameObject fuelBarObject = new GameObject("FuelBar", typeof(RectTransform));
        fuelBarObject.transform.SetParent(targetCanvas.transform);

        // Configuración de RectTransform para la barra
        fuelBarRect = fuelBarObject.GetComponent<RectTransform>();
        fuelBarRect.sizeDelta = new Vector2(0, 3); // Inicializar con ancho 0, que crecerá al agregar combustible
        fuelBarRect.anchorMin = new Vector2(0.5f, 0f); // Ajustar para que esté debajo del objeto del bidón
        fuelBarRect.anchorMax = new Vector2(0.5f, 0f);
        fuelBarRect.pivot = new Vector2(0.5f, 1f);
        fuelBarRect.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, -35, 0); // Posición bajo el objeto

        // Añadir componente Image y color inicial verde
        fuelBarImage = fuelBarObject.AddComponent<Image>();
        fuelBarImage.color = Color.green;
    }

    // Método para actualizar el tamaño de la barra de combustible
    private void UpdateFuelBar()
    {
        if (fuelBarRect != null && fuelBarImage != null && fuelBarImage.enabled)
        {
            // Ajustar el tamaño de la barra en función del fillAmount
            fuelBarRect.sizeDelta = new Vector2(100 * (fillAmount / maxCapacity), 40); // Ancho máximo de 800
            fuelBarRect.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, -20, 0); // Actualiza la posición
        }
    }
}
