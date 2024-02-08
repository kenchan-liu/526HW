using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 导入 NavMesh 命名空间

public class Navigation : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float detectionDistance = 5f; // 检测障碍物的距离
    public LayerMask obstaclesLayer; // 障碍物所在的层
    public float closeDistanceThreshold = 2f; // 判定为“近”的距离阈值

    void Start()
    {

    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float currentSpeed = speed;

        // 进行射线检测，判断前方是否有障碍物
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectionDistance, obstaclesLayer))
        {
            float distance = hit.distance;

            // 根据障碍物的距离动态调整移动方向和速度
            if (distance < closeDistanceThreshold)
            {
                // 当障碍物非常近时，减速并尝试更快地调整方向
                currentSpeed *= 0.5f; // 减速
                direction += hit.normal * 0.5f; // 调整方向以避开障碍物
            }
            else
            {
                // 当障碍物较远时，轻微调整方向
                direction += hit.normal * 0.1f; // 轻微调整方向
            }
        }

        // 应用移动
        Move(direction.normalized, currentSpeed);
    }

    void Move(Vector3 direction, float currentSpeed)
    {
        transform.position += direction * currentSpeed * Time.deltaTime;
    }
}

