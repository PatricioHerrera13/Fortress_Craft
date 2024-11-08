using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    public GameObject firePrefab; // Prefab del fuego
    public float spreadDistance = 1.5f; // Distancia a la que se generarán las copias
    public Collider fireSpawnArea; // Collider que define el área donde se puede generar fuego
    private static int currentFireCount = 0; // Contador estático para instancias de fuego
    public static int maxFireCount = 30; // Número máximo de instancias permitidas

    void Start()
    {
        GenerateFire();
    }

    void GenerateFire()
    {
        // Elegir una dirección aleatoria para expandirse
        Vector3 chosenDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * spreadDistance;
        Vector3 newPosition = transform.position + chosenDirection;

        // Verificar si la nueva posición está dentro del área del MeshCollider
        if (fireSpawnArea.bounds.Contains(newPosition))
        {
            // Verificar si la posición está ocupada
            Collider[] hitColliders = Physics.OverlapSphere(newPosition, 0.1f);
            bool positionOccupied = false;

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Fuego"))
                {
                    positionOccupied = true;
                    break;
                }
            }

            // Solo instanciar el nuevo fuego si la posición no está ocupada
            if (!positionOccupied)
            {
                Instantiate(firePrefab, newPosition, Quaternion.identity);
                currentFireCount++;
            }
            else
            {
                Debug.Log("La posición ya está ocupada, seleccionando un fuego existente para expandirse.");

                // Elegir un fuego existente al azar para expandirse
                Fuego[] existingFires = FindObjectsOfType<Fuego>(); // Obtener todos los fuegos existentes
                if (existingFires.Length > 0)
                {
                    // Elegir un fuego existente al azar
                    Fuego randomFire = existingFires[Random.Range(0, existingFires.Length)];
                    randomFire.SpreadFire(); // Invocar el método para expandir desde el fuego existente
                }
                else
                {
                    Debug.Log("No hay fuegos existentes para expandirse.");
                }
            }
        }
        else
        {
            Debug.Log("La nueva posición está fuera del área permitida.");
        }
    }
}
