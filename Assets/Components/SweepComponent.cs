using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SweepComponent : MonoBehaviour
{
    public float sweepRadius = 10f;          // Radius of the sweep area
    public float sweepDuration = 10f;       // Total sweep duration
    NavMeshAgent agent;
    SightComponent sight;
    private float sweepStartTime;
    private Vector3 currentTarget;

    private void Start()
    {
        sight = GetComponent<SightComponent>();
        agent = GetComponent<NavMeshAgent>();
    }

    private enum SweepState
    {
        Idle,
        MovingToPoint
    }

    private SweepState currentState = SweepState.Idle;

    public void StartSweep()
    {
        sweepStartTime = Time.time;
        currentState = SweepState.Idle;
    }

    public void StopSweep()
    {
        currentState = SweepState.Idle;
        agent.isStopped = true;
    }

    public void Sweep()
    {
        if (Time.time - sweepStartTime >= sweepDuration)
        {
            // Sweep complete; reset state
            currentState = SweepState.Idle;
            Debug.Log("Sweep completed.");
            return;
        }

        switch (currentState)
        {
            case SweepState.Idle:
                SetNextTarget();
                break;

            case SweepState.MovingToPoint:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = SweepState.Idle;
                }
                break;
        }
    }

    private void SetNextTarget()
    {
        Vector3 centerPoint = sight.PointAtWhichPlayerIsSeen;
        // Generate a random point within the sweep radius
        Vector3 randomPoint = centerPoint + Random.insideUnitSphere * sweepRadius;
        randomPoint.y = centerPoint.y; // Keep the point on the same Y-level
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, sweepRadius, NavMesh.AllAreas))
        {
            currentTarget = hit.position;
            agent.SetDestination(currentTarget);
            currentState = SweepState.MovingToPoint;
        }
    }
}