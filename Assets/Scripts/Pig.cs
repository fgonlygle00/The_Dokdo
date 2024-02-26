using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


// AI�� ���� �� �ִ� �پ��� ���� ����
public enum AIState
{
	Idle,           // ����
	Wandering,      // ��ȸ
	Attacking,      // ����
	Fleeing         // ����
}
public class Pig : MonoBehaviour
{
	[Header("Stats")]
	public int health;              //	NPC�� ü��
	public float walkSpeed;         // �ȱ� �ӵ�
	public float runSpeed;          // �޸��� �ӵ�

	[Header("AI")]
	private AIState aiState;            // ���� AI ����
	public float detectDistance;        // �÷��̾� ���� �Ÿ� (���� ������ ����)
	public float safeDistance;          // �÷��̾�κ��� �����ϴٰ� ���ֵǴ� �Ÿ� (���� ������ ����)

	[Header("Wandering")]
	public float minWanderDistance;     // �ּ� ��ȸ �Ÿ�
	public float maxWanderDistance;     // �ִ� ��ȸ �Ÿ�
	public float minWanderWaitTime;     // ��ȸ �� �ּ� ��� �ð�
	public float maxWanderWaitTime;     // ��ȸ �� �ִ� ��� �ð�

	[Header("Combat")]
	public int damage;                  // �÷��̾�� �ִ� ������
	public float attackRate;            // ���� ��
	private float lastAttackTime;       // ������ ���� �ð�
	public float attackDistance;        // ���� ���� �Ÿ�

	private float playerDistance;       // NPC�� �÷��̾� ������ �Ÿ�

	public float fieldOfView = 120f;    // �÷��̾ �����ϱ� ���� �þ߰�

	private NavMeshAgent agent;         // NaveMeshAgent ������Ʈ�� ���� ����
 // private Animator animator;		// Animator ������Ʈ�� ���� ����
	private SkinnedMeshRenderer[] meshRenderers;        // �÷��� ȿ���� ���� SinnedMeshRenderer ������Ʈ�� ���� ������

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	 // animator = GetComponentInChildren<Animator>();
		meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	private void Start()
	{
		SetState(AIState.Wandering);        // �ʱ� AI ���¸� ��ȸ�� ����
	}

	private void Update()
	{
		// �÷��̾���� �Ÿ� ��� : ���� �÷��̾ ���� �ּ�ó����.
		// playerDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

		// �̵��� ���� �ִϸ����� ������Ʈ
		//animator.SetBool("Moving", aiState != AIState.Idle);

		// ���� AI ���¿� ���� �ൿ ó��
		switch (aiState)
		{
				case AIState.Idle: PassiveUpdate(); break;
			case AIState.Wandering: PassiveUpdate(); break;
				//	case AIState.Attacking: AttackingUpdate(); break;
				//	case AIState.Fleeing: FleeingUpdate(); break;
		}
	}

	// ���� ���¿����� ������Ʈ ����
	//private void FleeingUpdate()
	//{
	//	if (agent.remainingDistance < 0.1f)
	//	{
	// agent.SetDestination(GetFleeLocation());			// ���� �������� ��������� ���ο� ���� ��ġ ����
	//	}
	//	else
	//	{
	//		SetState(AIState.Wandering);						// ���� ���°� �ƴϸ� ��ȸ�� ��ȯ
	//	}
	//}

	// ���� ���¿����� ������Ʈ ����
	//private void AttackingUpdate()
	//{
	//	if(playerDistance > attackDistance || !IsPlaterInFireldOfView())
	//	{
	//		agent.isStopped = false;
	//		NavMeshPath path = new NavMeshPath();
	//		if (agent.CalulatePath(PlayerController.instance.transform.position, path))
	//		{
	//			agent.SetDestination(PlayerController.instance.transform.position);
	//		}
	//		else
	//		{
	//			SetStateGraph(AIState.Fleeing);
	//		}
	//	}
	//	else
	//	{
	//		agent.isStopped = true;
	//		if (Time.time - lastAttackTime > attackRate)
	//		{
	//			lastAttackTime = Time.time;
	//			PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(damage);
	//			animator.speed = 1;
	//			animator.SetTrigger("Attack");
	//		}
	//	}
	//}

