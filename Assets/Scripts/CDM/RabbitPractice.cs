using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitPractice : MonoBehaviour
{
    public MonsterDataSO data;

    private AIState currentaistate;

    public float minWanderingDistance;          // �ּ� ��ȸ �Ÿ�
    public float maxWanderingDistance;          // �ִ� ��ȸ �Ÿ�
    public float minWanderingWaitTime;          // ��ȸ �� �ּ� ��� �ð�
    public float maxWanderingWaitTime;          // ��ȸ �� �ִ� ��� �ð�

    public AudioSource audioSource;             // ��ȸ �� �ִ� ��� �ð�

    private float playerDistance;               // NPC�� �÷��̾� ������ �Ÿ�

    private NavMeshAgent agent;                 // NavMeshAgent ������Ʈ�� ���� ����
    private Animator animator;                  // animator ������Ʈ�� ���� ����
    private SkinnedMeshRenderer[] meshRenderers;        // �÷��� ȿ���� ���� SkinnedMeshRenderer ������Ʈ�� ���� ������

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}
}
