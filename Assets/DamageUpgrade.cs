using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUpgrade : MonoBehaviour
{
    public float damageMultiplier = 1.5f; // The factor by which the damage is multiplied
    public float duration = 10f; // Duration of the damage upgrade effect
    Enemy enemy = null;

    private void Update()
    {
        if(enemy == null)
            enemy = FindAnyObjectByType<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && HUDManager.Instance.activedmg == false)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                StartCoroutine(enemy.ApplyDamageUpgrade(damageMultiplier, duration));

                // Assuming you have a UIManager or similar to manage UI elements
                HUDManager.Instance.SetDamageUpgradeTimer(duration);
            }

            // Notify the corresponding UpgradeSpawner that the upgrade is deactivated
            UpgradeSpawner spawner = FindObjectOfType<UpgradeSpawner>();

            // Destroy the upgrade object
            Destroy(gameObject);
        }
    }
}
