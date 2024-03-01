using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public PlayerManager playerManager;

    // ���� MonsterDataSO�� �ҷ���(ȣ��)
    public MonsterDataSO monsterData;

    private void Awake()
    {
        // Resources �������� MonsterDataSO �ε�
        monsterData = Resources.Load<MonsterDataSO>("MonsterData");
    }

    public void SaveGame()
    {
        //// ���� ���� ���� �ڵ� 
        //PlayerData data = playerManager.GetPlayerData();

        //// ���� �ý����̳� �����ͺ��̽��� ����
        //SavePlayerDataToFile(data);
    }

    public void LoadGame()
    {
        //���� ���� �ε� �ڵ�
        // �����̳� �����ͺ��̽����� �ε�
        //PlayerData data = LoadPlayerDataFromFile();

        //// �÷��̾� �Ŵ����� ����
        //playerManager.LoadPlayerData(data);
    }

    public int GetMonsterHealth()
    {
        // MonsterDataSO���� ü�� ��ȯ
        return monsterData.health;
    }

}


