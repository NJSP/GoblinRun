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
    public float searchDuration = 10f; // Duration to look around at the last known location
    public float searchPointDistance = 5f; // Distance to move in the player's forward direction


    private int currentPatrolIndex;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private VisionCone visionCone;
    private EnemyAttackController attackController;
    private bool isWaiting;
    private bool isChasing;
    private bool isAttacking;
    private float lastAttackTime;
    private bool isSearching;
    private bool isSearchingCoroutineRunning;
    private Vector3 lastKnownPlayerPosition;
    private Vector3 lastKnownPlayerForward;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        visionCone = GetComponent<VisionCone>();
        attackController = GetComponent<EnemyAttackController>();
        currentPatrolIndex = 0;
        isChasing = false;
        isAttacking = false;
        isSearching = false;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (isAttacking)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                isAttacking = false;
            }
            else
            {
                return;
            }
        }

        if (isChasing)
        {
            ChasePlayer();
            animator.SetBool("Chasing", true);
        }
        else if (isSearching)
        {
            if (!isSearchingCoroutineRunning)
            {
                StartCoroutine(SearchForPlayer());
            }
            animator.SetBool("Chasing", false);
        }
        else
        {
            Patrol();
            animator.SetBool("Chasing", false);
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
        if (!isChasing && visionCone.IsPlayerInSight)
        {
            Collider[] playersInRange = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
            foreach (var playerCollider in playersInRange)
            {
                if (playerCollider.CompareTag("Player"))
                {
                    player = playerCollider.transform;
                    lastKnownPlayerPosition = player.position; // Update last known player position
                    lastKnownPlayerForward = player.forward; // Update last known player forward direction
                    isChasing = true;
                    isSearching = false;
                    return;
                }
            }
        }
        else if (isChasing)
        {
            Collider[] playersInRange = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
            bool playerDetected = false;
            foreach (var playerCollider in playersInRange)
            {
                if (playerCollider.CompareTag("Player"))
                {
                    player = playerCollider.transform;
                    playerDetected = true;
                    break;
                }
            }

            if (!playerDetected)
            {
                isChasing = false;
                isSearching = true;
                StartCoroutine(SearchForPlayer());
            }
        }
        else
        {
            player = null;
        }
    }

    void ChasePlayer()
    {
        if (player == null)
        {
            isChasing = false;
            isSearching = true;
            StartCoroutine(SearchForPlayer());
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

    IEnumerator SearchForPlayer()
    {
        isSearchingCoroutineRunning = true;

        // Pick a point within a cone pointing towards the player's last known location
        Vector3 directionToPlayer = (lastKnownPlayerPosition - transform.position).normalized;
        float angle = Random.Range(-45f, 45f); // Cone angle
        Vector3 searchDirection = Quaternion.Euler(0, angle, 0) * directionToPlayer;
        Vector3 searchPoint = lastKnownPlayerPosition + searchDirection * searchPointDistance;

        NavMeshHit hit;
        NavMesh.SamplePosition(searchPoint, out hit, searchPointDistance, 1);
        Vector3 finalPosition = hit.position;

        agent.destination = finalPosition;

        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            // Rotate to face the direction of movement
            if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
            }
            yield return null;
        }

        // Look around for a while
        float searchEndTime = Time.time + searchDuration;
        while (Time.time < searchEndTime)
        {
            // Smoothly rotate between 30 and 90 degrees to the left
            float leftAngle = Random.Range(30f, 90f);
            Quaternion leftRotation = Quaternion.Euler(0, -leftAngle, 0) * transform.rotation;
            while (Quaternion.Angle(transform.rotation, leftRotation) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, leftRotation, 120 * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);

            // Smoothly rotate between 90 and 150 degrees to the right
            float rightAngle = Random.Range(90f, 150f);
            Quaternion rightRotation = Quaternion.Euler(0, rightAngle, 0) * transform.rotation;
            while (Quaternion.Angle(transform.rotation, rightRotation) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rightRotation, 120 * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);

            // Pick a random point within 10 units and move to that location
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
            finalPosition = hit.position;

            agent.destination = finalPosition;

            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                // Rotate to face the direction of movement
                if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
                }
                yield return null;
            }

            // Check if the player is in sight
            if (visionCone.IsPlayerInSight)
            {
                isSearching = false;
                isChasing = true;
                isSearchingCoroutineRunning = false;
                yield break;
            }
        }

        // If the player is not found, resume patrolling
        isSearching = false;
        isSearchingCoroutineRunning = false;
        GoToNextPatrolPoint();
    }

    IEnumerator AttackPlayer()
    {
        if (player == null)
        {
            yield break;
        }
        isAttacking = true;
        lastAttackTime = Time.time; // Update last attack time here
        // Add attack logic here (e.g., reduce player health)
        attackController.PerformRandomAttack();
        //player.GetComponent<HealthComponent>().TakeDamage();

        yield return new WaitForSeconds(attackCooldown);
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