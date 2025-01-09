using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertnessComponent : MonoBehaviour
{
    public AlertnessLevel AlertnessLevel;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetInteger("AlertLevel", (int)AlertnessLevel);
    }
}