using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlyingMinionAI : MonoBehaviour
{
    protected Rigidbody rb;
    public Transform player1;            // Jugador 1
    public Transform player2;            // Jugador 2
    public Transform TargetPlayer;       // Jugador objetivo
    public float moveSpeed = 5f;         // Velocidad del minion
    public float minDistance = 2f;       // Distancia mínima para interactuar
    public float maxDistance = 10f;      // Distancia máxima para empezar a acercarse al jugador
    protected bool isInteracting = false; // Estado de interacción

    protected float interactionDuration = 10f;  // Duración de la interacción (ejemplo)

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on " + gameObject.name);
        }

        // Desactivamos la gravedad del Rigidbody para que el minion pueda flotar
        rb.useGravity = false;
        rb.isKinematic = false; // Asegúrate de que el Rigidbody no sea kinemático
    }

    public virtual void Initialize(Transform p1, Transform p2)
    {
        player1 = p1;
        player2 = p2;

        // Decide cuál jugador seguir
        TargetPlayer = Vector3.Distance(transform.position, player1.position) < Vector3.Distance(transform.position, player2.position) ? player1 : player2;

        Debug.Log("Minion initialized, following: " + TargetPlayer.name); // Depuración
    }

    // Mueve el minion hacia el jugador
    protected void MoveTowardsPlayer()
    {
        if (TargetPlayer != null)
        {
            // Calcula la distancia actual al jugador
            float currentDistance = Vector3.Distance(transform.position, TargetPlayer.position);

            // Verifica si la distancia está dentro del rango
            if (currentDistance > minDistance && currentDistance <= maxDistance)
            {
                // Calcula la dirección hacia el jugador en 3D
                Vector3 direction = (TargetPlayer.position - transform.position).normalized;

                // Si el minion está por encima del jugador, baja gradualmente
                if (transform.position.y > TargetPlayer.position.y)
                {
                    direction.y -= 0.5f; // Ajusta este valor según qué tan rápido quieres que baje
                }

                // Calcula la nueva posición hacia el jugador
                Vector3 newPosition = Vector3.MoveTowards(transform.position, TargetPlayer.position, moveSpeed * Time.deltaTime);
                rb.MovePosition(newPosition);

                Debug.Log("Minion moving towards: " + TargetPlayer.name); // Depuración
            }
            else if (currentDistance > maxDistance)
            {
                // Si está demasiado lejos, comienza a acercarse al jugador
                Vector3 direction = (TargetPlayer.position - transform.position).normalized;

                // Mueve al minion hacia el jugador en todas las direcciones
                Vector3 newPosition = Vector3.MoveTowards(transform.position, TargetPlayer.position, moveSpeed * Time.deltaTime);
                rb.MovePosition(newPosition);

                Debug.Log("Minion moving closer from far distance to: " + TargetPlayer.name); // Depuración
            }
            else
            {
                // Detén al minion cuando está dentro de la distancia mínima
                Debug.Log("Minion within minimum distance, stopping."); // Depuración
            }
        }
        else
        {
            Debug.LogWarning("No target player assigned for " + gameObject.name); // Depuración en caso de que targetPlayer sea null
        }
    }

    // Métodos abstractos que deben implementarse en las clases derivadas
    protected abstract void StartInteraction();
    protected abstract void EndInteraction();

    // Actualiza el comportamiento del minion (debe ser implementado en las clases derivadas)
    protected abstract void UpdateAI();

    // Llamamos a UpdateAI desde el método Update (este es el ciclo de vida del minion)
    protected virtual void Update()
    {
        UpdateAI();  // Llamamos a la función implementada en las clases derivadas.
    }
}
