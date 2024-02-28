using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Zombie : MonoBehaviour
{
	// ������ ���� ����
	public AIState State;
	// ���� �����Ÿ�
	public float traceDistance = 10.0f;
	// ���� �����Ÿ�
	public float attackDistance = 2.0f;
	// ������ ��� ����
	public bool isDie = false;

	// Animoater �Ķ������ �ؽð� ����
	private readonly int hashTrace = Animator.StringToHash("IsTrace");
	private readonly int hashAttack = Animator.StringToHash("IsAttack");

	private Transform monsterTr;
	private Transform playerTr;
	private NavMeshAgent agent;
	private Animator anim;


	private void Start()
	{
		monsterTr = GetComponent<Transform>();
		playerTr = PlayerController.instance.transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();

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
				State = AIState.Idle;
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

				case AIState.Die:
					break;
			}
			yield return new WaitForSeconds(0.3f);
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

}
