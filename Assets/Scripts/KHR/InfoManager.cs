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
        else // DontDestroyOnLoad(gameObject); �ΰ� ���� ��쿡 �ı��Ǿ�� �� = ���ӸŴ����� �ϳ��� �����ϵ��� �ϴ� ��
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
        //���� ���� �ε� �ڵ�
        // �����̳� �����ͺ��̽����� �ε�
        //PlayerData data = LoadPlayerDataFromFile();

        //// �÷��̾� �Ŵ����� ����
        //playerManager.LoadPlayerData(data);

        Debug.Log("���̺��ưŬ��");

    }
}
