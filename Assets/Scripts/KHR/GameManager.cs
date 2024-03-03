using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class GameManager : MonoBehaviour
{
    public event Action GameOverEvent; // ���ӸŴ��������� ���� �����ϵ��� ����(�̺�Ʈ�� �̿��ؼ�, �Լ��� �̿��ؼ� ������ �� �ֵ��� ����)
    public static GameManager instance;
    public GameObject SaveButtonObject; // ���������δ� �����
    public GameObject PlayerObject; // �������� ���Ӹ޴��� �ν��Ͻ��� ������ �ϸ�, �÷��̾� ������Ʈ�� ������ �� �ֵ��� ����
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
        else // DontDestroyOnLoad(gameObject); �� ���ӸŴ����� �ΰ� ���� ��쿡 �ı��Ǿ�� �� = ���ӸŴ����� �ϳ��� �����ϵ��� �ϴ� ��
        {
            if (instance != this)
            {
                Destroy(gameObject);

            }
        }

    }
    private void Start()
    {
        // �ʱ� �÷��̾� ������ ����
        playerData = new PlayerData(100, 0, 100, 100);
        //playerManager.SetPlayerData(playerData);
        GameOverEvent += GameOver; 
        // ���ӿ����̺�Ʈ�� ���ӿ������ �Լ��� ����! (���ӿ����� �Ǿ����� ����Ǿ��ϴ� �͵��� ���ӿ����̺�Ʈ�� �� ���������ּ��� = ���Խ����ּ���)
    }

    private void Update()
    {
        // �÷��̾� ������ ����
        //playerData = playerManager.GetPlayerData();

        // ���� �÷ο� ����
        ControlGameFlow();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //esc ��ư�� �������� ����
            
            SaveButtonObject.SetActive(true);
            PlayerObject.GetComponent<PlayerController>().ToggleCursor(true);

            Debug.Log("ESC ��ư�� ���Ƚ��ϴ�!");

        }
    }



    public void CallGameOverEvent()
    {
        GameOverEvent?.Invoke(); // ? = null �̸� ���� x , �̺�Ʈ�� �ƹ��͵� ��ϵǾ����� ������ ����x
    }

    private void ControlGameFlow()
    {
        // ü�� 0 ���ϸ� ���ӿ���
        if (playerData.playerHealth <= 0)
            GameOver();
    }

    private void GameOver()
    {
        GameManager.instance.GameOver();
        // ���ӿ��� UI Ȱ��ȭ
        gameOverUI.SetActive(true);

        // �� ��ȯ���� �غ��� (���ӿ����� �Ǿ�����)

        // ���� ��� ��

        // �÷��̾� ģ������ �÷��̾��� hp <=0 �� ��쿡, �ش� CallGameOverEvent �Լ� �����ų �� �ֵ��� �־��ּ��� (�� �ֱ�)
        
    }
}

