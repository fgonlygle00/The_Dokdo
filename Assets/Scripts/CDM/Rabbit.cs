using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit : MonoBehaviour
{
	[Header("Stats")]
	public float walkSpeed;
	public float runSpeed;
	public float health;
	public ItemData[] dropOnDeath;

	[Header("AI")]

	public AIState aiState;
	public float detectDistance;
	public float safeDistance;            

	[Header("Wandering")]
	public float minWanderDistance;     // �ּ� ��ȸ �Ÿ�
	public float maxWanderDistance;     // �ִ� ��ȸ �Ÿ�
	public float minWanderWaitTime;     // ��ȸ �� �ּ� ��� �ð�
	public float maxWanderWaitTime;     // ��ȸ �� �ִ� ��� �ð�

	private float playerDistance;       // NPC�� �÷��̾� ������ �Ÿ�

	public float fieldOfView = 120f;

	private NavMeshAgent agent;         // NaveMeshAgent ������Ʈ�� ���� ����
    private Animator animator;			// Animator ������Ʈ�� ���� ����
	private SkinnedMeshRenderer[] meshRenderers;        // �÷��� ȿ���� ���� SinnedMeshRenderer ������Ʈ�� ���� ������

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		// audioSource = GetComponent<AudioSource>();			// AudioSource ������Ʈ ��������
		meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	private void Start()
	{
		SetState(AIState.Wandering);        // �ʱ� AI ���¸� ��ȸ�� ����
	}

	private void Update()
	{
		// �÷��̾���� �Ÿ� ��� 
		playerDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

		if (playerDistance < safeDistance) 
		{
			SetState(AIState.Fleeing);
		}
		else if (playerDistance >= safeDistance && aiState == AIState.Fleeing) // ���� ���¿��� ���� �Ÿ� �̻��� �Ǹ� ��ȸ ���·� ��ȯ
		{
			SetState(AIState.Wandering);
		}
		// �̵��� ���� �ִϸ����� ������Ʈ
		//animator.SetBool("Moving", aiState != AIState.Idle);

		// ���� AI ���¿� ���� �ൿ ó��

		switch (aiState)
		{
			case AIState.Idle: 
			case AIState.Wandering: PassiveUpdate(); break;
			case AIState.Fleeing: FleeingUpdate(); break;
		}
	}

	// ���� ���¿����� ������Ʈ ����
	private void FleeingUpdate()
	{
		if (!agent.pathPending && agent.remainingDistance < 0.1f)
		{
			agent.SetDestination(GetFleeLocation()); // ���ο� ���� ��ġ ����
		}
	}

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
	}

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

			case AIState.Fleeing:
				{
					agent.speed = runSpeed;
					agent.isStopped = false;
				}
				break;
		}

		animator.speed = agent.speed / walkSpeed;		// �̵� �ӵ��� ���� �ִϸ��̼� �ӵ� ����
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

		// ��ȸ ���� ���
		// audioSource.PlayOneShot(wanderSound);
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
	Vector3 GetFleeLocation()
	{
		NavMeshHit hit;

		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);

		int i = 0;
		while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
		{

			NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
			i++;
			if (i == 30)
				break;
		}

		return hit.position;
	}


	// ���� ������ ���� ������ ���� ���
	float GetDestinationAngle(Vector3 targetPos)
	{
		return Vector3.Angle(transform.position - PlayerController.instance.transform.position, transform.position + targetPos);
	}

	// NPC�� ���� �ջ� ó��
	public void TakePhysicalDamage(int damageAmount)
	{
		health -= damageAmount;
		if (health <= 0)
			Die();

		// �ջ� �޾��� ���� �÷��� ȿ��
		StartCoroutine(DamageFlash());

		// ���� �޾��� ���� ���� ���
		// audioSource.PlayOneShot(damageSound);
	}

	// NPC ��� ó��
	void Die()
	{
		// ��� ���� ���
		// audioSource.PlayOneShot(deathSound);

		for (int x = 0; x < dropOnDeath.Length; x++)
		{
			Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
		}

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

	public MonsterData GetState()
	{
		int uniqueID = 3;
		string monsterType = "Rabbit";
		return new MonsterData(uniqueID, monsterType, transform.position, transform.rotation, health);
	}

}
