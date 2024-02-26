using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ���� �Ŵ����� �̱��� �ν��Ͻ�
    public static GameManager instance;

    private void Awake()
    {
        // �̱��� ������ ���� �ڵ�
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������ ���۵� �� ȣ��Ǵ� �޼ҵ�
    public void StartGame()
    {
        Debug.Log("������ ���۵Ǿ����ϴ�.");
        // ���� ���ۿ� �ʿ��� �߰����� ������ ���⿡ �ۼ��ϼ���.
    }

    // ������ ���� �� ȣ��Ǵ� �޼ҵ�
    public void EndGame()
    {
        Debug.Log("������ ����Ǿ����ϴ�.");
        // ���� ���ῡ �ʿ��� �߰����� ������ ���⿡ �ۼ��ϼ���.
    }
}
