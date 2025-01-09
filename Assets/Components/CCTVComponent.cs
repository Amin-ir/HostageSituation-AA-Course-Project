using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CCTVComponent : MonoBehaviour
{
	[SerializeField] private Color alertColor;
	[SerializeField] private float alertingRadius;
	[SerializeField] private float alertDuration = 10;
	[SerializeField] private bool showRadius = false;

	private MeshRenderer sight;
	private Color meshDefaultColor;

	Animator animator;
	AudioSource audio;
	Vector3 PlayerDetectionPoint;
	float alertStartTime = 0;

	private void Start()
	{
		sight = GetComponentInChildren<Collider>().gameObject
			   .GetComponent<MeshRenderer>();

		meshDefaultColor = sight.materials[0].color;

		animator = GetComponent<Animator>();

		audio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Time.time - alertStartTime >= alertDuration)
			DeactiveAlarm();
	}

	private void DeactiveAlarm()
	{
		audio.Stop();
		animator.enabled = true;
		sight.materials[0].color = meshDefaultColor;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(GameTags.PlayerTag))
		{
			PlayerDetectionPoint = other.gameObject.transform.position;
			sight.materials[0].color = alertColor;
			animator.enabled = false;
			audio.Play();
			alertStartTime = Time.time;
			InformCloseAgents();
		}
	}

	private void InformCloseAgents()
	{
		var closeEnemies = FindObjectsOfType<EnemyStateManagerComponent>()
													.Where(agent => Vector3.Distance(agent.transform.position,
																					 transform.position) <= alertingRadius);

		foreach(var enemy in closeEnemies)
		{
			enemy.GetComponent<SightComponent>().PointAtWhichPlayerIsSeen = PlayerDetectionPoint;
			enemy.StateAfterThinking = AlertnessLevel.Alerted;
			enemy.TransitionToState(AlertnessLevel.Thinking);
		}
	}

	void OnDrawGizmos()
	{
        if (showRadius)
        {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, alertingRadius);	
        }
	}
}
