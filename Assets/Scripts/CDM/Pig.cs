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

	private float playerDistance;       // NPC와 플레이어 사이의 거리	

	public float fieldOfView = 120f;

	[Header("Sound")]
	public AudioSource audioSource; // AudioSource 컴포넌트 참조를 위한 변수
	public AudioClip attackSound; // 공격 사운드 클립
	public AudioClip damageSound; // 피해 받았을 때 사운드 클립
	public AudioClip deathSound; // 사망 사운드 클립
	public AudioClip wanderSound; // 배회 사운드 클립

	private NavMeshAgent agent;         // NaveMeshAgent 컴포넌트에 대한 참조
 // private Animator animator;		    // Animator 컴포넌트에 대한 참조
	private SkinnedMeshRenderer[] meshRenderers;        // 플래시 효과를 위한 SinnedMeshRenderer 컴포넌트에 대한 참조들

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		// animator = GetComponentInChildren<Animator>();
		// audioSource = GetComponent<AudioSource>();			// AudioSource 컴포넌트 가져오기
		meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	private void Start()
	{
		SetState(AIState.Wandering);        // 초기 AI 상태를 배회로 설정
	}

	private void Update()
	{
		// 플레이어와의 거리 계산 
		playerDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

		if (playerDistance < safeDistance)
		{
			 SetState(AIState.Attacking);
		}
		else if (playerDistance >= safeDistance && aiState == AIState.Attacking) 
		{
			SetState(AIState.Wandering);
		}

		// 이동에 따른 애니메이터 업데이트
		//animator.SetBool("Moving", aiState != AIState.Idle);

		// 현재 AI 상태에 따른 행동 처리
		switch (aiState)
		{
			case AIState.Idle: PassiveUpdate(); break;  // 대기 상태
			case AIState.Wandering: PassiveUpdate(); break; // 배회 상태
			case AIState.Attacking: AttackingUpdate(); break; // 공격 상태
		}
	}

	// 공격 상태에서의 업데이트 로직
	private void AttackingUpdate()
	{
		// 플레이어와의 거리가 공격 범위 밖이거나 플레이어가 시야 안에 있지 않은 경우
		if (playerDistance > attackDistance || !IsPlaterInFireldOfView())
		{
			// 플레이어와의 거리가 배회 거리보다 큰 경우, 배회 상태로 전환
			if (playerDistance > maxWanderDistance)
			{
				SetState(AIState.Wandering);
				Debug.Log(playerDistance);
				return; // 메서드 종료
			}

			// AI를 멈추지않고 플레이어에게 다가가기 위해 경로를 계산함
			agent.isStopped = false;
			NavMeshPath path = new NavMeshPath();
			if (agent.CalculatePath(PlayerController.instance.transform.position, path))
			{
				// 경로가 유효하면, 해당 위치로 이동을 설정
				agent.SetDestination(PlayerController.instance.transform.position);
			}
			else
			{
				// 경로를 계산할 수 없는 경우, 도망 상태로 전환
				SetState(AIState.Wandering);
			}
		}
		// 플레이어가 공격 범위 안에 있고 시야 안에 있는 경우
		else
		{
			
			agent.isStopped = true;		// AI를 멈추고 공격 준비
			if (Time.time - lastAttackTime > attackRate)
			{
				// 마지막 공격 이후 충분한 시간이 경과했으면 공격 실행
				lastAttackTime = Time.time;	// 마지막 공격 시간을 현재로 업데이트
				PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(damage);  // 플레이어에게 물리적 피해를 주는 코드
				// animator.speed = 1;		// 애니메이션 속도를 정상으로 설정
				// animator.SetTrigger("Attack");		// 공격 애니메이션 트리거
				// 공격 사운드 재생
				// audioSource.PlayOneShot(attackSound);
			}
		}

		
	}

	// 정지 및 배회 상태에서의 업데이트 로직
	private void PassiveUpdate()
	{
		Debug.Log(agent.remainingDistance);
		if (aiState == AIState.Wandering && agent.remainingDistance < 2.0f)
		{
		
			// 배회 목적지에 도달하면 정지 상태로 전환
			SetState(AIState.Idle);
			// 다음 배회 예약
			Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
		}

		// 플레이어를 감지하면 공격 상태로 전환
		if (playerDistance < detectDistance)
		{
			SetState(AIState.Attacking);
		}
	}

	// 플레이어가 시야각 내에 있는지 확인
	bool IsPlaterInFireldOfView()
	{
		// 플레이어와 AI 캐릭터 사이의 방향 벡터를 계산
		Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;

		// AI 캐릭터의 전방 벡터와 플레이어 방향 벡터 사이의 각도를 계산합니다
		float angle = Vector3.Angle(transform.forward, directionToPlayer);

		// 계산된 각도가 AI 캐릭터의 시야각의 절반보다 작은 경우 true를 반환
		// 이는 플레이어가 AI의 시야 내에 있음을 의미
		return angle < fieldOfView * 0.5f;
	}

	// AI 상태를 설정하고 관련 속성 업데이트
	private void SetState(AIState newState)
	{
		aiState = newState;		// AI의 현재 상태를 새로운 상태로 업데이트한다.

		// 새로운 상태에 따라 다른 동작을 실행합니다.
		switch (aiState)
		{
			case AIState.Idle:                              // AI 상태가 Idle(대기)일 때
				{
					agent.speed = walkSpeed;           // NavMeshAgent의 이동 속도를 걷는 속도로 설정합니다.
					agent.isStopped = true;                 // NavMeshAgent의 이동을 중지합니다.
				}
				break;
			case AIState.Wandering:                         // AI 상태가 Wandering(배회)일 때
				{
					agent.speed = walkSpeed;           // NavMeshAgent의 이동 속도를 걷는 속도로 설정합니다.
					agent.isStopped = false;                // NavMeshAgent가 목적지로 이동할 수 있도록 합니다.
				}
				break;

			case AIState.Attacking:                        
				{
					agent.speed = runSpeed;            // NavMeshAgent의 이동 속도를 뛰는 속도로 설정합니다.
					agent.isStopped = false;                // NavMeshAgent가 목적지로 이동할 수 있도록 합니다.
				}
				break;
		}

		// 아래의 코드는 애니메이션 속도를 NavMeshAgent의 이동 속도에 맞춰 조정합니다.
		// 이는 캐릭터의 이동 속도가 걷기 속도로 변경될 때 애니메이션 속도도 그에 맞춰 조정되도록 합니다.
		// animator.speed = agent.speed / walkSpeed;       // 이동 속도에 따른 애니메이션 속도 조정
	}

	// 새로운 위치로 배회 시작
	void WanderToNewLocation()
	{
		// 현재 AI 상태가 대기 상태가 아니라면 이 메서드를 종료한다.
		// 이는 AI가 다른 활동(공격, 도망 등) 중일 때는 새 위치로의 배회를 시작하지 않도록 하기 위함이다.
		if (aiState != AIState.Idle)
		{
			return;
		}
		// AI 상태를 배회 상태로 변경한다. 이는 AI가 새로운 위치로 이동하기 시작함을 의미한다.
		SetState(AIState.Wandering);
		// GetWanderLocation 메서드를 호출하여 새로운 배회 위치를 계산하고,
		// NavMeshAgent 컴포넌트의 목적지로 설정하여 AI를 이동시킨다.
		agent.SetDestination(GetWanderLocation());

		// 배회 사운드 재생
		// audioSource.PlayOneShot(wanderSound);
	}

	// 새로운 배회 위치 계산
	Vector3 GetWanderLocation()
	{

		NavMeshHit hit; // NavMeshHit 구조체를 선언한다. 이 구조체는 NavMesh 샘플링 결과를 저장하는 데 사용된다.


		// 현재 위치에서 무작위 방향으로 minWanderDistance와 maxWanderDistance 사이의 거리에 있는 지점에 대한 NavMesh 샘플링을 시도한다.
		// SamplePosition은 해당 지점이 NavMesh 상에 있는지 확인하고, 있으면 그 위치를 hit 변수에 저장한다.
		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

		int i = 0;  // 시도 횟수를 추적하기 위한 변수 i를 초기화한다.

		// 샘플링된 위치가 캐릭터의 감지 거리 내에 있는지 확인한다. 만약 그렇다면, 캐릭터가 쉽게 발견할 수 있는 위치이므로 다른 위치를 찾는다.
		while (Vector3.Distance(transform.position, hit.position) < detectDistance)
		{
			// 새로운 위치에 대한 NavMesh 샘플링을 다시 시도한다.
			NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
			i++;        // 시도 횟수를 증가시킨다.

			// 만약 30번 시도했음에도 불구하고 적절한 위치를 찾지 못했다면, 루프를 종료한다.
			// 이는 무한 루프에 빠지는 것을 방지하기 위함이다.
			if (i == 30)
				break;
		}

		return hit.position;        // 최종적으로 찾은 위치를 반환한다. 이 위치는 캐릭터가 배회할 새로운 목적지이다.
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
		// 배회 거리를 표시하는 기즈모 그리기
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, maxWanderDistance);

		// 공격 거리를 표시하는 기즈모 그리기
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackDistance);

		// AI의 위치와 시야각을 기준으로 기즈모를 그립니다.
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 0.5f); // AI의 위치를 표시하는 작은 원
	}

	public MonsterData GetState()
	{
		return new MonsterData(transform.position, transform.rotation, health);
	}

}
