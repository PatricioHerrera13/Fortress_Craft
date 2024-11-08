using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour
{
    public GameObject firePrefab; // El prefab del fuego
    public float minTime = 2f; // Tiempo mínimo de expansión
    public float maxTime = 5f; // Tiempo máximo de expansión
    public float spreadDistance = 1.5f; // Distancia a la que se generarán las copias
    public static int currentFireCount = 0; // Contador estático para instancias de fuego
    public static int maxFireCount = 30; // Número máximo de instancias permitidas

    private float nextSpreadTime;
    private float creationTime; // Hora de creación para cada fuego
    private bool hasSpread = false; // Estado para verificar si ya se ha expandido
    private Vector3 lastDirection = Vector3.zero; // Dirección de la última expansión
    private bool isRestarting = false; // Evitar múltiples reinicios simultáneos
    private bool isDestroyed = false; // Estado para verificar si el fuego ha sido destruido

    private MeshCollider meshCollider; // Referencia al MeshCollider

    public void Start()
    {
        // Hora de creación del fuego
        creationTime = Time.time;

        // Obtén el MeshCollider del objeto padre FireSpawner
        meshCollider = FindObjectOfType<FireSpawner>().GetComponent<MeshCollider>();

        // Cada fuego tendrá un tiempo de expansión individual entre minTime y maxTime
        nextSpreadTime = creationTime + Random.Range(minTime, maxTime);
    }

    void Update()
    {
        // Comprobar si es el momento de expandirse y no se ha expandido aún
        if (!hasSpread && Time.time >= nextSpreadTime)
        {
            SpreadFire();
            hasSpread = true; // Marcar como expandido
        }

        // Si no quedan fuegos en la escena y no estamos reiniciando ya, empezar la corrutina para reiniciar
        if (!isRestarting && GameObject.FindGameObjectsWithTag("Fuego").Length == 0)
        {
            StartCoroutine(RestartFireAfterDelay(20f)); // Espera 20 segundos antes de reiniciar
        }
    }

    public void SpreadFire()
    {
        // Si el fuego ha sido destruido, no continuar con la expansión
        if (isDestroyed) return;

        // Solo crear una nueva instancia si no hemos alcanzado el máximo permitido
        if (currentFireCount < maxFireCount)
        {
            // Verificar que firePrefab no sea nulo
            if (firePrefab == null)
            {
                Debug.LogError("firePrefab es nulo. Asegúrate de asignar un prefab válido en el inspector.");
                
                return; // Salir si no hay un prefab válido
            }

            // Elegir una dirección aleatoria para expandirse, excepto la contraria a la última
            List<Vector3> possibleDirections = new List<Vector3>
            {
                new Vector3(spreadDistance, 0, 0),   // Derecha
                new Vector3(-spreadDistance, 0, 0),  // Izquierda
                new Vector3(0, 0, spreadDistance),   // Delante
                new Vector3(0, 0, -spreadDistance)   // Detrás
            };

            // Remover la dirección contraria
            if (lastDirection != Vector3.zero)
            {
                Vector3 oppositeDirection = -lastDirection;
                possibleDirections.Remove(oppositeDirection);
            }

            // Intentar encontrar una posición válida
            for (int i = 0; i < 40; i++) // Intentar 40 veces
            {
                // Elegir una nueva dirección aleatoria de las posibles
                Vector3 chosenDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];

                // Verificar la nueva posición
                Vector3 newPosition = new Vector3(transform.position.x + chosenDirection.x, transform.position.y, transform.position.z + chosenDirection.z);

                // Comprobar si la nueva posición está dentro del Mesh Collider
                if (IsPositionInsideMeshCollider(newPosition))
                {
                    // Verificar si la nueva posición ya tiene un fuego
                    Collider[] hitColliders = Physics.OverlapSphere(newPosition, 0.1f); // Radio pequeño para detectar colisiones
                    bool positionOccupied = false;

                    foreach (Collider hitCollider in hitColliders)
                    {
                        if (hitCollider.gameObject.CompareTag("Fuego")) // Asegúrate de asignar la etiqueta "Fuego" a todos los fuegos
                        {
                            positionOccupied = true;
                            break;
                        }
                    }

                    // Solo instanciar el nuevo fuego si la posición no está ocupada
                    if (!positionOccupied)
                    {
                        GameObject newFire = Instantiate(firePrefab, newPosition, Quaternion.identity);

                        // Asegúrate de que el nuevo fuego tenga asignado el mismo prefab
                        Fuego newFireScript = newFire.GetComponent<Fuego>();
                        if (newFireScript != null)
                        {
                            newFireScript.firePrefab = firePrefab; // Asignar el prefab
                            currentFireCount++; // Incrementar el contador de instancias
                            newFireScript.lastDirection = chosenDirection; // Registrar la dirección de expansión
                        }
                        else
                        {
                            Debug.LogError("El prefab de fuego no tiene el script Fuego asignado.");
                        }
                        return; // Salir si se generó exitosamente
                    }
                    else
                    {
                        Debug.Log("La posición ya está ocupada, intentando otra vez.");
                    }
                }
                else
                {
                    Debug.Log("La nueva posición está fuera del Mesh Collider, intentando otra vez.");
                }
            }

            Debug.Log("No se pudo generar un nuevo fuego después de múltiples intentos.");
        }
        else
        {
            Debug.Log("Se ha alcanzado el número máximo de instancias de fuego.");
        }
    }

    private bool IsPositionInsideMeshCollider(Vector3 position)
    {
        // Comprobar si la posición está dentro del MeshCollider
        return meshCollider.bounds.Contains(position);
    }

    private IEnumerator RestartFireAfterDelay(float delay)
    {
        isRestarting = true; // Evitar que se llame a la corrutina varias veces
        Debug.Log("Esperando " + delay + " segundos antes de reiniciar...");
        yield return new WaitForSeconds(delay); // Esperar el tiempo especificado

        // Verificar nuevamente si no hay fuegos antes de reiniciar
        if (GameObject.FindGameObjectsWithTag("Fuego").Length == 0)
        {
            RestartFire();
        }
        isRestarting = false;
    }

    private void RestartFire()
    {
        Debug.Log("No quedan más fuegos en la escena. Reiniciando el proceso...");
        hasSpread = false; // Resetear el estado de expansión
        currentFireCount = 0; // Reiniciar el contador de fuegos

        // Reiniciar el tiempo de expansión para el próximo fuego
        nextSpreadTime = Time.time + Random.Range(minTime, maxTime);
    }

    // Método para marcar el fuego como destruido
    public void DestroyFire()
    {
        isDestroyed = true; // Marcar el fuego como destruido
        currentFireCount--; // Decrementar el contador de instancias
        Destroy(gameObject); // Destruir el objeto de fuego
    }
}
