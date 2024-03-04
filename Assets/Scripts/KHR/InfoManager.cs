using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
    public Vector3 playerPositions;
    public bool IsLoad = false;
    public ItemSlot[] slots;
    public static InfoManager instance;
    public List<float> playerCurValue = new List<float>();
    public List<MonsterData> monsterData = new List<MonsterData>();
    [SerializeField] private string savePath; // ���� ���� ���� ���

    private void Awake() // �̱���ȭ
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    // ���� ���� �� ȣ��Ǵ� �޼���

    // ���� �ҷ����� �� ȣ��Ǵ� �޼���
    public void LoadGame()
    {
        SceneManager.LoadScene("MainScene");
        if (!File.Exists(savePath))
        {
            // ����� ���� ������ ���� ��� �ε��� �� ������ �˸�
            Debug.Log("����� ������ �����ϴ�.");
            return;
        }
        IsLoad = true;

        // ���Ͽ��� ����� ���� �����͸� �о��
        string jsonData = File.ReadAllText(savePath);
        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);
        playerPositions = gameSaveData.playerPositions;

        // �÷��̾� ���� �ε�
        playerCurValue = gameSaveData.playerCurValue;
        //// ������ ���� �ε�
        slots = gameSaveData.slots;


        // ���� ���� �ε�
        monsterData = gameSaveData.monsterData;
    }
}





