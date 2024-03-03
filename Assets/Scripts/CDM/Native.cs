using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Native : MonoBehaviour
{
	// ������ ���� ����
	public AIState State;
	public ItemData[] dropOnDeath;
	public float health;

	// ���� �����Ÿ�
	public float traceDistance = 10.0f;
	// ���� �����Ÿ�
	public float attackDistance = 2.0f;
	// ��ȸ �����Ÿ�
	public float wanderDistance = 20.0f;
	// ������ ��� ����
	public bool isDie = false;

	[Header("Sound")]
	public AudioSource audioSource; // AudioSource ������Ʈ ������ ���� ����
	public AudioClip attackSound; // ���� ���� Ŭ��
	public AudioClip damageSound; // ���� �޾��� �� ���� Ŭ��
	public AudioClip deathSound; // ��� ���� Ŭ��
	public AudioClip wanderSound; // ��ȸ ���� Ŭ��

	// Animoater �Ķ������ �ؽð� ����
	private readonly int hashTrace = Animator.StringToHash("IsTrace");
	private readonly int hashAttack = Animator.StringToHash("IsAttack");
	private readonly int hashWander = Animator.StringToHash("IsWander");

	private Transform monsterTr;
	private Transform playerTr;
	private NavMeshAgent agent;
	private Animator anim;
	private SkinnedMeshRenderer[] meshRenderers;        // �÷��� ȿ���� ���� SinnedMeshRenderer ������Ʈ�� ���� ������


	private void Start()
	{
		monsterTr = GetComponent<Transform>();
		playerTr = PlayerController.instance.transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

		// agent.SetDestination(playerTr.position);

		// ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
		StartCoroutine(CheckMonsterState());
		// ���¿� ���� ������ �ൿ�� �����ϴ� �ڷ��� �Լ� ȣ��
		StartCoroutine(MonsterAction());
	}

	// ������ �������� ������ �ൿ ���¸� üũ
	IEnumerator CheckMonsterState()
	{
		while (!isDie)
		{
			// 0.3�ʸ��� ����(���)�ϴ� ���� ������� �޽��� ������ �纸
			yield return new WaitForSeconds(0.3f);

			// ���Ϳ� ���ΰ� ĳ���� ������ �Ÿ� ����
			float distance = Vector3.Distance(playerTr.position, monsterTr.position);

			// ���� �����Ÿ� ������ ���Դ��� Ȯ��
			if (distance <= attackDistance)
			{
				State = AIState.Attacking;
			}

			// ���� �����Ÿ� ������ ���Դ��� Ȯ��
			else if (distance <= traceDistance)
			{
				State = AIState.Tracing;
			}
			else
			{
				// Idle ���¿��� ���� Ȯ���� Wandering ���·� ��ȯ
				if (Random.Range(0, 5) < 1) // 20% Ȯ���� ��ȸ ���·� ��ȯ
				{
					State = AIState.Wandering;
				}
				else
				{
					State = AIState.Idle;
				}
			}
		}
	}

	// ������ ���¿� ���� ������ ������ ����
	IEnumerator MonsterAction()
	{
		while (!isDie)
		{
			switch (State)
			{
				case AIState.Idle:
					agent.isStopped = true;
					anim.SetBool(hashTrace, false);
					break;

				case AIState.Tracing:
					agent.SetDestination(playerTr.position);
					agent.isStopped = false;
					anim.SetBool(hashTrace, true);
					anim.SetBool(hashAttack, false);
					break;

				case AIState.Attacking:
					anim.SetBool(hashAttack, true);
					break;

				case AIState.Wandering:
					agent.isStopped = false;
					anim.SetBool(hashWander, true);
					// ��ȸ ���� ����
					Vector3 wanderPoint = RandomWanderPoint();
					agent.SetDestination(wanderPoint);
					break;
			}
			yield return new WaitForSeconds(0.3f);
		}

	}

	private Vector3 RandomWanderPoint()
	{
		Vector3 randomPoint = Random.insideUnitSphere * wanderDistance + transform.position;
		NavMeshHit navHit;
		NavMesh.SamplePosition(randomPoint, out navHit, wanderDistance, -1);
		return navHit.position;

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
		// ���� �����Ÿ� ǥ��
		if (State == AIState.Tracing)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, traceDistance);
		}

		// ���� �����Ÿ� ǥ��
		if (State == AIState.Attacking)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackDistance);
		}
	}

	public MonsterData GetState()
	{
		return new MonsterData(transform.position, transform.rotation, health);
	}
}
