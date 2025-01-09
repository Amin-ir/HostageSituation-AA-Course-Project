using UnityEngine;
using UnityEngine.AI;

public class HeathComponent : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;

    bool hasPlayedDeathAnimation = false;
    
    Animator animator;
    NavMeshAgent agent;
    
    void Start()
    {
        CurrentHealth = MaxHealth;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // because of using 'AnyState' we use trigger.
        if (!hasPlayedDeathAnimation && CurrentHealth <= 0)
        {
            animator.SetTrigger("IsDead");
            hasPlayedDeathAnimation = true;
        }


    }
}
