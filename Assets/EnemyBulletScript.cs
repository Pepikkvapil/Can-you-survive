using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f; // Bullet speed
    public int damage = 10;  // Damage inflicted on the player

    private Rigidbody2D rb;

    private Transform target;
    private Vector3 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed; // Move the bullet in its forward direction
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            lastDirection = (target.position - transform.position).normalized;
            RotateTowards(lastDirection);
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        // For a 2D game
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust the offset if needed
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with the player
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // Damage the player
            playerController.Damage(damage);

            // Destroy the bullet on collision with the player
            Destroy(gameObject);
        }
        else
        {
            // If the bullet collided with something else, just destroy it
            Destroy(gameObject);
        }
    }

}
