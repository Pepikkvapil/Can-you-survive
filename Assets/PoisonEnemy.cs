using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoisonEnemy : Enemy
{
    public GameObject poisonPrefab; // Assign this in the Unity Editor
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

    private void Update()
    {
        agent.SetDestination(player.position);
    }
    protected override void Die()
    {
        Instantiate(poisonPrefab, transform.position, Quaternion.identity);
        base.Die();
    }
}
