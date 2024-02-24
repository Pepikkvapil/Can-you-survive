using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : Enemy
{
    public GameObject barrierPrefab; // Assign your barrier prefab in the inspector
    public Vector2 arenaSize = new Vector2(10f, 10f); // The size of the arena
    private List<GameObject> spawnedBarriers = new List<GameObject>(); // To keep track of spawned barriers

    public GameObject bulletPrefab; // Assign your bullet prefab in the inspector
    public float shootingRange = 10f; // The range within which the boss will shoot
    public float shootCooldown = 2f; // Cooldown period between shots
    public float bulletSpeed = 10f; // Speed of the bullet

    private float lastShotTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        SetupBossArena();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if the playerObject is not null before accessing its transform
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }
    
    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange && Time.time - lastShotTime >= shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }

        agent.SetDestination(player.position);

        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

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

    void Shoot()
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


    void SetupBossArena()
    {
        player.position = Vector3.zero; // Teleport player to the center of the arena

        // Define barrier positions based on arenaSize and player position
        Vector3[] barrierPositions = new Vector3[]
        {
            new Vector3(-arenaSize.x / 2, 0, 0), // Left
            new Vector3(arenaSize.x / 2, 0, 0), // Right
            new Vector3(0, -arenaSize.y / 2, 0), // Bottom
            new Vector3(0, arenaSize.y / 2, 0)  // Top
        };

        Quaternion[] barrierRotations = new Quaternion[]
        {
            Quaternion.Euler(0, 0, 90), // Left
            Quaternion.Euler(0, 0, 90), // Right
            Quaternion.identity, // Bottom
            Quaternion.identity // Top
        };

        for (int i = 0; i < barrierPositions.Length; i++)
        {
            GameObject barrier = Instantiate(barrierPrefab, player.position + barrierPositions[i], barrierRotations[i]);
            spawnedBarriers.Add(barrier);
        }
    }

    protected override void Die()
    {
        base.Die(); // Call base class die functionality if it exists

        // Destroy all barriers when the boss dies
        foreach (GameObject barrier in spawnedBarriers)
        {
            Destroy(barrier);
        }
        spawnedBarriers.Clear();
    }

}
