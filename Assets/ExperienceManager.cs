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

    RocketsWeapon rocketsWeapon;
    BookWeapon bookWeapon;
    LightningScript lightningweapon;
    

    public Button overallDamageButton; //Assigning buttons in the Unity Editor
    public Button speedButton; 
    public Button healthButton; 
    public Button bookButton; 
    public Button rocketButton;
    public Button spellShieldButton;
    public Button lightningButton;

    public int maximumExp = 20;
    public int currentExp = 0;
    public int currentLevel = 0;
    public int killedEnemies = 0;
    public int damageupgrades = 0;

    bool isRocketFinalUpgrade = false;
    bool isBookFinalUpgrade = false;
    bool isSpeedFinalUpgrade = false;
    bool isLightningFinalUpgrade = false;
    bool isHealthFinalUpgrade = false;
    bool isSpellShieldFinalUpgrade = false;
    bool isDamageFinalUpgrade = false;

    public GameObject rocketPanel;
    public GameObject lightningPanel;
    public GameObject bookPanel;
    public GameObject shieldPanel;

    private List<UpgradeType> allUpgrades = new List<UpgradeType>()
    {
        UpgradeType.OverallDamage,
        UpgradeType.Speed,
        UpgradeType.Health,
        UpgradeType.Book,
        UpgradeType.Rocket,
        UpgradeType.SpellShield,
        UpgradeType.Lightning
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

        if(rocketsWeapon == null)
            rocketsWeapon = FindObjectOfType<RocketsWeapon>();
        

        if(bookWeapon == null)
            bookWeapon = FindObjectOfType<BookWeapon>();

        if (lightningweapon == null)
            lightningweapon = FindObjectOfType<LightningScript>();

        // Check and set the final upgrade flags
        if (rocketsWeapon != null)
        {
            isRocketFinalUpgrade = rocketsWeapon.FinalRocketUpgraded;
        }
        if (bookWeapon != null)
        {
            isBookFinalUpgrade = bookWeapon.FinalBookUpgraded;
        }
        if (lightningweapon != null)
        {
            isLightningFinalUpgrade = playerController.lightningupgraded;
        }

        isHealthFinalUpgrade = playerController.upgradedhealth;
        isSpeedFinalUpgrade = playerController.upgradedspeed;
        isSpellShieldFinalUpgrade = playerController.spellshieldupgraded;

        // Remove final upgrades from the list
        if (isRocketFinalUpgrade)
        {
            allUpgrades.Remove(UpgradeType.Rocket);
            Debug.Log("Rocket upgrade removed from options");
        }
        if (isBookFinalUpgrade)
        {
            allUpgrades.Remove(UpgradeType.Book);
            Debug.Log("Book upgrade removed from options");
        }
        if(isSpeedFinalUpgrade)
        {
            allUpgrades.Remove(UpgradeType.Speed);
            Debug.Log("Speed upgrade removed from options");
        }
        if (isLightningFinalUpgrade)
        {
            allUpgrades.Remove(UpgradeType.Lightning);
            Debug.Log("Lightning upgrade removed from options");
        }
        if (isHealthFinalUpgrade)
        {
            allUpgrades.Remove(UpgradeType.Health);
            Debug.Log("Health upgrade removed from options");
        }
        if (isSpellShieldFinalUpgrade)
        {
            allUpgrades.Remove(UpgradeType.SpellShield);
            Debug.Log("SpellShield upgrade removed from options");
        }
        if (damageupgrades == 5)
        {
            allUpgrades.Remove(UpgradeType.OverallDamage);
            Debug.Log("OverallDamage upgrade removed from options");
        }
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

        // Reset all buttons to inactive
        overallDamageButton.gameObject.SetActive(false);
        speedButton.gameObject.SetActive(false);
        healthButton.gameObject.SetActive(false);
        bookButton.gameObject.SetActive(false);
        rocketButton.gameObject.SetActive(false);
        spellShieldButton.gameObject.SetActive(false);
        lightningButton.gameObject.SetActive(false);

        // Activate buttons based on remaining upgrades in the list
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
                case UpgradeType.Rocket:
                    rocketButton.gameObject.SetActive(true);
                    break;
                case UpgradeType.SpellShield:
                    spellShieldButton.gameObject.SetActive(true);
                    break;
                case UpgradeType.Lightning:
                    lightningButton.gameObject.SetActive(true);
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
                Enemy.IncreaseDamageTakenMultiplier(0.1f);
                damageupgrades++;
                break;
            case "Health":
                playerController.IncreaseMaxHealth();
                break;
            case "Book":
                playerController.UpgradeBookWeapon();
                bookPanel.SetActive(true);
                break;
            case "Rockets":
                playerController.rocketsWeapon.UpgradeRocketWeapon();
                playerController.EnableRocketWeapon();
                rocketPanel.SetActive(true);
                break;
            case "SpellShield":
                playerController.EnableSpellShieldUpgrade();
                shieldPanel.SetActive(true);
                break;
            case "Lightning":
                playerController.EnableLightningWeapon();
                lightningPanel.SetActive(true);
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
    Book,
    Rocket,
    SpellShield,
    Lightning
}
