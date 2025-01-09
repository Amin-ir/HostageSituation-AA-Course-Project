using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatComponent : MonoBehaviour
{
    [Header("Combat Configurations")]
    public float DistanceToShoot = 6f;
    public float PauseDuration = 3f;

    SightComponent sight;
    NavMeshAgent agent;
    Animator animator;
    EnemyStateManagerComponent stateManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sight = GetComponent<SightComponent>();
        animator = GetComponent<Animator>();
        stateManager = GetComponent<EnemyStateManagerComponent>();
    }

    public void AttackPlayer()
    {
        bool closeEnoughToPlayerPosition = Vector3.Distance(transform.position, sight.PointAtWhichPlayerIsSeen) <= DistanceToShoot;

        if (!closeEnoughToPlayerPosition)
        {
            animator.SetBool("Shoot", false);
            agent.SetDestination(sight.PointAtWhichPlayerIsSeen);
        }
        else
        {
            if (sight.IsSeeingPlayer)
            {
                RotateTowards(sight.Player.transform.position);
                animator.SetBool("Shoot", true);
                agent.SetDestination(transform.position); // stay where you are
                sight.Player.GetComponent<HealthComponent>().CurrentHealth = 0; // simply kill the character
                stateManager.TransitionToState(AlertnessLevel.NonAlerted);
            }
            else
            {
                stateManager.StateAfterThinking = AlertnessLevel.AlertedSearching;
                stateManager.TransitionToState(AlertnessLevel.Thinking);
            }
        }
    }

    private void RotateTowards(Vector3 targetPoint)
    {
		Vector3 directionToTarget = targetPoint - transform.position;
		directionToTarget.y = 0;

		transform.forward = directionToTarget.normalized;
	}
}
