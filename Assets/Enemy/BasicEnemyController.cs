using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float waitTime = 2f;
    public float detectionRadius = 10f;
    public float attackRadius = 2f;
    public float attackCooldown = 1f;
    public LayerMask playerLayer;

    private int currentPatrolIndex;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private bool isWaiting;
    private bool isChasing;
    private bool isAttacking;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);


        if (isAttacking)
        {
            animator.SetTrigger("Attack");
            return;
        }

        if (isChasing)
        {
            ChasePlayer();
            animator.SetBool("Chasing", true);
        }
        else
        {
            Patrol();
        }

        DetectPlayer();


    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        GoToNextPatrolPoint();
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void DetectPlayer()
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (playersInRange.Length > 0)
        {
            player = playersInRange[0].transform;
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    void ChasePlayer()
    {
        if (player == null)
        {
            isChasing = false;
            return;
        }

        agent.destination = player.position;

        if (Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        // Add attack logic here (e.g., reduce player health)
        Debug.Log("Attacking player!");

        yield return new WaitForSeconds(attackCooldown);
        lastAttackTime = Time.time;
        isAttacking = false;

        // Ensure the enemy resumes chasing if the player is still within detection radius
        if (Vector3.Distance(transform.position, player.position) > attackRadius)
        {
            if (Vector3.Distance(transform.position, player.position) > detectionRadius)
            {
                player = null;
                isChasing = false;
            }
            else
            {
                isChasing = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}