using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public TMP_Text waveText;
    public GameObject enemyPrefab;

    [SerializeField] private float spawnMargin = 10.0f; // Margin outside camera bounds for spawning enemies
    [SerializeField] private int maxEnemiesPerWave = 3; // Maximum number of enemies per wave
    [SerializeField] private int totalWaves = 5; // Total number of waves

    private int currentWave = 0; // Tracks the current wave
    private bool isWaveActive = false; // Tracks if a wave is currently active

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
            // All enemies from the current wave are defeated
            isWaveActive = false;
            StartNextWave();
        }

        Debug.Log("Enemies Remaining: " + enemiesRemaining); // Debug log for remaining enemies
    }

    private void StartWave()
    {
        currentWave++;
        waveText.text = "Wave " + currentWave;

        for (int i = 0; i < maxEnemiesPerWave; i++)
        {
            Vector3 spawnPosition = GetSpawnPositionOutsideCameraView();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }

        isWaveActive = true; // Mark the wave as active
    }

    private void StartNextWave()
    {
        if (currentWave < totalWaves)
        {
            StartWave();
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

        return new Vector3(x, y, 0); // Assuming a 2D game
    }

}
