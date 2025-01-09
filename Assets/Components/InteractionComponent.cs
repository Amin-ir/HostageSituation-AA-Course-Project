using Assets.Resources.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class InteractionComponent : MonoBehaviour
{
    public bool isTryingToInteract = false;
    
    public float DistanceToInteractable = 2f;

    NavMeshAgent agent;
    Collider targetInteractable;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        targetInteractable = FindClosestInteractable();

        if (targetInteractable != null)
            targetInteractable.GetComponent<IInteractable>()
                              .DisplayInteractionButton();

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (targetInteractable != null)
            {
                MoveTowardsInteractable();
                isTryingToInteract = true;
            }
        }

		bool noPath = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance; // agent arrived at interactable
		if (isTryingToInteract && noPath) // has pressed 'interaction' button, now arrived at the location
        {
            IInteractable interactionComponent = targetInteractable.GetComponent<IInteractable>();
            RotateTowardsInteractable();
            interactionComponent.Interact();
            isTryingToInteract = false;
        }
    }

	private void MoveTowardsInteractable()
	{
		agent.SetDestination(targetInteractable.transform.position + targetInteractable.transform.forward);
	}

    private void RotateTowardsInteractable()
    {
        transform.forward = -targetInteractable.transform.forward;
    }

	private Collider FindClosestInteractable()
	{
		IEnumerable<Collider> colliders = Physics.OverlapBox(transform.position, Vector3.one * DistanceToInteractable);

        colliders = colliders.Where(c => c.TryGetComponent(out IInteractable interactable));

        if (!colliders.Any())
            return null;

        return colliders.OrderByDescending(c => Vector3.Distance(transform.position, c.transform.position))
                        .Last();
	}
}
