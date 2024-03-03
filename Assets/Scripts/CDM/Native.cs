using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Native : MonoBehaviour
{
	// 몬스터의 현재 상태
	public AIState State;
	public ItemData[] dropOnDeath;
	public float health;

	// 추적 사정거리
	public float traceDistance = 10.0f;
	// 공격 사정거리
	public float attackDistance = 2.0f;
	// 배회 사정거리
	public float wanderDistance = 20.0f;
	// 몬스터의 사망 여부
	public bool isDie = false;

	[Header("Sound")]
	public AudioSource audioSource; // AudioSource 컴포넌트 참조를 위한 변수
	public AudioClip attackSound; // 공격 사운드 클립
	public AudioClip damageSound; // 피해 받았을 때 사운드 클립
	public AudioClip deathSound; // 사망 사운드 클립
	public AudioClip wanderSound; // 배회 사운드 클립

	// Animoater 파라미터의 해시값 추출
	private readonly int hashTrace = Animator.StringToHash("IsTrace");
	private readonly int hashAttack = Animator.StringToHash("IsAttack");
	private readonly int hashWander = Animator.StringToHash("IsWander");

	private Transform monsterTr;
	private Transform playerTr;
	private NavMeshAgent agent;
	private Animator anim;
	private SkinnedMeshRenderer[] meshRenderers;        // 플래시 효과를 위한 SinnedMeshRenderer 컴포넌트에 대한 참조들


	private void Start()
	{
		monsterTr = GetComponent<Transform>();
		playerTr = PlayerController.instance.transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

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
				// Idle 상태에서 일정 확률로 Wandering 상태로 전환
				if (Random.Range(0, 5) < 1) // 20% 확률로 배회 상태로 전환
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

				case AIState.Wandering:
					agent.isStopped = false;
					anim.SetBool(hashWander, true);
					// 배회 지점 설정
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

	// NPC가 받은 손상 처리
	public void TakePhysicalDamage(int damageAmount)
	{
		health -= damageAmount;
		if (health <= 0)
			Die();

		// 손상 받았을 때의 플래시 효과
		StartCoroutine(DamageFlash());

		// 피해 받았을 때의 사운드 재생
		// audioSource.PlayOneShot(damageSound);
	}

	// NPC 사망 처리
	void Die()
	{
		// 사망 사운드 재생
		// audioSource.PlayOneShot(deathSound);

		for (int x = 0; x < dropOnDeath.Length; x++)
		{
			Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
		}

		// NPC 오브젝트 파괴
		Destroy(gameObject);
	}

	// 손상 플래시 효과를 위한 코루틴
	IEnumerator DamageFlash()
	{
		// 모든 메쉬 렌더러를 순회하는 반복문입니다.
		for (int x = 0; x < meshRenderers.Length; x++)
		{
			// 현재 메쉬 렌더러의 재질 색상을 빨간색 계열로 변경하여 데미지를 받았음을 표시합니다.
			// 여기서 색상 값은 R=1.0, G=0.6, B=0.6으로 설정되어, 약간 붉은색을 띕니다.
			meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);
		}

		// 0.1초 동안 대기합니다. 이 시간 동안 캐릭터는 변경된 색상으로 표시됩니다.
		yield return new WaitForSeconds(0.1f);

		// 대기 시간이 지난 후, 모든 메쉬 렌더러의 색상을 다시 원래대로 (흰색) 변경합니다.
		// 이로써 데미지를 받은 효과가 일시적임을 나타냅니다.
		for (int x = 0; x < meshRenderers.Length; x++)
		{
			meshRenderers[x].material.color = Color.white;
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

	public MonsterData GetState()
	{
		return new MonsterData(transform.position, transform.rotation, health);
	}
}
