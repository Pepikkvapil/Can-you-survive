using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FastEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
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

    void Update()
    {
        agent.SetDestination(player.position);
    }

    protected override void Die()
    {
        base.Die();
    }
}
