using UnityEngine;
using UnityEngine.AI;

public class HealthComponent : MonoBehaviour
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
        // and to avoid continuously triggering "IsDead" animation parameter,
        // we use a one-time hasPlayedDeathAnimation variable
        if (!hasPlayedDeathAnimation && CurrentHealth <= 0)
        {
            animator.SetTrigger("IsDead");
            hasPlayedDeathAnimation = true;
        }
    }
}
