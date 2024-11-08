using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public Transform player1;
    public Transform player2;

    public enum SpawnZone { Player1Zone, Player2Zone }

    public void SpawnMinion(SpawnZone spawnZone)
    {
        GameObject newMinion = Instantiate(minionPrefab, transform.position, Quaternion.identity);
        FlyingMinionAI minionAI = newMinion.GetComponent<FlyingMinionAI>();
        
        if (minionAI != null)
        {
            minionAI.player1 = player1;
            minionAI.player2 = player2;

            if (spawnZone == SpawnZone.Player1Zone)
            {
                minionAI.TargetPlayer = player1;
            }
            else
            {
                minionAI.TargetPlayer = player2;
            }

            Debug.Log("Minion spawned in " + spawnZone + " targeting " + minionAI.TargetPlayer.name);
        }
    }
}
