using System.Collections;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	public GameObject rabbitPrefab;
	public GameObject bearPrefab;

	public int maxRabbits = 300; // ���ϴ� �䳢�� �ִ� ��
	public int maxBears = 100; // ���ϴ� ������ �ִ� ��

	public float rabbitSpawnInterval = 3f; // �䳢 ��ȯ ����
	public float bearSpawnInterval = 5f; // ���� ��ȯ ����

	private int currentRabbitCount = 0;
	private int currentBearCount = 0;

	public Vector3 minimumBoundary; // ��ġ�� �ּ� ���
	public Vector3 maximumBoundary; // ��ġ�� �ִ� ���

	void Start()
	{
		StartCoroutine(SpawnRabbitsCoroutine());
		StartCoroutine(SpawnBearsCoroutine());
	}

	IEnumerator SpawnRabbitsCoroutine()
	{
		while (currentRabbitCount < maxRabbits)
		{
			SpawnAnimal(rabbitPrefab);
			yield return new WaitForSeconds(rabbitSpawnInterval);
		}
	}

	IEnumerator SpawnBearsCoroutine()
	{
		while (currentBearCount < maxBears)
		{
			SpawnAnimal(bearPrefab);
			yield return new WaitForSeconds(bearSpawnInterval);
		}
	}

	private void SpawnAnimal(GameObject animalPrefab)
	{
		Vector3 randomPosition = GetRandomPosition();
		Instantiate(animalPrefab, randomPosition, Quaternion.identity);
		if (animalPrefab == rabbitPrefab) currentRabbitCount++;
		else if (animalPrefab == bearPrefab) currentBearCount++;
	}

	// ���� ��� ������ ���� ��ġ�� ��ȯ�մϴ�.
	private Vector3 GetRandomPosition()
	{
		float x = Random.Range(minimumBoundary.x, maximumBoundary.x);
		float y = Random.Range(minimumBoundary.y, maximumBoundary.y);
		float z = Random.Range(minimumBoundary.z, maximumBoundary.z);
		return new Vector3(x, y, z);
	}
}
