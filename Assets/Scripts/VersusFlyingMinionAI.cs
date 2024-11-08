using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusFlyingMinionAI : FlyingMinionAI
{
    private bool isDestroyed = false;

    protected override void UpdateAI()
    {
        if (!isInteracting)
        {
            MoveTowardsPlayer();

            if (Vector3.Distance(transform.position, TargetPlayer.position) <= minDistance)
            {
                StartInteraction();
            }
        }
        else
        {
            EndInteraction();
            DestroyMinion();
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
        Vector3 randomDirection = Random.insideUnitSphere;

        while (elapsedTime < interactionDuration)
        {
            elapsedTime += Time.deltaTime;
            TargetPlayer.position += randomDirection * Time.deltaTime;
            yield return null;
        }

        EndInteraction();
    }

    private void DestroyMinion()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
