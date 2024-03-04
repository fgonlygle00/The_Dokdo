using System.Collections;
using System.Collections.Generic;
using System.IO; //�����̶�� ģ���� ���� �ְ� ���ִ� ����
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]

public class GameSaveData
{
    public List<float> playerCurValue = new List<float>();
    // new List<float>(); ������ �� ������ �� �κ���: MonoBehaviour�� ��� �޾������� ������ ����
    public ItemSlot[] slots;
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

    public ItemData LoadItemData(string itemName)
    {
        string json = PlayerPrefs.GetString(itemName);
        ItemData itemData = JsonUtility.FromJson<ItemData>(json);
        return itemData;
    }

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
            DontDestroyOnLoad(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }
     void Start()
    {
        playerConditions =  GameManager.instance.PlayerObject.GetComponent<PlayerConditions>();
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

        // ������ ���� ����
        gameSaveData.slots = Inventory.instance.slots;

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

    public void SetPlayerData() //�÷��̾�� ����� �ε带 ������ (��ŸƮ������ ȣ��)
    {
        if (InfoManager.instance.IsLoad && InfoManager.instance.playerCurValue != null)
        //�ҷ����⿡�� ������������, �����͸� �����⸦ �ϱ� ���� ����ó��
        {
            if(playerConditions == null)
            {
                playerConditions = GameManager.instance.PlayerObject.GetComponent<PlayerConditions>();
            }
            playerConditions.health.curValue = InfoManager.instance.playerCurValue[0];
            playerConditions.stress.curValue = InfoManager.instance.playerCurValue[1];
            playerConditions.hunger.curValue = InfoManager.instance.playerCurValue[2];
            playerConditions.stamina.curValue = InfoManager.instance.playerCurValue[3];
            //
            Scene scene = SceneManager.GetActiveScene();
            // ���� Ȱ��ȭ �� ���� �־���
            if (scene.name == "DungeonScene_1Zone" || scene.name == "DungeonScene_2Zone" || scene.name == "DungeonScene_3Zone")
            {
                return;
            }

            GameManager.instance.PlayerObject.transform.position = InfoManager.instance.playerPositions;
            // �ε������� �÷��̾��� ��ġ�� �ʱ�ȭ 
            //�ҷ��� ������ �����͸� InfoManager�� ����
            

        }
        else
        {
            Debug.Log("�÷��̾� ����� �����Ͱ� �������� �ʽ��ϴ�.");
        }
    }

    public void SetItemData()
    {
        if (InfoManager.instance.IsLoad && InfoManager.instance.slots != null)
        {
            //�ҷ����⿡�� ������������, �����͸� �����⸦ �ϱ� ���� ����ó��

            string itemName = "exampleItem"; // �ҷ��� ������ �̸��� ����
            ItemData itemData = LoadItemData(itemName);
            ItemSlotUI[] itemSlotUI = GameManager.instance.PlayerObject.GetComponent<Inventory>().uiSlots;
           
            GameManager.instance.PlayerObject.GetComponent<Inventory>().slots = InfoManager.instance.slots;
            // InfoManager�� ������ �����͸� ����
          
        }
    }

    public void SetMonsterData()//���Ϳ��� ����� �ε带 ������ (��ŸƮ������ ȣ��)
    {
        if (InfoManager.instance.IsLoad && InfoManager.instance.monsterData != null)
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


