//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DataManager : MonoBehaviour
//{
//    public PlayerManager playerManager;
//    public DayNightCycle dayNightCycle;  // �߰��� �Ӽ�
//    ���� MonsterDataSO�� �ҷ���(ȣ��)
//     public MonsterDataSO monsterData;

//    private void Awake()
//    {
//        Resources �������� MonsterDataSO �ε�
//        monsterData = Resources.Load<MonsterDataSO>("MonsterData");
//    }


//    ���̺� ��ư ui �� �������� ����ǵ���
//    public void SaveGame()
//    {
//        // ���� ���� ���� �ڵ� 
//        PlayerData data = playerManager.GetPlayerData();

//        // ���� �ý����̳� �����ͺ��̽��� ����
//        SavePlayerDataToFile(data);
//        PlayerPrefs.SetFloat("DayNightCycleTime", dayNightCycle.time);
//        PlayerPrefs.Save();
//        Debug.Log("���̺��ưŬ��");

//    }



//    ����ȭ�鿡�� �ҷ�����
//    public void LoadGame()
//    {
//        if (PlayerPrefs.HasKey("DayNightCycleTime"))
//        {
//            dayNightCycle.time = PlayerPrefs.GetFloat("DayNightCycleTime");
//        }
//        ���� ���� �ε� �ڵ�
//         �����̳� �����ͺ��̽����� �ε�
//        PlayerData data = LoadPlayerDataFromFile();

//        // �÷��̾� �Ŵ����� ����
//        playerManager.LoadPlayerData(data);

//        Debug.Log("���̺��ưŬ��");

//    }

//    public int GetMonsterHealth()
//    {
//        MonsterDataSO���� ü�� ��ȯ
//        return monsterData.health;
//    }

//}


