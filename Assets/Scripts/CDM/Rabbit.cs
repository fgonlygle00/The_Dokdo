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
	public float minWanderDistance;     // 최소 배회 거리
	public float maxWanderDistance;     // 최대 배회 거리
	public float minWanderWaitTime;     // 배회 전 최소 대기 시간
	public float maxWanderWaitTime;     // 배회 전 최대 대기 시간

	private float playerDistance;       // NPC와 플레이어 사이의 거리

	public float fieldOfView = 120f;

	private NavMeshAgent agent;         // NaveMeshAgent 컴포넌트에 대한 참조
    private Animator animator;			// Animator 컴포넌트에 대한 참조
	private SkinnedMeshRenderer[] meshRenderers;        // 플래시 효과를 위한 SinnedMeshRenderer 컴포넌트에 대한 참조들

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
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
			SetState(AIState.Fleeing);
		}
		else if (playerDistance >= safeDistance && aiState == AIState.Fleeing) // 도망 상태에서 안전 거리 이상이 되면 배회 상태로 전환
		{
			SetState(AIState.Wandering);
		}
		// 이동에 따른 애니메이터 업데이트
		//animator.SetBool("Moving", aiState != AIState.Idle);

		// 현재 AI 상태에 따른 행동 처리

		switch (aiState)
		{
			case AIState.Idle: 
			case AIState.Wandering: PassiveUpdate(); break;
			case AIState.Fleeing: FleeingUpdate(); break;
		}
	}

	// 도망 상태에서의 업데이트 로직
	private void FleeingUpdate()
	{
		if (!agent.pathPending && agent.remainingDistance < 0.1f)
		{
			agent.SetDestination(GetFleeLocation()); // 새로운 도망 위치 설정
		}
	}

	// 정지 및 배회 상태에서의 업데이트 로직
	private void PassiveUpdate()
	{
		if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
		{
			// 배회 목적지에 도달하면 정지 상태로 전환
			SetState(AIState.Idle);
			// 다음 배회 예약
			Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
		}
	}

	// AI 상태를 설정하고 관련 속성 업데이트
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

		animator.speed = agent.speed / walkSpeed;		// 이동 속도에 따른 애니메이션 속도 조정
	}

	// 새로운 위치로 배회 시작
	void WanderToNewLocation()
	{
		if (aiState != AIState.Idle)
		{
			return;
		}
		SetState(AIState.Wandering);
		agent.SetDestination(GetWanderLocation());

		// 배회 사운드 재생
		// audioSource.PlayOneShot(wanderSound);
	}

	// 새로운 배회 위치 계산
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

	// 도망 위치 계산
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


	// 도망 로직을 위한 목적지 각도 계산
	float GetDestinationAngle(Vector3 targetPos)
	{
		return Vector3.Angle(transform.position - PlayerController.instance.transform.position, transform.position + targetPos);
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
