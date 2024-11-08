using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMinionSpawner : MonoBehaviour
{
    public MinionSpawner minionSpawner; // Referencia al MinionSpawner
    public float spawnInterval = 2f;    // Intervalo entre spawns
    public int spawnCount = 5;          // Número de minions a generar automáticamente

    private int currentSpawnCount = 0;

    void Start()
    {
        // Llamar al spawn automáticamente cuando la escena comienza
        StartCoroutine(SpawnMinionsAutomatically());
    }

    private IEnumerator SpawnMinionsAutomatically()
    {
        // Generamos los minions durante el tiempo especificado
        while (currentSpawnCount < spawnCount)
        {
            minionSpawner.SpawnMinion(MinionSpawner.SpawnZone.Player1Zone); // Puedes cambiar la zona aquí
            currentSpawnCount++;

            yield return new WaitForSeconds(spawnInterval); // Esperar entre cada spawn
        }
    }
}