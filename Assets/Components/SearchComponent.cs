using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchComponent : MonoBehaviour
{
	HearingComponent hearingComponent;
    EnemyStateManagerComponent stateManager;
    NavMeshAgent agent;
    void Start()
    {
        hearingComponent = GetComponent<HearingComponent>();
        agent = GetComponent<NavMeshAgent>();
        stateManager = GetComponent<EnemyStateManagerComponent>();
    }

    public void SearchNoise()
    {
        agent.SetDestination(hearingComponent.NoisePosition);
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) // reached destination
        {
            stateManager.StateAfterThinking = AlertnessLevel.NonAlerted;
            stateManager.TransitionToState(AlertnessLevel.Thinking);
		}
    }
}