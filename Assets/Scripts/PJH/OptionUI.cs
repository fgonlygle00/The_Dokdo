using UnityEngine;

public class OptionUI : MonoBehaviour
{
    public GameObject OptionWindow;

    // â ����
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OptionWindow.SetActive(true);
        }
    }

    // â �ݱ�
    public void CloseWindow()
    {
        OptionWindow.SetActive(false);
    }

    // ���� ȭ������ �̵�
    public void HomeGame()
    {
        LoadingSceneManager.LoadScene("StartScene");
    }

    // ���� ����
    public void ExitGame()
    {
        // �ش� �ڵ�� �����Ϳ� ����� �����ϴ°�
        UnityEditor.EditorApplication.isPlaying = false;
        // ��¥ ������ �����ϴ°�
        Application.Quit();
    }
}
