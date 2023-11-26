using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public float duration = 5f; // Lifetime of the poison effect
    public int damage = 10;     // Damage done to the player

    private void Start()
    {
        // Destroy the poison effect after its duration
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player
            other.GetComponent<PlayerController>().Damage(damage);
        }
    }
}
