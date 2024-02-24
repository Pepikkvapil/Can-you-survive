using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{
    public GameObject healthUpgradePrefab;
    public GameObject damageUpgradePrefab;
    public float spawnInterval = 10f; // Time before a new upgrade spawns after the previous one is picked up
    private float timer;
    GameObject spawnedUpgrade;

    void Update()
    {
        if (spawnedUpgrade == null)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnRandomUpgrade();
                timer = 0;
            }
        }

    }

    void SpawnRandomUpgrade()
    {
        GameObject prefabToSpawn = Random.value > 0.5f ? healthUpgradePrefab : damageUpgradePrefab;
        spawnedUpgrade = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, transform);
    }

}