	// ���� �� ��ȸ ���¿����� ������Ʈ ����
	private void PassiveUpdate()
	{
		if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
		{
			// ��ȸ �������� �����ϸ� ���� ���·� ��ȯ
			SetState(AIState.Idle);
			// ���� ��ȸ ����
			Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
		}

		// �÷��̾ �����ϸ� ���� ���·� ��ȯ
		//if (playerDistance < detectDistance)
		//{
		//	SetState(AIState.Attacking);
		//}
	}

	// �÷��̾ �þ߰� ���� �ִ��� Ȯ��
	//bool IsPlaterInFireldOfView()
	//{
	//	Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;
	//	float angle = Vector3.Angle(transform.forward, directionToPlayer);
	//	return angle < fieldOfView * 0.5f;
	//}

	// AI ���¸� �����ϰ� ���� �Ӽ� ������Ʈ
	private void SetState(AIState newState)
	{
		aiState = newState;
		switch (aiState)
		{
			case AIState.Idle:
				{
					agent.speed = walkSpeed;
					agent.isStopped = true;
				}
				break;
			case AIState.Wandering:
				{
					agent.speed = walkSpeed;
					agent.isStopped = false;
				}
				break;

			case AIState.Attacking:
				{
					agent.speed = runSpeed;
					agent.isStopped = false;
				}
				break;
			case AIState.Fleeing:
				{
					agent.speed = runSpeed;
					agent.isStopped = false;
				}
				break;
		}

		// animator.speed = agent.speed / walkSpeed;		// �̵� �ӵ��� ���� �ִϸ��̼� �ӵ� ����
	}

	// ���ο� ��ġ�� ��ȸ ����
	void WanderToNewLocation()
	{
		if (aiState != AIState.Idle)
		{
			return;
		}
		SetState(AIState.Wandering);
		agent.SetDestination(GetWanderLocation());
	}

	// ���ο� ��ȸ ��ġ ���
	Vector3 GetWanderLocation()
	{
		NavMeshHit hit;

		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

		int i = 0;
		while (Vector3.Distance(transform.position, hit.position) < detectDistance)
		{
			NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
			i++;
			if (i == 30)
				break;
		}

		return hit.position;
	}

	// ���� ��ġ ���
	//Vector3 GetFleeLocation()
	//{
	//	NavMeshHit hit;

	//	NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);

	//	int i = 0;
	//	while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
	//	{

	//		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
	//		i++;
	//		if (i == 30)
	//			break;
	//	}

	//	return hit.position;
	//}


	// ���� ������ ���� ������ ���� ���
	//float GetDestinationAngle(Vector3 targetPos)
	//{
	//	return Vector3.Angle(transform.position - PlayerController.instance.transform.position, transform.position + targetPos);
	//}

	// NPC�� ���� �ջ� ó��
	public void TakePhysicalDamage(int damageAmount)
	{
		health -= damageAmount;
		if (health <= 0)
			Die();

		// �ջ� �޾��� ���� �÷��� ȿ��
		StartCoroutine(DamageFlash());
	}

	// NPC ��� ó��
	void Die()
	{
		//for (int x = 0; x < dropOnDeath.Length; x++)
		//{
		//	Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
		//}

		// NPC ������Ʈ �ı�
		Destroy(gameObject);
	}

	// �ջ� �÷��� ȿ���� ���� �ڷ�ƾ
	IEnumerator DamageFlash()
	{
		for (int x = 0; x < meshRenderers.Length; x++)
			meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

		yield return new WaitForSeconds(0.1f);
		for (int x = 0; x < meshRenderers.Length; x++)
			meshRenderers[x].material.color = Color.white;
	}
}
