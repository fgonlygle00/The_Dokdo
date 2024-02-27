using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/Create New Monster Data")]
public class MonsterDataSO : ScriptableObject
{
	public int health;
	public float walkSpeed;
	public float runSpeed;
	public int damage;
	public float attackRate;
	public float attackDistance;
	public float detectDistance;
	public float safeDistance;
	public float fieldOfView = 120f;
}
