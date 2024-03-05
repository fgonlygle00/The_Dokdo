using UnityEngine;

[System.Serializable]
public class MonsterData
{
	public int uniqueID; // ���� ���� �ĺ���
	public string monsterType; // ���� Ÿ��
	public float positionX, positionY, positionZ;
	public float rotationX, rotationY, rotationZ, rotationW;
	public float health;

	public MonsterData(int uniqueID, string monsterType, Vector3 position, Quaternion rotation, float health)
	{
		this.uniqueID = uniqueID;
		this.monsterType = monsterType;

		this.positionX = position.x;
		this.positionY = position.y;
		this.positionZ = position.z;

		this.rotationX = rotation.x;
		this.rotationY = rotation.y;
		this.rotationZ = rotation.z;
		this.rotationW = rotation.w;

		this.health = health;
	}
}
