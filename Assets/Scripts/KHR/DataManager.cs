using System.Collections;
using System.Collections.Generic;
using System.IO; //�����̶�� ģ���� ���� �ְ� ���ִ� ����
using System.Runtime.CompilerServices;
using UnityEngine;


[System.Serializable]

public class GameSaveData
{
    public List<float> playerCurValue = new List<float>();
    // new List<float>(); ������ �� ������ �� �κ���: MonoBehaviour�� ��� �޾������� ������ ����
    public ItemData itemData;
    public int selectedItemIndex;
    public List<MonsterData> monsterData;
    public Vector3 playerPositions;
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerManager playerManager;
    public DayNightCycle dayNightCycle;  // �߰��� �Ӽ�
    public PlayerConditions playerConditions; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ ����(�÷��̾� ������Ʈ�� ����)

    [SerializeField] private string savePath; // ���� ���� ���� ���


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }          
        }
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    // ���̺� ��ư ui �� �������� �����  ���� 
    public void SaveGame()
    {
        GameSaveData gameSaveData = new GameSaveData();

        // �÷��̾� ���� ����
        gameSaveData.playerCurValue.Add(playerConditions.health.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stress.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.hunger.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stamina.curValue);
     
        Debug.Log(MonsterDataManager.Instance.GetMonsters().Count);
        gameSaveData.monsterData = MonsterDataManager.Instance.GetMonsters();
        gameSaveData.selectedItemIndex = Inventory.instance.selectedItemIndex;
        gameSaveData.playerPositions = playerConditions.transform.position;

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

        Debug.Log("���̺��ưŬ��");

    }

    //public void SetPlayerData() //�÷��̾�� ����� �ε带 ������ (��ŸƮ������ ȣ��)
    //{
    //    if (InfoManager.instance.IsLoad&&InfoManager.instance.playerCurValue != null)
    //    //�ҷ����⿡�� ������������, �����͸� �����⸦ �ϱ� ���� ����ó��
    //    {
    //        playerConditions.health.curValue = InfoManager.instance.playerCurValue[0];
    //        playerConditions.stress.curValue = InfoManager.instance.playerCurValue[1];
    //        playerConditions.hunger.curValue = InfoManager.instance.playerCurValue[2];
    //        playerConditions.stamina.curValue = InfoManager.instance.playerCurValue[3];
    //        GameManager.instance.PlayerObject.transform.position = InfoManager.instance.playerPositions;
    //        // �ε������� �÷��̾��� ��ġ�� �ʱ�ȭ 
    //    }
    //    else
    //    {
    //        Debug.Log("�÷��̾� ����� �����Ͱ� �������� �ʽ��ϴ�.");
    //    }
    //}

    public void SetMonsterData()//���Ϳ��� ����� �ε带 ������ (��ŸƮ������ ȣ��)
    {
        if (InfoManager.instance.IsLoad&&InfoManager.instance.monsterData != null)
            //�ҷ����⿡�� ������������, �����͸� �����⸦ �ϱ� ���� ����ó��
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


