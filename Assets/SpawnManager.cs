using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;


    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxEnemiesPerWave = 3; // Set the maximum number of enemies per wave.
    [SerializeField] private int totalWaves = 5; // Set the total number of waves.

    private int currentWave = 0; // Tracks the current wave.
    private int enemiesRemaining; // Tracks the number of enemies remaining in the current wave.
    private bool isWaveActive = false; // Tracks whether a wave is currently active.

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (isWaveActive)
        {
            // Check if all enemies from the current wave are defeated.
            if (enemiesRemaining <= 0)
            {
                isWaveActive = false;
                StartNextWave();
            }
        }

        Debug.Log("Enemies Remaining: " + enemiesRemaining); // Debug log for remaining enemies
    }


    public void EnemyRemain()
    {
        enemiesRemaining--;
    }

    private void StartWave()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave);

        // Initialize enemiesRemaining with maxEnemiesPerWave for the current wave.
        enemiesRemaining = maxEnemiesPerWave;

        for (int i = 0; i < enemiesRemaining; i++)
        {
            Transform selectedSpawnPoint = GetClosestSpawnPoint();

            if (selectedSpawnPoint != null)
            {
                Instantiate(enemyPrefab, selectedSpawnPoint.position, Quaternion.identity);
            }
        }

        

        // Mark the wave as active.
        isWaveActive = true;
    }

    private void StartNextWave()
    {
        if (currentWave < totalWaves)
        {
            StartWave();
        }
    }

    private Transform GetClosestSpawnPoint()
    {
        Transform closestSpawnPoint = null;
        float closestDistance = float.MaxValue;

        foreach (Transform spawnPoint in spawnPoints)
        {
            // Check if the spawn point collides with the player using the "MainCamera" tag.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPoint.position, 0.2f);

            bool collidesWithPlayer = false;
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("MainCamera"))
                {
                    collidesWithPlayer = true;
                    break;
                }
            }

            // If it doesn't collide with the player and is closer than the current closest spawn point, update the selection.
            if (!collidesWithPlayer)
            {
                float distanceToSpawnPoint = Vector3.Distance(transform.position, spawnPoint.position);

                if (distanceToSpawnPoint < closestDistance)
                {
                    closestSpawnPoint = spawnPoint;
                    closestDistance = distanceToSpawnPoint;
                }
            }
        }

        return closestSpawnPoint;
    }
}
