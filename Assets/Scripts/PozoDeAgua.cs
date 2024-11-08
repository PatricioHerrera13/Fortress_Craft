using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PozoDeAgua : MonoBehaviour
{
    private CuboDeAgua cubo = null; // Definir la variable cubo fuera de los métodos para que sea accesible
    private CuboDeAgua2 cubo2 = null;

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al trigger es el cubo de agua
        if (other.GetComponent<CuboDeAgua>() != null && cubo == null)
        {
            cubo = other.GetComponent<CuboDeAgua>();
            Debug.Log("1 dentro");
        }
        // Verifica si el objeto que entra al trigger es el cubo de agua 2
        else if (other.GetComponent<CuboDeAgua2>() != null && cubo2 == null)
        {
            cubo2 = other.GetComponent<CuboDeAgua2>();
            Debug.Log("2 dentro");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el objeto sale del trigger, y es el cubo de agua, lo eliminamos
        if (other.GetComponent<CuboDeAgua>() != null)
        {
            cubo = null; // Para evitar seguir intentando recargar un cubo que ya no está en el trigger
            Debug.Log("1 fuera");
        }
        else if(other.GetComponent<CuboDeAgua2>() != null)
        {
            cubo2 = null;
            Debug.Log("2 fuera");
        }
    }

    void Update()
    {
        if ((cubo != null && Input.GetKeyDown(KeyCode.X)) || (cubo2 != null && Input.GetKeyDown(KeyCode.I)))
        {
            Debug.Log("recargo");
            if (cubo != null)
            {
                cubo.RecargarCubo();
            }
            else if (cubo2 != null)
            {
                cubo2.RecargarCubo();
            }
        }
    }
}
