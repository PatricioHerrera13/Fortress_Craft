using UnityEngine;
using UnityEngine.UI;

public class CraftingProgressBar : MonoBehaviour
{
    public Image progressBar; // Asigna la imagen de la barra de progreso
    private float craftingTime; // Tiempo total de crafting
    private float currentTime = 0f;
    private bool isCrafting = false;

    public delegate void CraftingComplete();
    public event CraftingComplete OnCraftingComplete; // Declaración del evento

    void Update()
    {
        if (isCrafting)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / craftingTime;
            progressBar.fillAmount = progress;

            if (progress >= 1f)
            {
                CompleteCrafting();
            }
        }
    }

    public void StartCrafting(float time)
    {
        craftingTime = time; // Asigna el tiempo de la receta
        isCrafting = true;
        currentTime = 0f;
        progressBar.fillAmount = 0f; // Reiniciar la barra
    }

    private void CompleteCrafting()
    {
        isCrafting = false;
        progressBar.fillAmount = 1f; // Asegurar que la barra esté llena

        // Llama al evento cuando la elaboración se complete
        OnCraftingComplete?.Invoke();
    }
}