using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetSceneByName("SampleScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("SampleScene");
        }
    }

    public void PlayGame()
    {
        // Load the game scene (e.g., "SampleScene")
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
