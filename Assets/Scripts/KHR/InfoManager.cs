//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;

//public class InfoManager : MonoBehaviour
//{
//    //public PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ ����
//    public Inventory inventory; // �κ��丮 ��ũ��Ʈ ����
    
//    public MonsterManager monsterManager; // ���� �Ŵ��� ��ũ��Ʈ ����

//    private string savePath; // ���� ���� ���� ���

//    //private void Awake()
//    //{
//    //    savePath = Path.Combine(Application.persistentDataPath, "save.json");
//    //}

//    // ���� ���� �� ȣ��Ǵ� �޼���
//    public void SaveGame()
//    {
//        //GameSaveData gameSaveData = new GameSaveData();

//        //// �÷��̾� ���� ����
//        //gameSaveData.playerData = new PlayerData
//        //{
//        //    health = PlayerConditions.health,
//        //    stress = PlayerConditions.stress,
//        //    condition = PlayerConditions.condition,
//        //    hunger = PlayerConditions.hunger
//        //};

//        // ������ ���� ����
//        gameSaveData.itemData = new ItemData
//        {
//            equippedItemID = inventory.GetEquippedItemID(),
//            inventoryItems = inventory.GetInventoryItems()
//        };

//        // ���� ���� ����
//        gameSaveData.monsterData = new MonsterData
//        {
//            deadMonsterCount = monsterManager.GetDeadMonsterCount()
//        };

//        // ���� ���� �����͸� JSON �������� ��ȯ�Ͽ� ���Ͽ� ����
//        string jsonData = JsonUtility.ToJson(gameSaveData);
//        File.WriteAllText(savePath, jsonData);
//    }

//    // ���� �ҷ����� �� ȣ��Ǵ� �޼���
//    public void LoadGame()
//    {
//        if (!File.Exists(savePath))
//        {
//            // ����� ���� ������ ���� ��� �ε��� �� ������ �˸�
//            Debug.Log("����� ������ �����ϴ�.");
//            return;
//        }

//        // ���Ͽ��� ����� ���� �����͸� �о��
//        string jsonData = File.ReadAllText(savePath);
//        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);

//        //// �÷��̾� ���� �ε�
//        //PlayerData playerData = gameSaveData.playerData;
//        //playerController.SetPlayerData(playerData.health, playerData.stress, playerData.condition, playerData.hunger);

//        //// ������ ���� �ε�
//        //ItemData itemData = gameSaveData.itemData;
//        //inventory.LoadEquippedItem(itemData.equippedItemID);
//        //inventory.LoadInventoryItems(itemData.inventoryItems);

//        //// ���� ���� �ε�
//        //MonsterData monsterData = gameSaveData.monsterData;
//        //monsterManager.LoadDeadMonsterCount(monsterData.deadMonsterCount);
//        // �ش� �ּ� ó�� �κ��� ���ξ��� �ҷ��ö� ó�� (���ξ��� �ִ� ���� �Ŵ������� ó���ϸ� �ǰԲ�,,)
//    }
//}

////[System.Serializable]
////public class GameSaveData
////{
////    public PlayerData playerData;
////    public ItemData itemData;
////    public MapData mapData;
////    public MonsterData monsterData;
////}

//[System.Serializable]
//public class PlayerData
//{
//    public float health;
//    public float stress;
//    public float condition;
//    public float hunger;
//}

//[System.Serializable]
//public class ItemData
//{
//    public int equippedItemID;
//    public List<int> inventoryItems;
//}

//[System.Serializable]
//public class MapData
//{
//    public Vector3 mapPosition;
//    public List<Vector3> treePositions;
//    public List<Vector3> itemPositions;
//}

//[System.Serializable]
//public class MonsterData
//{
//    public int deadMonsterCount;
//}
