using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChase : MonoBehaviour
{
    public float speed;

    private Transform player;
    NavMeshAgent agent;

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
        //player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 direction = player.transform.position - transform.position;
        //direction.Normalize();
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        ////transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        agent.SetDestination(player.position);
    }
}
