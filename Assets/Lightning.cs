using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScript : MonoBehaviour
{
    public int damage = 20; // Damage done by lightning

    private void Start()
    {
        Destroy(gameObject, 0.45f); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyController = other.GetComponent<Enemy>();
            if (!enemyController.recentlyHitByLightning)
            {
                enemyController.Damage(damage);
            }
        }
    }
}

