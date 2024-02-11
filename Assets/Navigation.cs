using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ���� NavMesh �����ռ�

public class Navigation : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NavMeshHit closestHit;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(player.position);
    }

    void Update()
    {
        if(player != null)
        {
            agent.SetDestination(player.position);
        }
        Debug.Log("Destination: " + agent.destination);


    }

}

