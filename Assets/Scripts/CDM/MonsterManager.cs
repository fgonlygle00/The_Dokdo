using System.Collections;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	public GameObject rabbitPrefab;
	public GameObject bearPrefab;

	public int maxRabbits = 300; // 원하는 토끼의 최대 수
	public int maxBears = 100; // 원하는 베어의 최대 수

	public float rabbitSpawnInterval = 3f; // 토끼 소환 간격
	public float bearSpawnInterval = 5f; // 베어 소환 간격

	private int currentRabbitCount = 0;
	private int currentBearCount = 0;

	public Vector3 minimumBoundary; // 배치할 최소 경계
	public Vector3 maximumBoundary; // 배치할 최대 경계

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

	// 맵의 경계 내에서 랜덤 위치를 반환합니다.
	private Vector3 GetRandomPosition()
	{
		float x = Random.Range(minimumBoundary.x, maximumBoundary.x);
		float y = Random.Range(minimumBoundary.y, maximumBoundary.y);
		float z = Random.Range(minimumBoundary.z, maximumBoundary.z);
		return new Vector3(x, y, z);
	}
}
