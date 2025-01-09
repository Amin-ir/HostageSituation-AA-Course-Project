using Assets.Resources.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HostageComponent : MonoBehaviour, IInteractable
{
    public bool IsFree = false;
    [SerializeField] GameObject player;
    [SerializeField] float followDistance = 2f;
    [SerializeField] Transform ExtractionPoint;
    [SerializeField] int rewardCount = 5;
    NavMeshAgent agent;
    Animator animator;
    HostageManager hostageManager;
    BillboardComponent interactionDisplayButton;
    InteractionComponent interactor;
	public void DisplayInteractionButton()
	{
		interactionDisplayButton.gameObject.SetActive(true);
	}

	public void HideInteractionButton()
	{
		interactionDisplayButton.gameObject.SetActive(false);
	}

	public void Interact()
    {
        if (!IsFree)
        {
            IsFree = true;
            gameObject.tag = GameTags.PlayerTag; // makes it visible to enemies
            GetComponent<BoxCollider>().enabled = false;
            player.GetComponent<TossingComponent>().ObjectCount += rewardCount;
        }
    }

	void Start()
    {
        interactor = FindObjectOfType<InteractionComponent>();
        interactionDisplayButton = GetComponentInChildren<BillboardComponent>();
        HideInteractionButton();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hostageManager = FindObjectOfType<HostageManager>();
    }

    void Update()
    {
        animator.SetBool("IsFree", IsFree);
        animator.SetBool("IsMoving", agent.hasPath);

        if (IsFree)
        {
            Vector3 followPosition = player.transform.position - player.transform.forward * followDistance;

            agent.SetDestination(followPosition);
        }

        if(Mathf.Abs(Vector3.Distance(transform.position, ExtractionPoint.position)) <= 2f)
        {
            player.GetComponent<TossingComponent>().ObjectCount += rewardCount;
            hostageManager.IncrementHostageFreed();
            gameObject.SetActive(false);
        }

		if (IsFree || Vector3.Distance(transform.position, interactor.transform.position) > interactor.DistanceToInteractable)
			HideInteractionButton();
	}
}
