using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{


    public SpriteRenderer entitySpriteRenderer; // Assign the entity's SpriteRenderer component in the Inspector
    public Color redColor = Color.red; // Set the desired red color
    public float redDuration = 0.5f; // Adjust the duration as needed

    [SerializeField] private int health = 50;

    [SerializeField] private GameObject xpPrefab;


    public float speed = 2f;

    public Transform player;
    public NavMeshAgent agent;

    public static float damageMultiplier = 1f;

    public bool recentlyHitByLightning = false;

    private bool isDead = false;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if the playerObject is not null before accessing its transform
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);

        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Flip the sprite based on player position
        if (directionToPlayer.x > 0)
        {
            // Player is on the right, flip the sprite
            entitySpriteRenderer.flipX = true;
        }
        else if (directionToPlayer.x < 0)
        {
            // Player is on the left, unflip the sprite
            entitySpriteRenderer.flipX = false;
        }
    }

    public static void IncreaseDamageMultiplier(float amount)
    {
        damageMultiplier += amount;
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= (int)(amount * damageMultiplier);

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