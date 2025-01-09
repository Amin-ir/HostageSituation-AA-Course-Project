using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CameraMovementComponent : MonoBehaviour
{
	[SerializeField] private float Speed = 20f;
	[SerializeField] private float zoomSpeed = 10f;
	[SerializeField] private float MaxZoomIn = 3f;
	[SerializeField] private float MaxZoomOut = 5f;
	[SerializeField] private float PlayerFollowSpeed = 5f;
	[SerializeField] private Vector3 PlayerFollowOffset = Vector3.zero;

	Camera cameraComponent;
	NavMeshAgent playerAgent;

	void Start()
	{
		cameraComponent = GetComponent<Camera>();
		playerAgent = FindObjectOfType<SetDestinationComponent>()
					 .GetComponent<NavMeshAgent>();
		// Initial position on player
		transform.position = new Vector3(playerAgent.transform.position.x,
										 transform.position.y,
										 playerAgent.transform.position.z) + PlayerFollowOffset;
	}

	void Update()
    {
		Vector3 move = GetManualCameraMovement();

		if(move != Vector3.zero)
			transform.Translate(move, Space.World);

		ApplyManualZoom();

		if(playerAgent.hasPath) // if player is moving, follow him
			FollowPlayer();
	}

	private void FollowPlayer()
	{
		Vector3 destination = new Vector3(playerAgent.transform.position.x,
									 transform.position.y,
									 playerAgent.transform.position.z);

		transform.position = Vector3.Lerp(transform.position,
									      destination + PlayerFollowOffset,
										  Time.deltaTime * PlayerFollowSpeed);
	}

	private void ApplyManualZoom()
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		cameraComponent.orthographicSize =
			Mathf.Clamp(cameraComponent.orthographicSize - scroll * zoomSpeed,
						MaxZoomIn,
						MaxZoomOut);
	}

	private Vector3 GetManualCameraMovement()
	{
		Vector3 move = Vector3.zero;

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			move.z += Speed * Time.deltaTime;

		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			move.z -= Speed * Time.deltaTime;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			move.x -= Speed * Time.deltaTime;

		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			move.x += Speed * Time.deltaTime;

		return move;
	}
}
