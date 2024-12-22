using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownJump : MonoBehaviour
{
    public float jumpForce = 5f; // Jump force
    public float groundCheckDistance = 0.1f; // How close to the ground we check
    public LayerMask groundLayer; // Ground layer for collision detection
    public float gravityScale = 1f; // Gravity scale for jump behavior

    private Rigidbody2D rb;
    private bool isGrounded = false;

    // For customizing Z-axis collisions and preventing X-axis interaction
    public Collider2D zAxisCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is grounded by checking for collisions only on the Z-axis
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // Trigger jump if player presses space and is on the ground
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Apply gravity manually for Y and Z axes
        if (!isGrounded)
        {
            // Apply gravity force manually for both Y and Z axes to simulate natural fall
            rb.velocity += new Vector2(0, -gravityScale * Time.deltaTime); // Gravity on Y-axis
        }
    }

    void Jump()
    {
        // Apply jump force to the Rigidbody2D (we modify both Y and Z velocity)
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Maintain X velocity and apply jump force on Y axis

        // You can also manually apply a jump force to the Z-axis by modifying the Z component of the velocity
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y); // To maintain jump without excessive Y movement
    }

    // Debugging: Visualize the raycast for ground checking
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
    }
}

