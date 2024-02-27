using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitPractice : MonoBehaviour
{
    public MonsterDataSO data;

    private AIState currentaistate;

    public float minWanderingDistance;          // 최소 배회 거리
    public float maxWanderingDistance;          // 최대 배회 거리
    public float minWanderingWaitTime;          // 배회 전 최소 대기 시간
    public float maxWanderingWaitTime;          // 배회 전 최대 대기 시간

    public AudioSource audioSource;             // 배회 전 최대 대기 시간

    private float playerDistance;               // NPC와 플레이어 사이의 거리

    private NavMeshAgent agent;                 // NavMeshAgent 컴포넌트에 대한 참조
    private Animator animator;                  // animator 컴포넌트에 대한 참조
    private SkinnedMeshRenderer[] meshRenderers;        // 플래시 효과를 위한 SkinnedMeshRenderer 컴포넌트에 대한 참조들

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}
}
