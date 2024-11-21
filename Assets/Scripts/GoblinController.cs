using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    private Rigidbody rb;
    private Vector3 movement;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        movement = new Vector3(moveX, 0, moveZ);

        // Update Animator parameter
        float speed = movement.magnitude; // 0 when stationary, >0 when moving
        animator.SetFloat("Speed", speed);
        //Debug.Log("Speed: " + speed);
        Debug.Log(animator.GetFloat("Speed"));
    }

    private void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * moveSpeed;

        // Rotate the player to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }
}