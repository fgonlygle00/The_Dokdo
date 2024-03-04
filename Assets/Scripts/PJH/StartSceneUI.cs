using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartSceneUI : MonoBehaviour
{

    [SerializeField] private Button LoadButton;

    // Start is called before the first frame update
    void Start()
    {
        LoadButton.onClick.AddListener(() => InfoManager.instance.LoadGame());
        //�ε� ��ư�� �ڵ�� ��������

    }


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
