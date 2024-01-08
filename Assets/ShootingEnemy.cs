using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : Enemy
{
    public float stoppingDistance = 5f;
    public float shootingRange = 10f;
    public GameObject bulletPrefab;
    public float shootCooldown = 2f;
    public float bulletSpeed = 10f; // Adjust this speed as needed

    private float lastShotTime;

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

    protected new void Update()
    {
        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            // Check if enough time has passed since the last shot
            if (Time.time - lastShotTime >= shootCooldown)
            {
                // Call the Shoot method to fire a bullet in the direction of the player
                Shoot();

                lastShotTime = Time.time;
            }
        }
        else if (distanceToPlayer <= stoppingDistance)
        {
            // Stop and face the player
            agent.SetDestination(transform.position);
        }
        else
        {
            // Move towards the player
            agent.SetDestination(player.position);
        }
    }

    protected new void Shoot()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Calculate the direction from the enemy to the player at the moment of shooting
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Create a bullet and set its initial position
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bullet.GetComponent<EnemyBullet>().SetTarget(playerObject.transform);
        }
    }




}
