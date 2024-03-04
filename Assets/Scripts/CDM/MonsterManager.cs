using System.Collections;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	public Transform[] points;

	private void Start()
	{
		// SpawnPointGroup 게임 오브젝트의 Transform 컴포넌트 추출
		Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

		// SpawnPointGroup 하위에 있는 모든 차일드 게임오브젝트의 Transform 컴포넌트 추출
		points = spawnPointGroup?.GetComponentsInChildren<Transform>();
	}
}
