using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // ���������ռ��Է��ʳ���������

public class Navigation : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public GameObject gameover;
    public GameObject restart;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(player.position);
        gameover.SetActive(false);
        restart.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            //caught player, stop moving, show game over
            if (Vector3.Distance(player.position, transform.position) < 1f)
            {
                agent.isStopped = true;
                Debug.Log("Game Over");
                gameover.SetActive(true);
                restart.SetActive(true);
            }
        }
        // ���������UI��ʾ��������Ұ�����F���������¼��ص�ǰ����
        if (restart.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ReloadCurrentScene();
        }
    }
    void ReloadCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex; // ��ȡ��ǰ����������
        SceneManager.LoadScene(sceneIndex); // �����������¼��س���
    }
}

