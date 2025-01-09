using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetDestinationComponent : MonoBehaviour
{
    [SerializeField] private GameObject clickedAreaPrefab;
    [SerializeField] private Vector3 clickedAreaPositionInstantiationOffset;

    Camera mainCamera;
    NavMeshAgent navAgent;
    Animator animator;
    LineRenderer lineRenderer;
    HealthComponent healthComponent;
    InteractionComponent interactionComponent;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
        healthComponent = GetComponent<HealthComponent>();
        interactionComponent = GetComponent<InteractionComponent>();
    }

    void Update()
    {
        animator.SetFloat("Speed", transform.position.x != navAgent.destination.x ? 1 : 0);

        if (healthComponent.CurrentHealth > 0) // is alive
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit rayHit))
                {
                    Instantiate(clickedAreaPrefab,
                                rayHit.point + clickedAreaPositionInstantiationOffset,
                                Quaternion.identity);
                    navAgent.SetDestination(rayHit.point);
                    interactionComponent.isTryingToInteract = false;
                }
            }

            if (navAgent.hasPath)
                DrawPath();
            else
                lineRenderer.positionCount = 0;

            if (Input.GetMouseButtonDown(2))
                navAgent.SetDestination(transform.position); // cancels the path
        }else
        {
            if(navAgent.hasPath)
                navAgent.SetDestination(transform.position); // if dead, stop him where he is
        }
    }

    private void DrawPath()
    {
        if (navAgent.path.corners.Length < 2) return; // No path or at destination

        lineRenderer.positionCount = navAgent.path.corners.Length;
        lineRenderer.SetPositions(navAgent.path.corners);
    }
}
