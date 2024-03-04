using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
    public Vector3 playerPositions;
    public bool IsLoad = false;
    public ItemSlot[] slots;
    public static InfoManager instance;
    public List<float> playerCurValue = new List<float>();
    public List<MonsterData> monsterData = new List<MonsterData>();
    [SerializeField] private string savePath; // 게임 저장 파일 경로

    private void Awake() // 싱글톤화
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    // 게임 저장 시 호출되는 메서드

    // 게임 불러오기 시 호출되는 메서드
    public void LoadGame()
    {
        SceneManager.LoadScene("MainScene");
        if (!File.Exists(savePath))
        {
            // 저장된 게임 파일이 없을 경우 로드할 수 없음을 알림
            Debug.Log("저장된 게임이 없습니다.");
            return;
        }
        IsLoad = true;

        // 파일에서 저장된 게임 데이터를 읽어옴
        string jsonData = File.ReadAllText(savePath);
        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);
        playerPositions = gameSaveData.playerPositions;

        // 플레이어 정보 로드
        playerCurValue = gameSaveData.playerCurValue;
        //// 아이템 정보 로드
        slots = gameSaveData.slots;


        // 몬스터 정보 로드
        monsterData = gameSaveData.monsterData;
    }
}





