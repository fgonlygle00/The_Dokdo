using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;
    public float dayNightCycleTime;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // DontDestroyOnLoad(gameObject); 두개 있을 경우에 파괴되어야 함 = 게임매니저가 하나만 존재하도록 하는 거
        {
            if (instance != this)
            {
                Destroy(gameObject);

            }
        }

    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("DayNightCycleTime"))
        {
            dayNightCycleTime = PlayerPrefs.GetFloat("DayNightCycleTime");
        }
        //기존 게임 로드 코드
        // 파일이나 데이터베이스에서 로드
        //PlayerData data = LoadPlayerDataFromFile();

        //// 플레이어 매니저로 복구
        //playerManager.LoadPlayerData(data);

        Debug.Log("세이브버튼클릭");

    }
}
