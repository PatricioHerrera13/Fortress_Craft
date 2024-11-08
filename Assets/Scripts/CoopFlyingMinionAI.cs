using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopFlyingMinionAI : FlyingMinionAI
{
    private float interactionTimer = 0f;

    protected override void UpdateAI()
    {
        if (!isInteracting)
        {
            MoveTowardsPlayer();

            if (Vector3.Distance(transform.position, TargetPlayer.position) <= minDistance)
            {
                StartInteraction();
                interactionTimer = interactionDuration;
            }
        }
        else
        {
            interactionTimer -= Time.deltaTime;
            if (interactionTimer <= 0)
            {
                EndInteraction();
            }
        }
    }

    protected override void StartInteraction()
    {
        isInteracting = true;
        StartCoroutine(DragPlayer());
    }

    protected override void EndInteraction()
    {
        isInteracting = false;
    }

    private IEnumerator DragPlayer()
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = TargetPlayer.position;
        Vector3 randomDirection = Random.insideUnitSphere;  // Dirección aleatoria de arrastre
        
        while (elapsedTime < interactionDuration)
        {
            elapsedTime += Time.deltaTime;
            TargetPlayer.position += randomDirection * Time.deltaTime;
            yield return null;
        }
        
        EndInteraction();
    }
}
