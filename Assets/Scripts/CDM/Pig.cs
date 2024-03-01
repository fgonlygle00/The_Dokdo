using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pig : MonoBehaviour, IDamagable
{
	[Header("Stats")]
	public float walkSpeed;
	public float runSpeed;
	public float health;
	public ItemData[] dropOnDeath;

	[Header("AI")]
	private AIState aiState;
	public float detectDistance;
	public float safeDistance;

	[Header("Wandering")]
	public float minWanderDistance;
	public float maxWanderDistance;
	public float minWanderWaitTime;
	public float maxWanderWaitTime;

	[Header("Combat")]
	public int damage;
	public float attackRate;
	private float lastAttackTime;
	public float attackDistance;

	private float playerDistance;       // NPC�� �÷��̾� ������ �Ÿ�	

	public float fieldOfView = 120f;

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
		// �÷��̾���� �Ÿ� ��� 
		playerDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

		if (playerDistance < safeDistance)
		{
			 SetState(AIState.Attacking);
		}
		else if (playerDistance >= safeDistance && aiState == AIState.Attacking) 
		{
			SetState(AIState.Wandering);
		}

		// �̵��� ���� �ִϸ����� ������Ʈ
		//animator.SetBool("Moving", aiState != AIState.Idle);

		// ���� AI ���¿� ���� �ൿ ó��
		switch (aiState)
		{
			case AIState.Idle: PassiveUpdate(); break;  // ��� ����
			case AIState.Wandering: PassiveUpdate(); break; // ��ȸ ����
			case AIState.Attacking: AttackingUpdate(); break; // ���� ����
		}
	}

	// ���� ���¿����� ������Ʈ ����
	private void AttackingUpdate()
	{
		// �÷��̾���� �Ÿ��� ���� ���� ���̰ų� �÷��̾ �þ� �ȿ� ���� ���� ���
		if (playerDistance > attackDistance || !IsPlaterInFireldOfView())
		{
			// �÷��̾���� �Ÿ��� ��ȸ �Ÿ����� ū ���, ��ȸ ���·� ��ȯ
			if (playerDistance > maxWanderDistance)
			{
				SetState(AIState.Wandering);
				Debug.Log(playerDistance);
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
				SetState(AIState.Wandering);
			}
		}
		// �÷��̾ ���� ���� �ȿ� �ְ� �þ� �ȿ� �ִ� ���
		else
		{
			
			agent.isStopped = true;		// AI�� ���߰� ���� �غ�
			if (Time.time - lastAttackTime > attackRate)
			{
				// ������ ���� ���� ����� �ð��� ��������� ���� ����
				lastAttackTime = Time.time;	// ������ ���� �ð��� ����� ������Ʈ
				PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(damage);  // �÷��̾�� ������ ���ظ� �ִ� �ڵ�
				// animator.speed = 1;		// �ִϸ��̼� �ӵ��� �������� ����
				// animator.SetTrigger("Attack");		// ���� �ִϸ��̼� Ʈ����
				// ���� ���� ���
				// audioSource.PlayOneShot(attackSound);
			}
		}

		
	}

	// ���� �� ��ȸ ���¿����� ������Ʈ ����
	private void PassiveUpdate()
	{
		Debug.Log(agent.remainingDistance);
		if (aiState == AIState.Wandering && agent.remainingDistance < 2.0f)
		{
		
			// ��ȸ �������� �����ϸ� ���� ���·� ��ȯ
			SetState(AIState.Idle);
			// ���� ��ȸ ����
			Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
		}

		// �÷��̾ �����ϸ� ���� ���·� ��ȯ
		if (playerDistance < detectDistance)
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
		return angle < fieldOfView * 0.5f;
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
					agent.speed = walkSpeed;           // NavMeshAgent�� �̵� �ӵ��� �ȴ� �ӵ��� �����մϴ�.
					agent.isStopped = true;                 // NavMeshAgent�� �̵��� �����մϴ�.
				}
				break;
			case AIState.Wandering:                         // AI ���°� Wandering(��ȸ)�� ��
				{
					agent.speed = walkSpeed;           // NavMeshAgent�� �̵� �ӵ��� �ȴ� �ӵ��� �����մϴ�.
					agent.isStopped = false;                // NavMeshAgent�� �������� �̵��� �� �ֵ��� �մϴ�.
				}
				break;

			case AIState.Attacking:                        
				{
					agent.speed = runSpeed;            // NavMeshAgent�� �̵� �ӵ��� �ٴ� �ӵ��� �����մϴ�.
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
		while (Vector3.Distance(transform.position, hit.position) < detectDistance)
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
		Gizmos.DrawWireSphere(transform.position, attackDistance);

		// AI�� ��ġ�� �þ߰��� �������� ����� �׸��ϴ�.
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 0.5f); // AI�� ��ġ�� ǥ���ϴ� ���� ��
	}

	public MonsterData GetState()
	{
		return new MonsterData(transform.position, transform.rotation, health);
	}

}
