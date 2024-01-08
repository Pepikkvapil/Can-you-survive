using UnityEngine;
using UnityEngine.AI;

public class FastEvolvedEnemy : Enemy
{
    public float evolvedSpeed = 4.0f; // Adjust the speed for the evolved enemy
    public int evolvedDamage = 30; // Adjust the damage for the evolved enemy

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

        // Customize the evolved enemy's behavior
        agent.speed = evolvedSpeed;
    }

    private void Update()
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

    protected override void Die()
    {
        base.Die();
    }
}
