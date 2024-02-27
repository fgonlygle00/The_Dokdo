using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pig : MonoBehaviour
{
	public MonsterDataSO data;

	private AIState aiState;

	[Header("Wandering")]
	public float minWanderDistance;
	public float maxWanderDistance;
	public float minWanderWaitTime;
	public float maxWanderWaitTime;

	[Header("Combat")]
	private float lastAttackTime;
	
	private float playerDistance;       // NPC�� �÷��̾� ������ �Ÿ�	

	[Header("Sound")]
	public AudioSource audioSource; // AudioSource ������Ʈ ������ ���� ����
	public AudioClip attackSound; // ���� ���� Ŭ��
	public AudioClip damageSound; // ���� �޾��� �� ���� Ŭ��
	public AudioClip deathSound; // ��� ���� Ŭ��
	public AudioClip wanderSound; // ��ȸ ���� Ŭ��

	private NavMeshAgent agent;         // NaveMeshAgent ������Ʈ�� ���� ����
 // private Animator animator;		    // Animator ������Ʈ�� ���� ����
	private SkinnedMeshRenderer[] meshRenderers;        // �÷��� ȿ���� ���� SinnedMeshRenderer ������Ʈ�� ���� ������

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		// animator = GetComponentInChildren<Animator>();
		// audioSource = GetComponent<AudioSource>();			// AudioSource ������Ʈ ��������
		meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	private void Start()
	{
		SetState(AIState.Wandering);        // �ʱ� AI ���¸� ��ȸ�� ����
	}

	private void Update()
	{
		// �÷��̾���� �Ÿ� ��� : ���� �÷��̾ ���� �ּ�ó����.
		playerDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

		// �̵��� ���� �ִϸ����� ������Ʈ
		//animator.SetBool("Moving", aiState != AIState.Idle);

		// ���� AI ���¿� ���� �ൿ ó��
		switch (aiState)
		{
			case AIState.Idle: PassiveUpdate(); break;  // ��� ����
			case AIState.Wandering: PassiveUpdate(); break; // ��ȸ ����
			case AIState.Attacking: AttackingUpdate(); break; // ���� ����
			case AIState.Fleeing: FleeingUpdate(); break;		// ���� ����
		}
	}

	// ���� ���¿����� ������Ʈ ����
	private void FleeingUpdate()
	{
		if (agent.remainingDistance < 0.1f)
		{
			agent.SetDestination(GetFleeLocation());            // ���� �������� ��������� ���ο� ���� ��ġ ����
		}
		else
		{
			SetState(AIState.Wandering);                        // ���� ���°� �ƴϸ� ��ȸ�� ��ȯ
		}
		Debug.Log("���� ����");
	}

	// ���� ���¿����� ������Ʈ ����
	private void AttackingUpdate()
	{
		// �÷��̾���� �Ÿ��� ���� ���� ���̰ų� �÷��̾ �þ� �ȿ� ���� ���� ���
		if (playerDistance > data.attackDistance || !IsPlaterInFireldOfView())
		{
			// �÷��̾���� �Ÿ��� ��ȸ �Ÿ����� ū ���, ��ȸ ���·� ��ȯ
			if (playerDistance > maxWanderDistance)
			{
				SetState(AIState.Wandering);
				return; // �޼��� ����
			}

			// AI�� �������ʰ� �÷��̾�� �ٰ����� ���� ��θ� �����
			agent.isStopped = false;
			NavMeshPath path = new NavMeshPath();
			if (agent.CalculatePath(PlayerController.instance.transform.position, path))
			{
				// ��ΰ� ��ȿ�ϸ�, �ش� ��ġ�� �̵��� ����
				agent.SetDestination(PlayerController.instance.transform.position);
			}
			else
			{
				// ��θ� ����� �� ���� ���, ���� ���·� ��ȯ
				SetState(AIState.Fleeing);
			}
		}
		// �÷��̾ ���� ���� �ȿ� �ְ� �þ� �ȿ� �ִ� ���
		else
		{
			
			agent.isStopped = true;		// AI�� ���߰� ���� �غ�
			if (Time.time - lastAttackTime > data.attackRate)
			{
				// ������ ���� ���� ����� �ð��� ��������� ���� ����
				lastAttackTime = Time.time;	// ������ ���� �ð��� ����� ������Ʈ
				// PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(data.damage);	// �÷��̾�� ������ ���ظ� �ִ� �ڵ�
				// animator.speed = 1;		// �ִϸ��̼� �ӵ��� �������� ����
				// animator.SetTrigger("Attack");		// ���� �ִϸ��̼� Ʈ����
			}
		}

		// ���� ���� ���
		// audioSource.PlayOneShot(attackSound);
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

		// �÷��̾ �����ϸ� ���� ���·� ��ȯ
		if (playerDistance < data.detectDistance)
		{
			SetState(AIState.Attacking);
		}
	}

	// �÷��̾ �þ߰� ���� �ִ��� Ȯ��
	bool IsPlaterInFireldOfView()
	{
		// �÷��̾�� AI ĳ���� ������ ���� ���͸� ���
		Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;

		// AI ĳ������ ���� ���Ϳ� �÷��̾� ���� ���� ������ ������ ����մϴ�
		float angle = Vector3.Angle(transform.forward, directionToPlayer);

		// ���� ������ AI ĳ������ �þ߰��� ���ݺ��� ���� ��� true�� ��ȯ
		// �̴� �÷��̾ AI�� �þ� ���� ������ �ǹ�
		return angle < data.fieldOfView * 0.5f;
	}

	// AI ���¸� �����ϰ� ���� �Ӽ� ������Ʈ
	private void SetState(AIState newState)
	{
		aiState = newState;		// AI�� ���� ���¸� ���ο� ���·� ������Ʈ�Ѵ�.

		// ���ο� ���¿� ���� �ٸ� ������ �����մϴ�.
		switch (aiState)
		{
			case AIState.Idle:                              // AI ���°� Idle(���)�� ��
				{
					agent.speed = data.walkSpeed;           // NavMeshAgent�� �̵� �ӵ��� �ȴ� �ӵ��� �����մϴ�.
					agent.isStopped = true;                 // NavMeshAgent�� �̵��� �����մϴ�.
				}
				break;
			case AIState.Wandering:                         // AI ���°� Wandering(��ȸ)�� ��
				{
					agent.speed = data.walkSpeed;           // NavMeshAgent�� �̵� �ӵ��� �ȴ� �ӵ��� �����մϴ�.
					agent.isStopped = false;                // NavMeshAgent�� �������� �̵��� �� �ֵ��� �մϴ�.
				}
				break;

			case AIState.Attacking:                        
				{
					agent.speed = data.runSpeed;            // NavMeshAgent�� �̵� �ӵ��� �ٴ� �ӵ��� �����մϴ�.
					agent.isStopped = false;                // NavMeshAgent�� �������� �̵��� �� �ֵ��� �մϴ�.
				}
				break;
			case AIState.Fleeing:                           // AI ���°� Fleeing(����)�� ��
				{
					agent.speed = data.runSpeed;            // NavMeshAgent�� �̵� �ӵ��� �ٴ� �ӵ��� �����մϴ�.
					agent.isStopped = false;                // NavMeshAgent�� �������� �̵��� �� �ֵ��� �մϴ�.
				}
				break;
		}

		// �Ʒ��� �ڵ�� �ִϸ��̼� �ӵ��� NavMeshAgent�� �̵� �ӵ��� ���� �����մϴ�.
		// �̴� ĳ������ �̵� �ӵ��� �ȱ� �ӵ��� ����� �� �ִϸ��̼� �ӵ��� �׿� ���� �����ǵ��� �մϴ�.
		// animator.speed = agent.speed / walkSpeed;       // �̵� �ӵ��� ���� �ִϸ��̼� �ӵ� ����
	}

	// ���ο� ��ġ�� ��ȸ ����
	void WanderToNewLocation()
	{
		// ���� AI ���°� ��� ���°� �ƴ϶�� �� �޼��带 �����Ѵ�.
		// �̴� AI�� �ٸ� Ȱ��(����, ���� ��) ���� ���� �� ��ġ���� ��ȸ�� �������� �ʵ��� �ϱ� �����̴�.
		if (aiState != AIState.Idle)
		{
			return;
		}
		// AI ���¸� ��ȸ ���·� �����Ѵ�. �̴� AI�� ���ο� ��ġ�� �̵��ϱ� �������� �ǹ��Ѵ�.
		SetState(AIState.Wandering);
		// GetWanderLocation �޼��带 ȣ���Ͽ� ���ο� ��ȸ ��ġ�� ����ϰ�,
		// NavMeshAgent ������Ʈ�� �������� �����Ͽ� AI�� �̵���Ų��.
		agent.SetDestination(GetWanderLocation());

		// ��ȸ ���� ���
		// audioSource.PlayOneShot(wanderSound);
	}

	// ���ο� ��ȸ ��ġ ���
	Vector3 GetWanderLocation()
	{

		NavMeshHit hit; // NavMeshHit ����ü�� �����Ѵ�. �� ����ü�� NavMesh ���ø� ����� �����ϴ� �� ���ȴ�.


		// ���� ��ġ���� ������ �������� minWanderDistance�� maxWanderDistance ������ �Ÿ��� �ִ� ������ ���� NavMesh ���ø��� �õ��Ѵ�.
		// SamplePosition�� �ش� ������ NavMesh �� �ִ��� Ȯ���ϰ�, ������ �� ��ġ�� hit ������ �����Ѵ�.
		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

		int i = 0;  // �õ� Ƚ���� �����ϱ� ���� ���� i�� �ʱ�ȭ�Ѵ�.

		// ���ø��� ��ġ�� ĳ������ ���� �Ÿ� ���� �ִ��� Ȯ���Ѵ�. ���� �׷��ٸ�, ĳ���Ͱ� ���� �߰��� �� �ִ� ��ġ�̹Ƿ� �ٸ� ��ġ�� ã�´�.
		while (Vector3.Distance(transform.position, hit.position) < data.detectDistance)
		{
			// ���ο� ��ġ�� ���� NavMesh ���ø��� �ٽ� �õ��Ѵ�.
			NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
			i++;        // �õ� Ƚ���� ������Ų��.

			// ���� 30�� �õ��������� �ұ��ϰ� ������ ��ġ�� ã�� ���ߴٸ�, ������ �����Ѵ�.
			// �̴� ���� ������ ������ ���� �����ϱ� �����̴�.
			if (i == 30)
				break;
		}

		return hit.position;        // ���������� ã�� ��ġ�� ��ȯ�Ѵ�. �� ��ġ�� ĳ���Ͱ� ��ȸ�� ���ο� �������̴�.
	}

	// ���� ��ġ ���
	Vector3 GetFleeLocation()
	{
		NavMeshHit hit;             // NavMeshHit ����ü�� �����Ѵ�. �� ����ü�� NavMesh ���ø� ����� �����ϴ� �� ���ȴ�.

		// ���� ��ġ���� ������ �������� ���� �Ÿ�(data.safeDistance)��ŭ ������ ������ ���� NavMesh ���ø��� �õ��Ѵ�.
		// SamplePosition�� �ش� ������ NavMesh �� �ִ��� Ȯ���ϰ�, ������ �� ��ġ�� hit ������ �����Ѵ�.
		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * data.safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);

		
		int i = 0;              // �õ� Ƚ���� �����ϱ� ���� ���� i�� �ʱ�ȭ�Ѵ�.

		// ���ø��� ��ġ�� �÷��̾�κ����� ������ 90�� �̻��̰�, �÷��̾�κ����� �Ÿ��� ���� �Ÿ�(data.safeDistance) �̻����� Ȯ���Ѵ�.
		while (GetDestinationAngle(hit.position) > 90 || playerDistance < data.safeDistance)
		{
			// ������ �������� �ʴ� ��ġ�� ���� ���ο� ��ġ�� ���� NavMesh ���ø��� �ٽ� �õ��Ѵ�.
			NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * data.safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
			i++;            // �õ� Ƚ���� ������Ų��.

			// ���� 30�� �õ��������� �ұ��ϰ� ������ ��ġ�� ã�� ���ߴٸ�, ������ �����Ѵ�.
			// �̴� ���� ������ ������ ���� �����ϱ� �����̴�.
			if (i == 30)
				break;
		}

		// ���������� ã�� ��ġ�� ��ȯ�Ѵ�. �� ��ġ�� ĳ���Ͱ� ����ĥ �� ����� ���� ��ġ�̴�.
		return hit.position;
	}

	float GetDestinationAngle(Vector3 targetPos)
	{
		// �÷��̾��� ��ġ���� ��ǥ ��ġ������ ���� ���͸� ����Ѵ�.
		Vector3 directionToTarget = targetPos - PlayerController.instance.transform.position;

		// AI ĳ������ ���� ���Ϳ�, �÷��̾� ��ġ���� ��ǥ ��ġ������ ���� ���� ������ ������ ����Ѵ�.
		float angle = Vector3.Angle(transform.forward, directionToTarget);

		// ���� ������ ��ȯ�Ѵ�.
		return angle;
	}

	// NPC�� ���� �ջ� ó��
	public void TakePhysicalDamage(int damageAmount)
	{
		data.health -= damageAmount;
		if (data.health <= 0)
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
		// ��� �޽� �������� ��ȸ�ϴ� �ݺ����Դϴ�.
		for (int x = 0; x < meshRenderers.Length; x++)
		{
			// ���� �޽� �������� ���� ������ ������ �迭�� �����Ͽ� �������� �޾����� ǥ���մϴ�.
			// ���⼭ ���� ���� R=1.0, G=0.6, B=0.6���� �����Ǿ�, �ణ �������� ��ϴ�.
			meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);
		}

		// 0.1�� ���� ����մϴ�. �� �ð� ���� ĳ���ʹ� ����� �������� ǥ�õ˴ϴ�.
		yield return new WaitForSeconds(0.1f);

		// ��� �ð��� ���� ��, ��� �޽� �������� ������ �ٽ� ������� (���) �����մϴ�.
		// �̷ν� �������� ���� ȿ���� �Ͻ������� ��Ÿ���ϴ�.
		for (int x = 0; x < meshRenderers.Length; x++)
		{
			meshRenderers[x].material.color = Color.white;
		}
	}

	private void OnDrawGizmos()
	{
		// ��ȸ �Ÿ��� ǥ���ϴ� ����� �׸���
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, maxWanderDistance);

		// ���� �Ÿ��� ǥ���ϴ� ����� �׸���
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, data.attackDistance);

		// AI�� ��ġ�� �þ߰��� �������� ����� �׸��ϴ�.
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 0.5f); // AI�� ��ġ�� ǥ���ϴ� ���� ��
	}

}
