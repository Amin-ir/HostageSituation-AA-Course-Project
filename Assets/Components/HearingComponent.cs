using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AlertnessComponent))]
public class HearingComponent : MonoBehaviour
{
    [SerializeField] private bool ShowHearingRadius = false;
    [SerializeField] private float PauseDuration = 2f;
    public float HearingRadius = 6f;

    NavMeshAgent agent;
    EnemyStateManagerComponent stateManager;
    Collider targetHeardCollider;

	[HideInInspector]
    public Vector3 NoisePosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateManager = GetComponent<EnemyStateManagerComponent>();
        InvokeRepeating(nameof(DetectNoises), 0f, 0.5f);
    }

    private void DetectNoises()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, Vector3.one * HearingRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag(GameTags.NoiseObjectTag) &&
                stateManager.CurrentState < AlertnessLevel.Alerted &&
                collider != targetHeardCollider)
            {
				if (collider.gameObject.GetComponent<AudioSource>().isPlaying)
				{
                    targetHeardCollider = collider;
					NoisePosition = collider.transform.position;
                    stateManager.StateAfterThinking = AlertnessLevel.Suspicious;
                    stateManager.TransitionToState(AlertnessLevel.Thinking);
				}
			}
        }
    }

	void OnDrawGizmos()
    {
        if (ShowHearingRadius)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, HearingRadius);
        }
    }
}