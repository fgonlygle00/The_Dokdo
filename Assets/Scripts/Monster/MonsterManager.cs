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
	public float bearSpawnProbability = 0.3f; // ���� ������ Ȯ�� (30%)
	public int maxMonsterCount = 600; // �ִ� ���� ��

	private int currentMonsterCount = 0; // ���� ������ ���� ��

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
		// ���� ���� ���� �ִ� ���� ���� �ʰ����� �ʴ��� Ȯ��
		if (currentMonsterCount >= maxMonsterCount)
		{
			return; // �ִ� ���� ���� ���������� �� �̻� ���͸� �������� ����
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

		currentMonsterCount++; // ���� �� ����
	}

	// ���Ͱ� �ı��� �� ȣ��Ǿ� ���� ���� ���� ���ҽ�Ű�� �Լ�
	public void DecreaseMonsterCount()
	{
		if (currentMonsterCount > 0)
		{
			currentMonsterCount--;
		}
	}
}
