using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
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
            if(instance != this)
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
        SceneManager.LoadScene("MainScene 1");
        if (!File.Exists(savePath))
        {
            // ����� ���� ������ ���� ��� �ε��� �� ������ �˸�
            Debug.Log("����� ������ �����ϴ�.");
            return;
        }

        // ���Ͽ��� ����� ���� �����͸� �о��
        string jsonData = File.ReadAllText(savePath);
        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);

        // �÷��̾� ���� �ε�
        playerCurValue = gameSaveData.playerCurValue;
        //playerConditions.SetPlayerData(playerData.health, playerData.stress, playerData.condition, playerData.hunger);

        //// ������ ���� �ε�
        //ItemData itemData = gameSaveData.itemData;
        //inventory.LoadEquippedItem(itemData.equippedItemID);
        //inventory.LoadInventoryItems(itemData.inventoryItems);

        // ���� ���� �ε�
        monsterData = gameSaveData.monsterData;
        //monsterManager.LoadDeadMonsterCount(monsterData.deadMonsterCount);
        //�ش� �ּ� ó�� �κ��� ���ξ��� �ҷ��ö� ó��(���ξ��� �ִ� ���� �Ŵ������� ó���ϸ� �ǰԲ�,,)

    }
}





