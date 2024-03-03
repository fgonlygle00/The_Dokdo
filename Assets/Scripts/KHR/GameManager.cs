using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class GameManager : MonoBehaviour
{
    public event Action GameOverEvent; // 게임매니저에서만 접근 가능하도록 만듦(이벤트를 이용해서, 함수를 이용해서 접근할 수 있도록 제작)
    public static GameManager instance;
    public GameObject SaveButtonObject; // 최종적으로는 지우기
    public GameObject PlayerObject; // 언제든지 게임메니저 인스턴스에 접근을 하면, 플레이어 오브젝트에 접근할 수 있도록 만듦
    public PlayerManager playerManager; 
    public GameObject gameOverUI;

    private PlayerData playerData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // DontDestroyOnLoad(gameObject); 가 게임매니저가 두개 있을 경우에 파괴되어야 함 = 게임매니저가 하나만 존재하도록 하는 거
        {
            if (instance != this)
            {
                Destroy(gameObject);

            }
        }

    }
    private void Start()
    {
        // 초기 플레이어 데이터 설정
        playerData = new PlayerData(100, 0, 100, 100);
        //playerManager.SetPlayerData(playerData);
        GameOverEvent += GameOver; 
        // 게임오버이벤트에 게임오버라는 함수를 구독! (게임오버가 되었을때 실행되야하는 것들을 게임오버이벤트에 다 구독시켜주세요 = 대입시켜주세요)
    }

    private void Update()
    {
        // 플레이어 데이터 갱신
        //playerData = playerManager.GetPlayerData();

        // 게임 플로우 제어
        ControlGameFlow();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //esc 버튼이 눌렸을때 실행
            
            SaveButtonObject.SetActive(true);
            PlayerObject.GetComponent<PlayerController>().ToggleCursor(true);

            Debug.Log("ESC 버튼이 눌렸습니다!");

        }
    }



    public void CallGameOverEvent()
    {
        GameOverEvent?.Invoke(); // ? = null 이면 실행 x , 이벤트에 아무것도 등록되어있지 않으면 실행x
    }

    private void ControlGameFlow()
    {
        // 체력 0 이하면 게임오버
        if (playerData.playerHealth <= 0)
            GameOver();
    }

    private void GameOver()
    {
        GameManager.instance.GameOver();
        // 게임오버 UI 활성화
        gameOverUI.SetActive(true);

        // 씬 전환으로 해보기 (게임오버가 되었을때)

        // 사운드 재생 등

        // 플레이어 친구에게 플레이어의 hp <=0 일 경우에, 해당 CallGameOverEvent 함수 실행시킬 수 있도록 넣어주세요 (디렉 넣기)
        
    }
}

