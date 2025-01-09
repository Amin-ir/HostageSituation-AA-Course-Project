using UnityEngine;
using UnityEngine.AI;

public class PatrolComponent : MonoBehaviour
{
	NavMeshAgent agent;
	LineRenderer lineRenderer;

	public Transform[] patrolPoints; 
	public float destinationThreshold = 0.5f;

	[Header("Pause Configuration")]
	[SerializeField] private float pauseDuration = 2f; // Time to pause at each endpoint
	[SerializeField] private bool enableRandomPauseDuration = true;
	[SerializeField] private float randomPauseVariation = 1f; // +/- seconds

	AgentPatrolDirection patrolDirection = AgentPatrolDirection.Going; 
	int currentPatrolPointIndex = 0;

	// New state tracking for pausing
	bool isPausing = false;
	private float pauseStartTime;
	private float currentPauseDuration;

	// if enemy is placed far away from patrol route, should move towards it.
	public bool onRoute = false;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		lineRenderer = GetComponent<LineRenderer>();

		if (patrolPoints.Length > 0)
		{
			currentPatrolPointIndex = 0; 
			agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
		}
	}

    public void Patrol()
    {
        if (patrolPoints.Length == 0) return;

		// If the agent is not already patrolling, set the destination to the closest patrol point
		bool noPath = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
        if (noPath && !isPausing)
        {
            MoveToClosestPatrolPoint();
        }

        // Check if the agent has reached its current patrol point
        bool hasAgentArrivedAtDestination =
            Vector3.Distance(agent.transform.position, patrolPoints[currentPatrolPointIndex].position) < destinationThreshold;

        if (hasAgentArrivedAtDestination)
        {
            if (!isPausing)
            {
                StartPause();
            }
            else
            {
                ContinuePause();
            }
        }
        else if (agent.hasPath) // Ensure the agent continues patrolling
        {
            DrawPath();
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    private void MoveToClosestPatrolPoint()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = currentPatrolPointIndex;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float distance = Vector3.Distance(agent.transform.position, patrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentPatrolPointIndex = closestIndex;
        agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
    }


    void StartPause()
	{
		isPausing = true;
		pauseStartTime = Time.time;

		// Optionally randomize pause duration
		currentPauseDuration = enableRandomPauseDuration
			? pauseDuration + Random.Range(-randomPauseVariation, randomPauseVariation)
			: pauseDuration;

		agent.isStopped = true;
	}

	void ContinuePause()
	{
		// Check if pause duration has elapsed
		if (Time.time - pauseStartTime >= currentPauseDuration)
		{
			// Resume patrolling
			isPausing = false;
			//animator.SetBool("IsIdle", false);
			agent.isStopped = false;

			// Update patrol index and set new destination
			UpdatePatrolIndex();
			agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
		}
	}

	void UpdatePatrolIndex()
	{
		if (patrolDirection == AgentPatrolDirection.Going)
		{
			if (currentPatrolPointIndex < patrolPoints.Length - 1)
				currentPatrolPointIndex++;
			else
				patrolDirection = AgentPatrolDirection.Returning;
		}
		else if (patrolDirection == AgentPatrolDirection.Returning)
		{
			if (currentPatrolPointIndex > 0)
				currentPatrolPointIndex--;
			else
				patrolDirection = AgentPatrolDirection.Going;
		}
	}

	private void DrawPath()
	{
		if (agent.path.corners.Length < 2) return; // No path or at destination

		lineRenderer.positionCount = agent.path.corners.Length;
		lineRenderer.SetPositions(agent.path.corners);
	}
}