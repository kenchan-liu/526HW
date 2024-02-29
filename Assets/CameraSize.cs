using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]

public class CameraSize : MonoBehaviour
{
    // Ŀ���߱�
    private float targetAspectRatio = 1.6f; // ���磬960x600�Ŀ�߱���1.6

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // ��ȡ��ǰ��������
        Camera camera = GetComponent<Camera>();

        // ���㵱ǰ���ڵĿ�߱�
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;

        // �������ű���
        float scaleHeight = windowAspectRatio / targetAspectRatio;

        // �������ű�������camera��viewportRect
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}