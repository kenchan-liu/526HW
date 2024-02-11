using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public GameObject gameover;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.updateUpAxis = true;
        agent.SetDestination(player.position);
        gameover.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            //caught player, stop moving, show game over
            if (Vector3.Distance(player.position, transform.position) < 1.5f)
            {
                agent.isStopped = true;
                Debug.Log("Game Over");
                gameover.SetActive(true);
            }
        }
    }
}

