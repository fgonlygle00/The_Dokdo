//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;

//public class InfoManager : MonoBehaviour
//{
//    //public PlayerController playerController; // 플레이어 컨트롤러 스크립트 참조
//    public Inventory inventory; // 인벤토리 스크립트 참조
    
//    public MonsterManager monsterManager; // 몬스터 매니저 스크립트 참조

//    private string savePath; // 게임 저장 파일 경로

//    //private void Awake()
//    //{
//    //    savePath = Path.Combine(Application.persistentDataPath, "save.json");
//    //}

//    // 게임 저장 시 호출되는 메서드
//    public void SaveGame()
//    {
//        //GameSaveData gameSaveData = new GameSaveData();

//        //// 플레이어 정보 저장
//        //gameSaveData.playerData = new PlayerData
//        //{
//        //    health = PlayerConditions.health,
//        //    stress = PlayerConditions.stress,
//        //    condition = PlayerConditions.condition,
//        //    hunger = PlayerConditions.hunger
//        //};

//        // 아이템 정보 저장
//        gameSaveData.itemData = new ItemData
//        {
//            equippedItemID = inventory.GetEquippedItemID(),
//            inventoryItems = inventory.GetInventoryItems()
//        };

//        // 몬스터 정보 저장
//        gameSaveData.monsterData = new MonsterData
//        {
//            deadMonsterCount = monsterManager.GetDeadMonsterCount()
//        };

//        // 게임 저장 데이터를 JSON 형식으로 변환하여 파일에 저장
//        string jsonData = JsonUtility.ToJson(gameSaveData);
//        File.WriteAllText(savePath, jsonData);
//    }

//    // 게임 불러오기 시 호출되는 메서드
//    public void LoadGame()
//    {
//        if (!File.Exists(savePath))
//        {
//            // 저장된 게임 파일이 없을 경우 로드할 수 없음을 알림
//            Debug.Log("저장된 게임이 없습니다.");
//            return;
//        }

//        // 파일에서 저장된 게임 데이터를 읽어옴
//        string jsonData = File.ReadAllText(savePath);
//        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);

//        //// 플레이어 정보 로드
//        //PlayerData playerData = gameSaveData.playerData;
//        //playerController.SetPlayerData(playerData.health, playerData.stress, playerData.condition, playerData.hunger);

//        //// 아이템 정보 로드
//        //ItemData itemData = gameSaveData.itemData;
//        //inventory.LoadEquippedItem(itemData.equippedItemID);
//        //inventory.LoadInventoryItems(itemData.inventoryItems);

//        //// 몬스터 정보 로드
//        //MonsterData monsterData = gameSaveData.monsterData;
//        //monsterManager.LoadDeadMonsterCount(monsterData.deadMonsterCount);
//        // 해당 주석 처리 부분은 메인씬을 불러올때 처리 (메인씬에 있는 기존 매니저들이 처리하면 되게끔,,)
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
