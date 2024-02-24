using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject EscapseUI;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause state
            isPaused = !isPaused;

            // Show/hide pause UI
            EscapseUI.SetActive(isPaused);

            // Pause/unpause the game
            if (isPaused)
            {
                Time.timeScale = 0f; // This freezes the game
            }
            else
            {
                Time.timeScale = 1f; // This resumes the game
            }
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }
}
