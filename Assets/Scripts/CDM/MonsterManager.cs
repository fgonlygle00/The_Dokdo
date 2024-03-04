using System.Collections;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	public Transform[] points;

	private void Start()
	{
		// SpawnPointGroup ���� ������Ʈ�� Transform ������Ʈ ����
		Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

		// SpawnPointGroup ������ �ִ� ��� ���ϵ� ���ӿ�����Ʈ�� Transform ������Ʈ ����
		points = spawnPointGroup?.GetComponentsInChildren<Transform>();
	}
}
