using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int meleeDmg = 10;
    public float damageMultiplier = 1f;

    public SpriteRenderer entitySpriteRenderer; // Assign the entity's SpriteRenderer component in the Inspector
    public Color redColor = Color.red; // Set the desired red color
    public float redDuration = 0.5f; // Adjust the duration as needed

    [SerializeField] private int health = 50;

    [SerializeField] private GameObject xpPrefab;

    PlayerController playerController;

    public float speed = 2f;

    public Transform player;
    public NavMeshAgent agent;

    public static float selfDamageMultiplier = 1f;
    public static float oldSelfDamageMultiplier = 1f;

    public bool recentlyHitByLightning = false;

    private bool isDead = false;

    private void Start()
    {
        if(playerController == null)
            playerController = FindAnyObjectByType<PlayerController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (playerController == null)
            playerController = FindAnyObjectByType<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.Damage(meleeDmg);
        }
    }

    public void UpdateStatsBasedOnWave(int wave)
    {
        // Example: You can increase health and damage based on the wave number
        int healthIncrease = wave * 2; // Adjust as needed
        float damageMultiplierIncrease = wave * 0.1f; // Adjust as needed

        // Apply the updates
        health += healthIncrease;
        damageMultiplier += damageMultiplierIncrease;
    }

    public static void IncreaseDamageTakenMultiplier(float amount)
    {
        selfDamageMultiplier += amount;
    }

    public IEnumerator ApplyDamageUpgrade(float multiplier, float duration)
    {
        oldSelfDamageMultiplier = selfDamageMultiplier;
        selfDamageMultiplier += multiplier; // Increase the damage

        yield return new WaitForSeconds(duration); // Wait for the effect to wear off

        selfDamageMultiplier = oldSelfDamageMultiplier; // Reset the damage back to original
    }

    public virtual void DamagingEnemy(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= (int)(amount * selfDamageMultiplier);

        // Change the material to red temporarily
        entitySpriteRenderer.color = redColor;

        // Start a coroutine to revert the color back to the original material
        StartCoroutine(RevertColor());


        if (health <= 0)
        {
            if (isDead == false)
            {
                Die();
                isDead = true;
            }
            
        }
    }

    private IEnumerator RevertColor()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(redDuration);

        // Revert the sprite color back to the original color (usually white)
        entitySpriteRenderer.color = Color.white; // You can set it to the original color
    }

    private void DropXP()
    {
        if (xpPrefab != null)
        {
            // Instantiate the XP prefab at the enemy's position
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }
    }


    protected virtual void Die()
    {
        DropXP();
        ExperienceManager.Instance.AddKilled(); 

        // Destroy the enemy or perform other cleanup
        Debug.Log("DEAD!");
        Destroy(gameObject);
    }


}