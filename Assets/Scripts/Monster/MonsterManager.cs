using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
	public List<Transform> points = new List<Transform>();

	public GameObject rabbitPrefab;
	public GameObject bearPrefab;

	public float createTime = 3.0f;
	public float bearSpawnProbability = 0.3f; // 곰이 스폰될 확률 (30%)
	public int maxMonsterCount = 600; // 최대 몬스터 수

	private int currentMonsterCount = 0; // 현재 스폰된 몬스터 수

	private void Start()
	{
		Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

		foreach (Transform point in spawnPointGroup)
		{
			points.Add(point);
		}

		InvokeRepeating("CreateMonster", 2.0f, createTime);
	}

	void CreateMonster()
	{
		// 현재 몬스터 수가 최대 몬스터 수를 초과하지 않는지 확인
		if (currentMonsterCount >= maxMonsterCount)
		{
			return; // 최대 몬스터 수에 도달했으면 더 이상 몬스터를 생성하지 않음
		}

		int idx = Random.Range(0, points.Count);
		Vector3 spawnPoint = points[idx].position;

		NavMeshHit hit;
		if (NavMesh.SamplePosition(spawnPoint, out hit, 10.0f, NavMesh.AllAreas))
		{
			spawnPoint = hit.position;
		}
		else
		{
			return;
		}

		if (Random.value <= bearSpawnProbability)
		{
			Instantiate(bearPrefab, spawnPoint, Quaternion.identity);
		}
		else
		{
			Instantiate(rabbitPrefab, spawnPoint, Quaternion.identity);
		}

		currentMonsterCount++; // 몬스터 수 증가
	}

	// 몬스터가 파괴될 때 호출되어 현재 몬스터 수를 감소시키는 함수
	public void DecreaseMonsterCount()
	{
		if (currentMonsterCount > 0)
		{
			currentMonsterCount--;
		}
	}
}
