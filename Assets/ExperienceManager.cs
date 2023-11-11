using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public TMP_Text levelText;
    public TMP_Text killedText;
    public Image mask;
    public GameObject upgradeWindow; // Assign the upgrade panel in the Inspector

    public int maximumExp = 20;
    public int currentExp = 0;
    public int currentLevel = 0;
    public int killedEnemies = 0;

    public PlayerController playerController;

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

    void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

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
        upgradeWindow.SetActive(true); // Show the upgrade window
        Time.timeScale = 0f; // Pause the game
    }

    public void OnUpgradeSelected()
    {
        upgradeWindow.SetActive(false); // Hide the upgrade window
        Time.timeScale = 1f; // Resume the game
        // Apply the selected upgrade (will expand this method with actual upgrade logic later)
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
        }

        OnUpgradeSelected(); // Hide the upgrade window and resume the game
    }
}
