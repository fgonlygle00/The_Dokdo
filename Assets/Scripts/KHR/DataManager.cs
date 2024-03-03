//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DataManager : MonoBehaviour
//{
//    public PlayerManager playerManager;
//    public DayNightCycle dayNightCycle;  // 추가된 속성
//    몬스터 MonsterDataSO를 불러옴(호출)
//     public MonsterDataSO monsterData;

//    private void Awake()
//    {
//        Resources 폴더에서 MonsterDataSO 로드
//        monsterData = Resources.Load<MonsterDataSO>("MonsterData");
//    }


//    세이브 버튼 ui 를 눌렀을때 저장되도록
//    public void SaveGame()
//    {
//        // 기존 게임 저장 코드 
//        PlayerData data = playerManager.GetPlayerData();

//        // 파일 시스템이나 데이터베이스에 저장
//        SavePlayerDataToFile(data);
//        PlayerPrefs.SetFloat("DayNightCycleTime", dayNightCycle.time);
//        PlayerPrefs.Save();
//        Debug.Log("세이브버튼클릭");

//    }



//    시작화면에서 불러오기
//    public void LoadGame()
//    {
//        if (PlayerPrefs.HasKey("DayNightCycleTime"))
//        {
//            dayNightCycle.time = PlayerPrefs.GetFloat("DayNightCycleTime");
//        }
//        기존 게임 로드 코드
//         파일이나 데이터베이스에서 로드
//        PlayerData data = LoadPlayerDataFromFile();

//        // 플레이어 매니저로 복구
//        playerManager.LoadPlayerData(data);

//        Debug.Log("세이브버튼클릭");

//    }

//    public int GetMonsterHealth()
//    {
//        MonsterDataSO에서 체력 반환
//        return monsterData.health;
//    }

//}


