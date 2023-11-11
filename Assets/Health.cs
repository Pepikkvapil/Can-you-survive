using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{


    public SpriteRenderer entitySpriteRenderer; // Assign the entity's SpriteRenderer component in the Inspector
    public Color redColor = Color.red; // Set the desired red color
    public float redDuration = 0.5f; // Adjust the duration as needed

    [SerializeField] private int health = 100;

    [SerializeField] private GameObject xpPrefab;

    private int MAX_HEALTH = 100;
 

    private void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Damage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            // Heal(10);
        }
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= amount;

        // Change the material to red temporarily
        entitySpriteRenderer.color = redColor;

        // Start a coroutine to revert the color back to the original material
        StartCoroutine(RevertColor());


        if (health <= 0)
        {
            DropXP();
            Die();
        }
    }

    private IEnumerator RevertColor()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(redDuration);

        // Revert the sprite color back to the original color (usually white)
        entitySpriteRenderer.color = Color.white; // You can set it to the original color
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void DropXP()
    {
        if (xpPrefab != null)
        {
            // Instantiate the XP prefab at the enemy's position
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Die()
    {
        ExperienceManager.Instance.AddKilled();

        // Destroy the enemy or perform other cleanup
        Debug.Log("DEAD!");
        Destroy(gameObject);
    }


}