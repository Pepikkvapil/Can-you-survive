using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : MonoBehaviour
{
    public float healthPercentageIncrease = 0.10f; // X% increase

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Calculate the amount of health to increase based on the percentage
                int healthIncreaseAmount = Mathf.RoundToInt(playerController.MAX_HEALTH * healthPercentageIncrease);
                playerController.Heal(healthIncreaseAmount);
            }

            // Notify the corresponding UpgradeSpawner that the upgrade is deactivated
            UpgradeSpawner spawner = FindObjectOfType<UpgradeSpawner>(); // This finds any UpgradeSpawner in the scene

            // Destroy the upgrade object
            Destroy(gameObject);
        }
    }
}

