using System.Collections;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	public GameObject rabbitPrefab;
	public GameObject bearPrefab;

	public int maxRabbits = 50;
	public int maxBears = 20;

	public float rabbitXMin = 1597f;
	public float rabbitXMax = 1942f;
	public float rabbitZMin = 432f;
	public float rabbitZMax = 727f;

	public float bearXMin = 1950f;
	public float bearXMax = 2100f;
	public float bearZMin = 977f;
	public float bearZMax = 1390f;

	public float rabbitSpawnInterval = 3f;
	public float bearSpawnInterval = 5f;

	private int currentRabbitCount = 0;
	private int currentBearCount = 0;

	void Start()
	{
		StartCoroutine(SpawnRabbitsCoroutine());
		StartCoroutine(SpawnBearsCoroutine());
	}

	IEnumerator SpawnRabbitsCoroutine()
	{
		while (currentRabbitCount < maxRabbits)
		{
			Vector3 spawnPosition = GenerateRabbitSpawnPosition();
			SpawnAnimal(rabbitPrefab, spawnPosition);
			currentRabbitCount++;
			yield return new WaitForSeconds(rabbitSpawnInterval);
		}
	}

	IEnumerator SpawnBearsCoroutine()
	{
		while (currentBearCount < maxBears)
		{
			Vector3 spawnPosition = GenerateBearSpawnPosition();
			SpawnAnimal(bearPrefab, spawnPosition);
			currentBearCount++;
			yield return new WaitForSeconds(bearSpawnInterval);
		}
	}

	private void SpawnAnimal(GameObject animalPrefab, Vector3 spawnPosition)
	{
		Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
	}

	private Vector3 GenerateRabbitSpawnPosition()
	{
		float x = Random.Range(rabbitXMin, rabbitXMax);
		float z = Random.Range(rabbitZMin, rabbitZMax);
		float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z)) + Terrain.activeTerrain.GetPosition().y;
		return new Vector3(x, y, z);
	}

	private Vector3 GenerateBearSpawnPosition()
	{
		float x = Random.Range(bearXMin, bearXMax);
		float z = Random.Range(bearZMin, bearZMax);
		float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z)) + Terrain.activeTerrain.GetPosition().y;
		return new Vector3(x, y, z);
	}
}
