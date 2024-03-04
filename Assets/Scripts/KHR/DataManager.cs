using System.Collections;
using System.Collections.Generic;
using System.IO; //�����̶�� ģ���� ���� �ְ� ���ִ� ����
using System.Runtime.CompilerServices;
using UnityEngine;



[System.Serializable]

public class GameSaveData
{
    public List<float> playerCurValue = new List<float>();
    // ������ �� ������ �� �κ���: MonoBehaviour�� ��� �޾������� ������ ����
    public ItemData itemData;
    public int selectedItemIndex;
    public List<MonsterData> monsterData;
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerManager playerManager;
    public DayNightCycle dayNightCycle;  // �߰��� �Ӽ�
    public PlayerConditions playerConditions; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ ����

    [SerializeField] private string savePath; // ���� ���� ���� ���

    // ���� MonsterDataSO�� �ҷ���(ȣ��)
    //public MonsterDataSO monsterData;



    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        if (Instance != this)
    //        { Destroy(gameObject)}
    //    }

    //    savePath = Path.Combine(Application.persistentDataPath, "save.json");

    //}

    // ���̺� ��ư ui �� �������� ����ǵ��� 
    public void SaveGame()
    {
        GameSaveData gameSaveData = new GameSaveData();

        // �÷��̾� ���� ����
        gameSaveData.playerCurValue.Add(playerConditions.health.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stress.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.hunger.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stamina.curValue);
        //// ���� ���� ���� �ڵ� 
        //PlayerData data = playerManager.GetPlayerData();
        Debug.Log(MonsterDataManager.Instance.GetMonsters().Count);
        gameSaveData.monsterData = MonsterDataManager.Instance.GetMonsters();
        gameSaveData.selectedItemIndex = Inventory.instance.selectedItemIndex;
        //// ���� �ý����̳� �����ͺ��̽��� ����
        //SavePlayerDataToFile(data);

        // ���� ���� �����͸� JSON �������� ��ȯ�Ͽ� ���Ͽ� ����
        string jsonData = JsonUtility.ToJson(gameSaveData);
        File.WriteAllText(savePath, jsonData);

        PlayerPrefs.SetFloat("DayNightCycleTime", dayNightCycle.time);
        //PlayerPrefs.Save();
        Debug.Log("���̺��ưŬ��");

    }



    // ����ȭ�鿡�� �ҷ����� 
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("DayNightCycleTime"))
        {
            dayNightCycle.time = PlayerPrefs.GetFloat("DayNightCycleTime");
        }
        //���� ���� �ε� �ڵ�
        // �����̳� �����ͺ��̽����� �ε�
        //PlayerData data = LoadPlayerDataFromFile();

        //// �÷��̾� �Ŵ����� ����
        //playerManager.LoadPlayerData(data);

        Debug.Log("���̺��ưŬ��");

    }

    public void SetPlayerData()
    {
        if (InfoManager.instance.playerCurValue != null)
        {
            playerConditions.health.curValue = InfoManager.instance.playerCurValue[0];
            playerConditions.stress.curValue = InfoManager.instance.playerCurValue[1];
            playerConditions.hunger.curValue = InfoManager.instance.playerCurValue[2];
            playerConditions.stamina.curValue = InfoManager.instance.playerCurValue[3];

        }
        else
        {
            Debug.Log("�÷��̾� ����� �����Ͱ� �������� �ʽ��ϴ�.");
        }

    }

    public void SetMonsterData()
    {
        if (InfoManager.instance.monsterData != null)
        {
            int count = 0;
            foreach (var monster in InfoManager.instance.monsterData)
            {
                MonsterDataManager.Instance.monsters[count++] = monster;
            }
        }
        else
        {
            Debug.Log("���� �����Ͱ� �������� �ʽ��ϴ�.");
        }
    }

}


