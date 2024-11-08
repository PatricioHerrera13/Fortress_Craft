using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboDeAgua : MonoBehaviour
{
    public int maxUsos = 2; // Cantidad máxima de fuegos que puede apagar
    public int usosRestantes;
    private bool puedeApagar = true; // Controla si el cubo puede seguir apagando fuegos
    private Collider fuegoEnContacto; // Guarda referencia al fuego con el que el cubo está en contacto

    void Start()
    {
        // Inicializamos los usos restantes al máximo
        usosRestantes = maxUsos;
    }

    void OnTriggerEnter(Collider other)
    {
        // Detectamos si el cubo entra en contacto con un fuego
        if (other.CompareTag("Fuego") && puedeApagar)
        {
            // Guardamos el fuego con el que estamos en contacto
            fuegoEnContacto = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el cubo sale del área de colisión del fuego, eliminamos la referencia
        if (other.CompareTag("Fuego"))
        {
            fuegoEnContacto = null;
        }
    }

    void Update()
    {
        // Comprobar si el jugador presiona "T" y hay un fuego en contacto
        if ((Input.GetKeyDown(KeyCode.X) && fuegoEnContacto != null && puedeApagar) || (Input.GetKeyDown(KeyCode.I) && fuegoEnContacto != null && puedeApagar))
        {
            // Obtén el script de fuego y llama al método DestroyFire
            Fuego fuego = fuegoEnContacto.GetComponent<Fuego>();
            if (fuego != null)
            {
                fuego.DestroyFire(); // Llama al método para destruir el fuego
                fuegoEnContacto = null; // Reseteamos la referencia después de destruir el fuego

                // Reduce los usos restantes
                usosRestantes--;

                // Si ya no tiene más usos, desactivar la capacidad de apagar fuegos
                if (usosRestantes <= 0)
                {
                    puedeApagar = false;
                    Debug.Log("El cubo ya no tiene más usos. Debes recargarlo.");
                }
            }
        }
    }

    // Método que recarga el cubo cuando interactúa con el pozo
    public void RecargarCubo()
    {
        Debug.Log("cubo recargo");
        usosRestantes = maxUsos;
        puedeApagar = true;
        Debug.Log("El cubo ha sido recargado.");
    }
}
