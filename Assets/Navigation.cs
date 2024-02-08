using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // ���� NavMesh �����ռ�

public class Navigation : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float detectionDistance = 5f; // ����ϰ���ľ���
    public LayerMask obstaclesLayer; // �ϰ������ڵĲ�
    public float closeDistanceThreshold = 2f; // �ж�Ϊ�������ľ�����ֵ

    void Start()
    {

    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float currentSpeed = speed;

        // �������߼�⣬�ж�ǰ���Ƿ����ϰ���
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectionDistance, obstaclesLayer))
        {
            float distance = hit.distance;

            // �����ϰ���ľ��붯̬�����ƶ�������ٶ�
            if (distance < closeDistanceThreshold)
            {
                // ���ϰ���ǳ���ʱ�����ٲ����Ը���ص�������
                currentSpeed *= 0.5f; // ����
                direction += hit.normal * 0.5f; // ���������Աܿ��ϰ���
            }
            else
            {
                // ���ϰ����Զʱ����΢��������
                direction += hit.normal * 0.1f; // ��΢��������
            }
        }

        // Ӧ���ƶ�
        Move(direction.normalized, currentSpeed);
    }

    void Move(Vector3 direction, float currentSpeed)
    {
        transform.position += direction * currentSpeed * Time.deltaTime;
    }
}

