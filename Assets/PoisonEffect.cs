using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public float duration = 5f; // Lifetime of the poison effect
    public int damage = 10;     // Damage done to the player
    private float damageInterval = 2f; // Interval between damages
    private Dictionary<Collider2D, float> lastDamageTimes; // Track last damage time for each player

    private void Start()
    {
        lastDamageTimes = new Dictionary<Collider2D, float>();
        // Destroy the poison effect after its duration
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryDamagePlayer(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryDamagePlayer(other);
        }
    }

    private void TryDamagePlayer(Collider2D playerCollider)
    {
        if (!lastDamageTimes.TryGetValue(playerCollider, out float lastDamageTime))
        {
            lastDamageTime = -damageInterval; // Set default value if player hasn't been damaged yet
        }

        if (Time.time - lastDamageTime >= damageInterval)
        {
            playerCollider.GetComponent<PlayerController>().Damage(damage);
            lastDamageTimes[playerCollider] = Time.time;
        }
    }
}
