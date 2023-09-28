using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[ExecuteInEditMode()]
public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public TMP_Text levelText;

    public int maximumExp = 20;

    public int currentExp = 0;

    public Image mask;

    public int currentLevel = 0;

    //Jen jeden Manager
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        levelText.text = "Level: " + currentLevel.ToString();
    }

    void Update()
    {
       GetCurrentFill(currentExp);
        levelText.text = "Level: " + currentLevel.ToString();
    }

    public void AddExperience(int amount)
    {
        currentExp += 5;
        if (currentExp >= maximumExp)
        {
            LevelUp();
        }
    }

    void GetCurrentFill(float test)
    {
        float fillAmount = (float)currentExp / (float)maximumExp;
        Debug.Log(fillAmount);
        mask.fillAmount = fillAmount;
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp = 0;
        maximumExp += 10;
    }



  
}