using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Zombie : MonoBehaviour
{
	// 몬스터의 현재 상태
	public AIState State;
	// 추적 사정거리
	public float traceDistance = 10.0f;
	// 공격 사정거리
	public float attackDistance = 2.0f;
	// 몬스터의 사망 여부
	public bool isDie = false;

	// Animoater 파라미터의 해시값 추출
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

		// 몬스터의 상태를 체크하는 코루틴 함수 호출
		StartCoroutine(CheckMonsterState());
		// 상태에 따라 몬스터의 행동을 수행하는 코루팀 함수 호출
		StartCoroutine(MonsterAction());
	}

	// 일정한 간격으로 몬스터의 행동 상태를 체크
	IEnumerator CheckMonsterState()
	{
		while (!isDie)
		{
			// 0.3초마다 중지(대기)하는 동안 제어권을 메시지 루프에 양보
			yield return new WaitForSeconds(0.3f);

			// 몬스터와 주인공 캐릭터 사이의 거리 측정
			float distance = Vector3.Distance(playerTr.position, monsterTr.position);

			// 공격 사정거리 범위로 들어왔는지 확인
			if (distance <= attackDistance)
			{
				State = AIState.Attacking;
			}

			// 추적 사정거리 범위로 들어왔는지 확인
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

	// 몬스터의 상태에 따라 몬스터의 동작을 수행
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
		// 추적 사정거리 표시
		if (State == AIState.Tracing)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, traceDistance);
		}

		// 공격 사정거리 표시
		if (State == AIState.Attacking)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackDistance);
		}
	}

}
