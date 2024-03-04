using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
	public static MonsterDataManager Instance;
	public List<MonsterData> monsters = new List<MonsterData>();

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}
    private void Start()
    {
        //DataManager.instance
    }

    public void RegisterMonster(MonsterData monsterData)
	{
		monsters.Add(monsterData);
	}

	public void UnregisterMonster(MonsterData monsterData)
	{
		monsters.Remove(monsterData);
	}

	public List<MonsterData> GetMonsters()
	{
		return monsters;
	}
}