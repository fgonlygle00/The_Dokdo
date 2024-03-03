using System.Collections;
using System.Collections.Generic;
using System.IO; //�����̶�� ģ���� ���� �ְ� ���ִ� ����
using System.Runtime.CompilerServices;
using UnityEngine;



[System.Serializable]

public class GameSaveData
{
    public List<float>  playerCurValue = new List<float>(); 
    // ������ �� ������ �� �κ���: MonoBehaviour�� ��� �޾������� ������ ����
    public ItemData itemData;

    public MonsterData monsterData;
}
public class DataManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public DayNightCycle dayNightCycle;  // �߰��� �Ӽ�
    public PlayerConditions playerConditions; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ ����
 
    [SerializeField] private string savePath; // ���� ���� ���� ���

    // ���� MonsterDataSO�� �ҷ���(ȣ��)
    //public MonsterDataSO monsterData;

   private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }


        // ���̺� ��ư ui �� �������� ����ǵ��� 
    public void SaveGame()
    {
        GameSaveData gameSaveData = new GameSaveData();

        // �÷��̾� ���� ����
        gameSaveData.playerCurValue.Add(playerConditions.health.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stress.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.hunger.curValue);
        gameSaveData.playerCurValue.Add(playerConditions.stamina.curValue);
        //// ���� ���� ���� �ڵ� 
        //PlayerData data = playerManager.GetPlayerData();
        gameSaveData.monsterData = MonsterManager.InS
        //// ���� �ý����̳� �����ͺ��̽��� ����
        //SavePlayerDataToFile(data);

        // ���� ���� �����͸� JSON �������� ��ȯ�Ͽ� ���Ͽ� ����
        string jsonData = JsonUtility.ToJson(gameSaveData);
        File.WriteAllText(savePath, jsonData);

        PlayerPrefs.SetFloat("DayNightCycleTime", dayNightCycle.time);
        //PlayerPrefs.Save();
        Debug.Log("���̺��ưŬ��");

    }



        // ����ȭ�鿡�� �ҷ����� 
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("DayNightCycleTime"))
        {
            dayNightCycle.time = PlayerPrefs.GetFloat("DayNightCycleTime");
        }
        //���� ���� �ε� �ڵ�
        // �����̳� �����ͺ��̽����� �ε�
        //PlayerData data = LoadPlayerDataFromFile();

        //// �÷��̾� �Ŵ����� ����
        //playerManager.LoadPlayerData(data);

        Debug.Log("���̺��ưŬ��");

    }
    //public int GetMonsterHealth()
    //{
    //    // MonsterDataSO���� ü�� ��ȯ
    //    return monsterData.health;
    //}



}


