using System.Collections;
using System.Collections.Generic;
using System.IO; //파일이라는 친구를 쓸수 있게 해주는 선언
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]

public class GameSaveData
{
    public List<float> playerCurValue = new List<float>();
    // new List<float>(); 생략할 수 있으나 그 부분은: MonoBehaviour를 상속 받았을때만 생략이 가능
    public ItemSlot[] slots;
    public int selectedItemIndex;
    public List<MonsterData> monsterData;
    public Vector3 playerPositions;
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerManager playerManager;
    public DayNightCycle dayNightCycle;  // 추가된 속성
    public PlayerConditions playerConditions; // 플레이어 컨트롤러 스크립트 참조(플레이어 오브젝트에 있음)

    [SerializeField] private string savePath; // 게임 저장 파일 경로

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
        playerConditions = GameManager.instance.PlayerObject.GetComponent<PlayerConditions>();
    }

    // 세이브 버튼 ui 를 눌렀을때 저장되  도록 
    public void SaveGame()
    {
        GameSaveData gameSaveData = new GameSaveData();

        // 플레이어 정보 저장
        gameSaveData.playerCurValue.Add(playerConditions.health.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stress.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.hunger.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stamina.curValue);

        Debug.Log(MonsterDataManager.Instance.GetMonsters().Count);
        gameSaveData.monsterData = MonsterDataManager.Instance.GetMonsters();
        gameSaveData.selectedItemIndex = Inventory.instance.selectedItemIndex;
        gameSaveData.playerPositions = playerConditions.transform.position;

        // 아이템 정보 저장
        gameSaveData.slots = Inventory.instance.slots;

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

        Debug.Log("세이브버튼클릭");

    }

    public void SetPlayerData() //플레이어에게 저장된 로드를 덮어씌우기 (스타트시점에 호출)
    {
        Debug.Log(InfoManager.instance.IsLoad);
        Scene scene = SceneManager.GetActiveScene();
        if  ((scene.name == "DungeonScene_1Zone" || scene.name == "DungeonScene_2Zone" || scene.name == "DungeonScene_3Zone") && InfoManager.instance.playerCurValue != null)
        //불러오기에서 시작했을때만, 데이터를 덮어씌우기를 하기 위해 예외처리
        {
            if (playerConditions == null)
            {
                playerConditions = GameManager.instance.PlayerObject.GetComponent<PlayerConditions>();
            }
            
            playerConditions.health.curValue = InfoManager.instance.playerCurValue[0];
            playerConditions.stress.curValue = InfoManager.instance.playerCurValue[1];
            playerConditions.hunger.curValue = InfoManager.instance.playerCurValue[2];
            playerConditions.stamina.curValue = InfoManager.instance.playerCurValue[3];
            //플레이어가 씬 이동됬을떄 파괴가 되지 않도록,, (Dont디스트로이)
            //우리가 던전에 들어갔을때 임의로 세이브, 자동세이브,,
            
        }
        

        else if (InfoManager.instance.IsLoad && InfoManager.instance.playerCurValue != null)
        //불러오기에서 시작했을때만, 데이터를 덮어씌우기를 하기 위해 예외처리
        {
            if (playerConditions == null)
            {
                playerConditions = GameManager.instance.PlayerObject.GetComponent<PlayerConditions>();
            }
            playerConditions.health.curValue = InfoManager.instance.playerCurValue[0];
            playerConditions.stress.curValue = InfoManager.instance.playerCurValue[1];
            playerConditions.hunger.curValue = InfoManager.instance.playerCurValue[2];
            playerConditions.stamina.curValue = InfoManager.instance.playerCurValue[3];
            //

            // 현재 활성화 된 씬을 넣어줌
            if (scene.name == "DungeonScene_1Zone" || scene.name == "DungeonScene_2Zone" || scene.name == "DungeonScene_3Zone")
            {
                return;
            }
            Debug.Log(InfoManager.instance.playerPositions);

            GameManager.instance.PlayerObject.transform.position = InfoManager.instance.playerPositions;
            // 로드했을때 플레이어의 위치값 초기화 
            //불러온 아이템 데이터를 InfoManager에 저장


        }
        else
        {
            Debug.Log("플레이어 컨디션 데이터가 존재하지 않습니다.");
        }
    }

    public void SetItemData()
    {
        if (InfoManager.instance.IsLoad && InfoManager.instance.slots != null)
        {
            //불러오기에서 시작했을때만, 데이터를 덮어씌우기를 하기 위해 예외처리

            string itemName = "exampleItem"; // 불러올 아이템 이름을 지정
            ItemData itemData = LoadItemData(itemName);
            ItemSlotUI[] itemSlotUI = GameManager.instance.PlayerObject.GetComponent<Inventory>().uiSlots;

            GameManager.instance.PlayerObject.GetComponent<Inventory>().slots = InfoManager.instance.slots;
            // InfoManager에 아이템 데이터를 저장

        }
    }

    public void SetMonsterData()//몬스터에게 저장된 로드를 덮어씌우기 (스타트시점에 호출)
    {
        if (InfoManager.instance.IsLoad && InfoManager.instance.monsterData != null)
        //불러오기에서 시작했을때만, 데이터를 덮어씌우기를 하기 위해 예외처리
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


