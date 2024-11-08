using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Referencia a la cámara
    public Camera mainCamera;

    void Start()
    {
        // Obtén la cámara principal si no se ha asignado
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // Encuentra todos los objetos con la tag "Item"
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        // Recorre cada objeto con la tag "Item"
        foreach (GameObject item in items)
        {
            // Asegúrate de que el objeto no sea nulo
            if (item != null)
            {
                // Obtener la dirección de la cámara solo en el eje Y
                Vector3 cameraForward = mainCamera.transform.forward;
                // Mantén la dirección en el plano horizontal

                // Obtén la rotación hacia esa dirección
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

                // Limita la rotación del objeto a 45 grados en el eje Y
                Quaternion limitedRotation = Quaternion.RotateTowards(item.transform.rotation, targetRotation, 45f);

                // Aplica la rotación limitada al objeto
                item.transform.rotation = limitedRotation;
            }
        }
    }
}
