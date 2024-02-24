using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    // Start is called before the first frame update

    public void ResetTheGame()
    {
        SceneManager.LoadScene("MenuScene");
        SceneManager.UnloadSceneAsync("SampleScene");
    }


}
