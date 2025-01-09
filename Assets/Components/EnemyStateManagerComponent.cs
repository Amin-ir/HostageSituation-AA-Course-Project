using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStateManagerComponent : MonoBehaviour
{
    public float alertedSpeed = 2f, patrolSpeed = 1f, searchSpeed = 1f, sweepSpeed = 1.5f;

    AlertnessComponent alertnessComponent;
    public AlertnessLevel CurrentState => alertnessComponent.AlertnessLevel;
    public AlertnessLevel StateAfterThinking = AlertnessLevel.Suspicious;

    [SerializeField] Image stateIndicatorImage;
    [SerializeField] Sprite ThinkingIcon, SuspiciousIcon, AlertedIcon, AlertedSearchingIcon;

    SearchComponent searchComponent;
    PatrolComponent patrolComponent;
    CombatComponent combatComponent;
    SweepComponent sweepComponent;
    NavMeshAgent agent;
    Animator animator;

    private float stateStartTime;

    void Start()
    {
        combatComponent = GetComponent<CombatComponent>();
        searchComponent = GetComponent<SearchComponent>();
        patrolComponent = GetComponent<PatrolComponent>();
        sweepComponent = GetComponent<SweepComponent>();
        alertnessComponent = GetComponent<AlertnessComponent>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        TransitionToState(AlertnessLevel.NonAlerted);
    }

    private void Update()
    {
        animator.SetBool("IsMoving", agent.hasPath);
        // Handle state-specific behavior
        switch (alertnessComponent.AlertnessLevel)
        {
            case AlertnessLevel.NonAlerted:
                PatrolBehavior();
                stateIndicatorImage.sprite = null;
                stateIndicatorImage.color = Color.clear;
                break;
            case AlertnessLevel.Suspicious:
                SuspiciousBehavior();
                stateIndicatorImage.sprite = SuspiciousIcon;
                stateIndicatorImage.color = Color.white;
                break;
            case AlertnessLevel.Alerted:
                AlertedBehavior();
                stateIndicatorImage.sprite = AlertedIcon;
                stateIndicatorImage.color = Color.white;
                break;
            case AlertnessLevel.AlertedSearching:
                SearchingBehavior();
                stateIndicatorImage.sprite = AlertedSearchingIcon;
                stateIndicatorImage.color = Color.white;
                break;
            case AlertnessLevel.Thinking:
                SurprisedBehavior();
                stateIndicatorImage.sprite = ThinkingIcon;
                stateIndicatorImage.color = Color.white;
                break;
        }

    }

    public void TransitionToState(AlertnessLevel newState)
    {
        Debug.Log($"Transitioning from {alertnessComponent.AlertnessLevel} to {newState}");

        alertnessComponent.AlertnessLevel = newState;
        stateStartTime = Time.time;

        switch (newState)
        {
            case AlertnessLevel.NonAlerted:
                agent.speed = patrolSpeed; 
                break;
            case AlertnessLevel.Suspicious:
                agent.speed = searchSpeed; 
                break;
            case AlertnessLevel.Alerted:
                agent.speed = alertedSpeed;
                break;
            case AlertnessLevel.AlertedSearching:
                agent.speed = sweepSpeed; 
                break;
            case AlertnessLevel.Thinking:
                agent.SetDestination(agent.transform.position);
                agent.isStopped = true; 
                break;
        }
    }

    private void PatrolBehavior()
    {
        patrolComponent.Patrol();
    }

    private void SuspiciousBehavior()
    {
        searchComponent.SearchNoise();
    }

    private void AlertedBehavior()
    {
        combatComponent.AttackPlayer();
    }

    private void SearchingBehavior()
    {
        sweepComponent.StartSweep();
        sweepComponent.Sweep();
    }

    private void SurprisedBehavior()
    {
        if (Time.time - stateStartTime >= 2.0f) // Pause duration
        {
            TransitionToState(StateAfterThinking);
            agent.isStopped = false; // Resume movement
        }
    }

}
