using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TossingComponent : MonoBehaviour
{
    public int ObjectCount;
    [SerializeField] private bool ShowTossRadius = false;
    [SerializeField] private GameObject throwablePrefab;
    [SerializeField] private float LimitRadius;
    [SerializeField] private float throwAngle = 45f; // Angle in degrees
    [SerializeField] private Transform throwStartPoint; // Point at which object is thrown from
    [SerializeField] private Vector3 throwableRotation;
    [SerializeField] private TextMeshProUGUI remainingObjectIndicator;
    [SerializeField] private GameObject tossingRangeIndicatorPrefab;

    NavMeshAgent navMeshAgent;
    Camera mainCamera;
    Animator animator;

    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        UpdateRemainingObjectUI();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && ObjectCount > 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit groundHit)) // clicked a ground?
            {
                if (Vector3.Distance(groundHit.point, transform.position) <= LimitRadius)
                {
                    // Stop the player
                    navMeshAgent.destination = transform.position;
                    RotateTowards(groundHit.point);
                    ThrowObjectTowards(groundHit.point);
                    animator.SetTrigger("Throw");
                    ObjectCount--;
                    UpdateRemainingObjectUI();
                }
                else
                {
                    InstantiateTossingRangeIndicator();
                    //Debug.Log("**** cannot throw item here!");
                }

            }
        }
    }

    private void RotateTowards(Vector3 targetPoint)
    {
        Vector3 directionToTarget = targetPoint - transform.position;
        directionToTarget.y = 0; 

        transform.forward = directionToTarget.normalized;
    }

    private void ThrowObjectTowards(Vector3 targetPoint)
    {
        GameObject throwable = Instantiate(throwablePrefab, throwStartPoint.position, Quaternion.identity);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 velocity = CalculateVelocity(targetPoint, throwStartPoint.position, throwAngle);
            rb.velocity = velocity;
            // Object Rotation 
            rb.AddTorque(throwableRotation, ForceMode.Impulse);
        }
    }

    private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float angle)
    {
        float gravity = Physics.gravity.y;
        float radianAngle = angle * Mathf.Deg2Rad;

        // Calculate the horizontal and vertical distances
        float distance = Vector3.Distance(new Vector3(target.x, 0, target.z), new Vector3(origin.x, 0, origin.z));
        float heightDifference = target.y - origin.y;

        // Calculate the initial velocity magnitude
        float velocity = Mathf.Sqrt(distance * Mathf.Abs(gravity) / Mathf.Sin(2 * radianAngle));

        // Calculate the velocity components
        Vector3 direction = (target - origin).normalized;
        Vector3 horizontalVelocity = direction * Mathf.Cos(radianAngle) * velocity;
        Vector3 verticalVelocity = Vector3.up * Mathf.Sin(radianAngle) * velocity;

        return horizontalVelocity + verticalVelocity;
    }

    private void UpdateRemainingObjectUI()
    {
        remainingObjectIndicator.text = ObjectCount.ToString(); 
    }

    public void InstantiateTossingRangeIndicator()
    {
        var tossingRangeIndicator = Instantiate(tossingRangeIndicatorPrefab,
                                                transform.position,
                                                Quaternion.Euler(new Vector3(90, 0, 0)));
        var rangeSprite = tossingRangeIndicator.GetComponent<SpriteRenderer>();
        float spriteWidth = rangeSprite.sprite.bounds.size.x;
        float desiredScale = (LimitRadius * 2) / spriteWidth;
        tossingRangeIndicator.transform.localScale = new Vector3(desiredScale, desiredScale, 1f);
    }

	private void OnDrawGizmos()
	{
        if (ShowTossRadius)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, LimitRadius);
        }
	}
}
