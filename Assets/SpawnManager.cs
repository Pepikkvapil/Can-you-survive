using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public TMP_Text waveText;
    public PlayerController playerController;

    [SerializeField] private float spawnMargin = 10.0f; // Margin outside camera bounds for spawning enemies
    [SerializeField] private int maxEnemiesPerWave = 3; // Maximum number of enemies per wave
    [SerializeField] private int totalWaves; // Total number of waves

    public GameObject[] enemyPool1; // pool 1
    public GameObject[] enemyPool2; // pool 2
    public GameObject[] enemyPool3; // pool 3
    public GameObject[] enemyPool4; // pool 4
    public GameObject[] enemyPool5; // pool 5
    public GameObject[] enemyPool6; // pool 6
    public GameObject[] enemyPool7; // pool 7
    public GameObject[] enemyPool8; // pool 8

    public int pool1StartWave = 1; // Wave using pool 1
    public int pool2StartWave = 25; // Wave using pool 2
    public int pool3StartWave = 50; // Wave using pool 3
    public int pool4StartWave = 75; // Wave using pool 4
    public int pool5StartWave = 100; // Wave using pool 5
    public int pool6StartWave = 125; // Wave using pool 6
    public int pool7StartWave = 150; // Wave using pool 7
    public int pool8StartWave = 175; // Wave using pool 8

    private int currentWave = 0;
    private bool isWaveActive = false;

    private void Start()
    {
        Invoke(nameof(StartWave), 5f);
    }

    private void Update()
    {
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemiesRemaining = totalEnemies.Length;

        if (isWaveActive && enemiesRemaining <= 0)
        {
            isWaveActive = false;
            StartNextWave();
        }

        Debug.Log("Enemies Remaining: " + enemiesRemaining); // Debug log for remaining enemies
    }

    private void StartWave()
    {
        currentWave++;
        waveText.text = "Wave " + currentWave;

        // Determine which pool of enemies to use based on the current wave
        GameObject[] chosenEnemyPool = GetEnemyPoolForWave(currentWave);

        for (int i = 0; i < maxEnemiesPerWave; i++)
        {
            Vector3 spawnPosition = GetSpawnPositionOutsideCameraView();

            // Randomly pick an enemy type from the chosen pool
            int randomIndex = Random.Range(0, chosenEnemyPool.Length);
            GameObject chosenEnemyPrefab = chosenEnemyPool[randomIndex];

            // Instantiate the chosen enemy at the calculated spawn position
            Instantiate(chosenEnemyPrefab, spawnPosition, Quaternion.identity);
        }

        isWaveActive = true;
    }

    private void StartNextWave()
    {
        maxEnemiesPerWave++;
        if (currentWave < totalWaves)
        {
            StartWave();
        }
    }

    private GameObject[] GetEnemyPoolForWave(int wave)
    {
        if (wave >= pool1StartWave && wave < pool2StartWave)
        {
            return enemyPool1;
        }
        else if (wave >= pool2StartWave && wave < pool3StartWave)
        {
            return enemyPool2;
        }
        else if (wave >= pool3StartWave && wave < pool4StartWave)
        {
            return enemyPool3;
        }
        else if (wave >= pool4StartWave && wave < pool5StartWave)
        {
            return enemyPool4;
        }
        else if (wave >= pool5StartWave && wave < pool6StartWave)
        {
            return enemyPool5;
        }
        else if (wave >= pool6StartWave && wave < pool7StartWave)
        {
            return enemyPool6;
        }
        else if (wave >= pool7StartWave && wave < pool8StartWave)
        {
            return enemyPool7;
        }
        else
        {
            return enemyPool8;
        }
    }


    private Vector3 GetSpawnPositionOutsideCameraView()
    {
        Camera mainCamera = Camera.main;
        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;

        while (!validPosition)
        {
            spawnPosition = GenerateRandomPositionAroundCamera(mainCamera, spawnMargin);

            // Check for overlap with walls using a Physics check
            if (!Physics2D.OverlapCircle(spawnPosition, 0.5f)) // Adjust the radius as needed
            {
                validPosition = true;
            }
        }

        return spawnPosition;
    }

    private Vector3 GenerateRandomPositionAroundCamera(Camera camera, float margin)
    {
        // Determine the camera's bounds
        float cameraHeight = 2f * camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;
        float halfCameraWidth = cameraWidth * 0.5f;
        float halfCameraHeight = cameraHeight * 0.5f;

        // Get the camera's position
        Vector3 cameraPosition = camera.transform.position;

        // Generate a random point around the camera bounds
        float x = 0f;
        float y = 0f;

        if (Random.value > 0.5f)
        {
            // Spawn at a random point along the horizontal margin
            x = cameraPosition.x + (Random.value > 0.5f ? halfCameraWidth + margin : -halfCameraWidth - margin);
            y = Random.Range(cameraPosition.y - halfCameraHeight - margin, cameraPosition.y + halfCameraHeight + margin);
        }
        else
        {
            // Spawn at a random point along the vertical margin
            x = Random.Range(cameraPosition.x - halfCameraWidth - margin, cameraPosition.x + halfCameraWidth + margin);
            y = cameraPosition.y + (Random.value > 0.5f ? halfCameraHeight + margin : -halfCameraHeight - margin);
        }

        return new Vector3(x, y, 0); 
    }

}
