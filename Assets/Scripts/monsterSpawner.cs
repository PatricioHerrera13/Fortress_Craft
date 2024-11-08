using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveContent
    {
        [SerializeField][NonReorderable] GameObject[] monsterSpawner;

        public GameObject[] GetMonsterSpawnList()
        {
            return monsterSpawner;
        }
    }

    [SerializeField][NonReorderable] WaveContent[] waves;
    [SerializeField] Transform[] patrolPoints; // Añadido para almacenar los puntos de patrullaje
    int currentWave = 0;
    float SpawRange = 10;

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        
    }

    void SpawnWave()
    {
        GameObject[] monsters = waves[currentWave].GetMonsterSpawnList();

        for (int i = 0; i < monsters.Length; i++)
        {
            GameObject monster = Instantiate(monsters[i], FindSpawnLoc(), Quaternion.identity);
            Minion minion = monster.GetComponent<Minion>(); // Obtiene el script Minion del enemigo instanciado
            if (minion != null)
            {
                minion.patrolPoints = patrolPoints; // Asigna los puntos de patrullaje al enemigo
            }
        }
    }

    Vector3 FindSpawnLoc()
    {
        Vector3 SpawnPos;

        float xLoc = Random.Range(-SpawRange, SpawRange) + transform.position.x;
        float zLoc = Random.Range(-SpawRange, SpawRange) + transform.position.z;
        float yLoc = transform.position.y;

        SpawnPos = new Vector3(xLoc, yLoc, zLoc);

        if (Physics.Raycast(SpawnPos, Vector3.down, 5))
        {
            return SpawnPos;
        }
        else
        {
            return FindSpawnLoc();
        }
    }
}
