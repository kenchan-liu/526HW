using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void UnlockLevel(int levelIndex)
    {
        // ������ӽ����ؿ����߼�
        // ���¹ؿ�״̬�����籣�浽���ƫ�û����ݿ���
    }

    // ����ÿ���ؿ���ť�����������
    public void OnLevelButtonClicked(string levelName)
    {
        LoadLevel(levelName);
    }
}