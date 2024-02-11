using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 导入 NavMesh 命名空间

public class Navigation : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public GameObject gameover;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(player.position);
        gameover.SetActive(false);
    }

    void Update()
    {
        if(player != null)
        {
            agent.SetDestination(player.position);
            //caught player, stop moving, show game over
            if(Vector3.Distance(player.position, transform.position) < 1.5f)
            {
                agent.isStopped = true;
                Debug.Log("Game Over");
                gameover.SetActive(true);
            }
        }


    }

}

