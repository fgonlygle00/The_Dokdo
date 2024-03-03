using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterManager : MonoBehaviour
{
	//public float xMin = 1597f;
	//public float xMax = 1942f;
	//public float zMin = 432f;
	//public float zMax = 727f;
	//public GameObject objectPrefab;
	//public int objectCount = 10;

	//// 오브젝트 생성 간의 시간 간격 (예: 5초)
	//public float spawnInterval = 5f;

	//void Start()
	//{
	//	// 코루틴 시작
	//	StartCoroutine(GenerateRandomPositionsCoroutine());
	//}

	//IEnumerator GenerateRandomPositionsCoroutine()
	//{
	//	for (int i = 0; i < objectCount; i++)
	//	{
	//		float x = Random.Range(xMin, xMax);
	//		float z = Random.Range(zMin, zMax);
	//		Vector3 randomPosition = new Vector3(x, 300, z);

	//		GameObject createdObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity);

	//		// 10분(600초) 후 파괴
	//		Destroy(createdObject, 600f);

	//		// 다음 오브젝트 생성까지 대기
	//		yield return new WaitForSeconds(spawnInterval);
	//	}
	//}
}
