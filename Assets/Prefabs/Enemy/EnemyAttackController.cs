using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    public string[] attackTriggers; // List of animation trigger names
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // Example triggers if not set in the Inspector
        if (attackTriggers.Length == 0)
        {
            attackTriggers = new string[] { "Attack1", "Attack2", "Attack3" };
        }
    }

    public void PerformRandomAttack()
    {
        // Select a random trigger from the list
        string randomTrigger = attackTriggers[Random.Range(0, attackTriggers.Length)];

        // Trigger the selected animation
        animator.SetTrigger(randomTrigger);

        Debug.Log($"Enemy performs attack: {randomTrigger}");
    }
}
