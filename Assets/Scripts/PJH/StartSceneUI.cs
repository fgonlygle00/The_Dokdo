using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    // ���ν���
    public void NewGame()
    {
        LoadingSceneManager.LoadScene("MainScene");
    }

    // ����
    public void ExitGame()
    {
        // �ش� �ڵ�� �����Ϳ� ����� �����ϴ°�
        UnityEditor.EditorApplication.isPlaying = false;
        // ��¥ ������ �����ϴ°�
        Application.Quit();
    }
}
