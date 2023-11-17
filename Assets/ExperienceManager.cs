using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public TMP_Text levelText;
    public TMP_Text killedText;
    public Image mask;
    public GameObject upgradeWindow; 

    public PlayerController playerController;

    public Button overallDamageButton; // Assign in the Unity Editor
    public Button speedButton; // Assign in the Unity Editor
    public Button healthButton; // Assign in the Unity Editor
    public Button bookButton; // Assign in the Unity Editor

    public int maximumExp = 20;
    public int currentExp = 0;
    public int currentLevel = 0;
    public int killedEnemies = 0;

    private List<UpgradeType> allUpgrades = new List<UpgradeType>()
    {
        UpgradeType.OverallDamage,
        UpgradeType.Speed,
        UpgradeType.Health,
        UpgradeType.Book
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        levelText.text = "Level: " + currentLevel.ToString();
        killedText.text = "Enemies killed: " + killedEnemies.ToString();
        upgradeWindow.SetActive(false); // Ensure the upgrade window is not visible at start
    }

    void Update()
    {
        GetCurrentFill(currentExp);
        levelText.text = "Level: " + currentLevel.ToString();
        killedText.text = "Enemies killed: " + killedEnemies.ToString();
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;
        if (currentExp >= maximumExp)
        {
            LevelUp();
        }
    }

    public void AddKilled()
    {
        killedEnemies++;
    }

    void GetCurrentFill(float test)
    {
        float fillAmount = (float)currentExp / (float)maximumExp;
        mask.fillAmount = fillAmount;
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp = 0;
        maximumExp += 10;
        ShowUpgradeOptions();
    }

    private void ShowUpgradeOptions()
    {
        List<UpgradeType> selectedUpgrades = PickRandomUpgrades(3);

        overallDamageButton.gameObject.SetActive(false);
        speedButton.gameObject.SetActive(false);
        healthButton.gameObject.SetActive(false);
        bookButton.gameObject.SetActive(false);

        foreach (var upgrade in selectedUpgrades)
        {
            switch (upgrade)
            {
                case UpgradeType.OverallDamage:
                    overallDamageButton.gameObject.SetActive(true);
                    break;
                case UpgradeType.Speed:
                    speedButton.gameObject.SetActive(true);
                    break;
                case UpgradeType.Health:
                    healthButton.gameObject.SetActive(true);
                    break;
                case UpgradeType.Book:
                    bookButton.gameObject.SetActive(true);
                    break;
            }
        }

        upgradeWindow.SetActive(true); // Show the upgrade window
        Time.timeScale = 0f; // Pause the game
    }

    private List<UpgradeType> PickRandomUpgrades(int numberOfUpgrades)
    {
        List<UpgradeType> shuffledList = new List<UpgradeType>(allUpgrades);
        int n = shuffledList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            UpgradeType value = shuffledList[k];
            shuffledList[k] = shuffledList[n];
            shuffledList[n] = value;
        }
        return shuffledList.GetRange(0, numberOfUpgrades);
    }

    public void ChooseUpgrade(string upgradeType)
    {
        switch (upgradeType)
        {
            case "Speed":
                playerController.IncreaseSpeedMultiplier(0.1f);
                break;
            case "Damage":
                BulletScript.IncreaseDamageMultiplier(0.2f); // Increase by 20% for example
                break;
            case "Health":
                playerController.IncreaseMaxHealth(20);
                break;
            case "Book":
                playerController.UpgradeBookWeapon();
                break;
        }

        OnUpgradeSelected();
    }

    private void OnUpgradeSelected()
    {
        upgradeWindow.SetActive(false); // Hide the upgrade window
        Time.timeScale = 1f; // Resume the game
    }
}

public enum UpgradeType
{
    OverallDamage,
    Speed,
    Health,
    Book
}
