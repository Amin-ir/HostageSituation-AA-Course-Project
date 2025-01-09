using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SightComponent : MonoBehaviour
{
    [Header("Field of view settings")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private int detectionAngle = 90;
    [SerializeField] private float detectionStartThreshold; // how far away should start looking for player
    [SerializeField] private Transform Eye;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool DisplayRays;
    [SerializeField] private float PauseDuration = 1f;

    [HideInInspector]
    public Vector3 PointAtWhichPlayerIsSeen;
    [HideInInspector]
    public bool IsSeeingPlayer = false;
    [HideInInspector]
    public GameObject Player;

    EnemyStateManagerComponent stateManager;
    CapsuleCollider PlayerCapsule;
    NavMeshAgent agent;
    private void Start()
    {
        // Run the detection every 0.5 seconds (Optimization tip)
        InvokeRepeating(nameof(CheckFieldOfViewWithSphere), 0f, 0.5f);
        Player = FindObjectOfType<SetDestinationComponent>()
                .gameObject;
        PlayerCapsule = Player.GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        stateManager = GetComponent<EnemyStateManagerComponent>();
    }

    private void CheckFieldOfViewWithSphere()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= detectionStartThreshold)
        {
            Vector3 capsuleCenter = Player.transform.position + PlayerCapsule.center;
            float capsuleRadius = PlayerCapsule.radius;
            float capsuleHeight = PlayerCapsule.height;

            Collider[] hits = Physics.OverlapCapsule(capsuleCenter,
                                                     capsuleCenter + Vector3.up * capsuleHeight / 2,
                                                     capsuleRadius,
                                                     playerLayer);
            foreach (var hit in hits)
            {
                if (hit.CompareTag(GameTags.PlayerTag))
                {
                    Vector3 directionToPlayer = (hit.transform.position - Eye.position).normalized;

                    float angleToPlayer = Vector3.Angle(Eye.forward, directionToPlayer);
                    if (angleToPlayer < detectionAngle / 2)
                    {
                        if (IsPathClear(Eye.position, hit.transform.position))
                        {
                            if (DisplayRays)
                                Debug.DrawRay(Eye.position, directionToPlayer * detectionRadius, Color.green, 5f);

                            stateManager.TransitionToState(AlertnessLevel.Alerted);
                            PointAtWhichPlayerIsSeen = hit.transform.position;
                            IsSeeingPlayer = true;
                            return;
                        }
                        else
                        {
                            if (DisplayRays)
                                Debug.DrawRay(Eye.position, directionToPlayer * detectionRadius, Color.yellow, 5f);
                        }
                    }
                }
            }

            IsSeeingPlayer = false; // if player is seen, function is returned. but is seeing player prop. is set here, instead of
                                    // adding it, after each 'else' clause
        }
    }

	private bool IsPathClear(Vector3 start, Vector3 target)
    {
        NavMeshHit hit;
        if (NavMesh.Raycast(start, target, out hit, NavMesh.AllAreas))
        {
            return false; // Path is blocked
        }
        return true; // Path is clear
    }
}
