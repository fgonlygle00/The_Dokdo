using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public PlayerManager playerManager;

    // 몬스터 MonsterDataSO를 불러옴(호출)
    public MonsterDataSO monsterData;

    private void Awake()
    {
        // Resources 폴더에서 MonsterDataSO 로드
        monsterData = Resources.Load<MonsterDataSO>("MonsterData");
    }

    public void SaveGame()
    {
        //// 기존 게임 저장 코드 
        //PlayerData data = playerManager.GetPlayerData();

        //// 파일 시스템이나 데이터베이스에 저장
        //SavePlayerDataToFile(data);
    }

    public void LoadGame()
    {
        //기존 게임 로드 코드
        // 파일이나 데이터베이스에서 로드
        //PlayerData data = LoadPlayerDataFromFile();

        //// 플레이어 매니저로 복구
        //playerManager.LoadPlayerData(data);
    }

    public int GetMonsterHealth()
    {
        // MonsterDataSO에서 체력 반환
        return monsterData.health;
    }

}


