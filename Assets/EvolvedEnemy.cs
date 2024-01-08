using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class EvolvedEnemy : Enemy
{
    public GameObject enemyDropPrefab; 


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


    protected override void Die()
    {
        GameObject newEnemy1 = Instantiate(enemyDropPrefab, transform.position + Vector3.left, Quaternion.identity);
        GameObject newEnemy2 = Instantiate(enemyDropPrefab, transform.position + Vector3.right, Quaternion.identity);

        base.Die();

    }


}