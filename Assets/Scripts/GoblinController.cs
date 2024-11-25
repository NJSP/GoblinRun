using UnityEngine;
using System.Collections;

public class GoblinController : MonoBehaviour
{
    public float walkSpeed = 5f; // Movement speed
    public float sprintSpeed = 8f; // Sprint speed
    private Rigidbody rb;
    private Vector3 movement;
    private Animator animator;
    private InventoryComponent inventoryComponent;

    private bool isSprinting;
    public float dropItemChance = 0.25f;
    private Coroutine dropItemCoroutine;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        inventoryComponent = GetComponent<InventoryComponent>();
    }

    private void Update()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        movement = new Vector3(moveX, 0, moveZ);


        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            walkSpeed = sprintSpeed;
            if (!isSprinting)
            {
                isSprinting = true;
                dropItemCoroutine = StartCoroutine(DropItemWhileSprinting());
            }
        }
        else
        {
            walkSpeed = 5f;
            if (isSprinting)
            {
                isSprinting = false;
                if (dropItemCoroutine != null)
                {
                    StopCoroutine(dropItemCoroutine);
                    dropItemCoroutine = null;
                }
            }
        }



        // Update Animator parameter
        float speed = movement.magnitude; // 0 when stationary, >0 when moving
        animator.SetFloat("Speed", speed);
        animator.SetBool("isSprinting", isSprinting);
    }

    private void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * walkSpeed;

        // Rotate the player to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * walkSpeed);
        }
    }

    private IEnumerator DropItemWhileSprinting()
    {
        while (isSprinting)
        {
            yield return new WaitForSeconds(2f);

            // 10% chance to drop an item
            if (Random.value <= dropItemChance)
            {
                inventoryComponent.DropLowestValueItem();
            }
        }
    }
}
