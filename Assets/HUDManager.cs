using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public Image damageUpgradeFillBar;

    public GameObject statWindow;

    public GameObject rocketPanel;
    public GameObject lightningPanel;
    public GameObject bookPanel;
    public GameObject shieldPanel;

    public GameObject DMGTimer;

    public bool activedmg;

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        statWindow.SetActive(false); // Hide the HUD at the start

        rocketPanel.SetActive(false);
        lightningPanel.SetActive(false);
        bookPanel.SetActive(false);
        shieldPanel.SetActive(false);
        DMGTimer.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            statWindow.SetActive(!statWindow.activeSelf); // Toggle visibility
        }
    }

    public void SetDamageUpgradeTimer(float duration)
    {
        activedmg = true;
        DMGTimer.SetActive(true);
        if (damageUpgradeFillBar != null)
        {
            StopAllCoroutines(); // Stop existing countdowns to avoid overlaps
            StartCoroutine(DamageUpgradeTimer(duration));
        }

    }

    private IEnumerator DamageUpgradeTimer(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            damageUpgradeFillBar.fillAmount = timer / duration;
            yield return null;
        }
        DMGTimer.SetActive(false);
        activedmg = false;
    }
}
