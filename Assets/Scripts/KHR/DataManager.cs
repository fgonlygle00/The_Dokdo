using System.Collections;
using System.Collections.Generic;
using System.IO; //파일이라는 친구를 쓸수 있게 해주는 선언
using System.Runtime.CompilerServices;
using UnityEngine;



[System.Serializable]

public class GameSaveData
{
    public List<float> playerCurValue = new List<float>();
    // 생략할 수 있으나 그 부분은: MonoBehaviour를 상속 받았을때만 생략이 가능
    public ItemData itemData;
    public int selectedItemIndex;
    public List<MonsterData> monsterData;
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerManager playerManager;
    public DayNightCycle dayNightCycle;  // 추가된 속성
    public PlayerConditions playerConditions; // 플레이어 컨트롤러 스크립트 참조

    [SerializeField] private string savePath; // 게임 저장 파일 경로

    // 몬스터 MonsterDataSO를 불러옴(호출)
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

    // 세이브 버튼 ui 를 눌렀을때 저장되도록 
    public void SaveGame()
    {
        GameSaveData gameSaveData = new GameSaveData();

        // 플레이어 정보 저장
        gameSaveData.playerCurValue.Add(playerConditions.health.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stress.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.hunger.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stamina.curValue);
        //// 기존 게임 저장 코드 
        //PlayerData data = playerManager.GetPlayerData();
        Debug.Log(MonsterDataManager.Instance.GetMonsters().Count);
        gameSaveData.monsterData = MonsterDataManager.Instance.GetMonsters();
        gameSaveData.selectedItemIndex = Inventory.instance.selectedItemIndex;
        //// 파일 시스템이나 데이터베이스에 저장
        //SavePlayerDataToFile(data);

        // 게임 저장 데이터를 JSON 형식으로 변환하여 파일에 저장
        string jsonData = JsonUtility.ToJson(gameSaveData);
        File.WriteAllText(savePath, jsonData);

        PlayerPrefs.SetFloat("DayNightCycleTime", dayNightCycle.time);
        //PlayerPrefs.Save();
        Debug.Log("세이브버튼클릭");

    }



    // 시작화면에서 불러오기 
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("DayNightCycleTime"))
        {
            dayNightCycle.time = PlayerPrefs.GetFloat("DayNightCycleTime");
        }
        //기존 게임 로드 코드
        // 파일이나 데이터베이스에서 로드
        //PlayerData data = LoadPlayerDataFromFile();

        //// 플레이어 매니저로 복구
        //playerManager.LoadPlayerData(data);

        Debug.Log("세이브버튼클릭");

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
            Debug.Log("플레이어 컨디션 데이터가 존재하지 않습니다.");
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
            Debug.Log("몬스터 데이터가 존재하지 않습니다.");
        }
    }

}


