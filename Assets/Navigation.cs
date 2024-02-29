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
    public bool getconfused = false;


    private Vector3 stopPosition;
    private bool playerStopped = false;

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
        if (player != null && !getconfused)
        {
            if (!playerStopped)
            {
                agent.SetDestination(player.position);
            }
            //caught player, stop moving, show game over
            if (Vector3.Distance(player.position, transform.position) < 0.8f)
            {
                agent.isStopped = true;
                gameover.SetActive(true);
                restart.SetActive(true);
                // ������ҵĵ�ǰλ�ã�����playerStopped����Ϊtrue
                stopPosition = player.position;
                playerStopped = true;
            }
        }
        // ���playerStoppedΪtrue����ǿ�����ͣ����stopPositionλ��
        if (playerStopped)
        {
            player.position = stopPosition;
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

