using UnityEngine;

[System.Serializable]
public class MonsterData
{
	public float positionX, positionY, positionZ;
	public float rotationX, rotationY, rotationZ, rotationW;
	public float health;

	public MonsterData(Vector3 position, Quaternion rotation, float health)
	{
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
