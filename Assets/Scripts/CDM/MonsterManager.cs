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

	//// ������Ʈ ���� ���� �ð� ���� (��: 5��)
	//public float spawnInterval = 5f;

	//void Start()
	//{
	//	// �ڷ�ƾ ����
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

	//		// 10��(600��) �� �ı�
	//		Destroy(createdObject, 600f);

	//		// ���� ������Ʈ �������� ���
	//		yield return new WaitForSeconds(spawnInterval);
	//	}
	//}
}
