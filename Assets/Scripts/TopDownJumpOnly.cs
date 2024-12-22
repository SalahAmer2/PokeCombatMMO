using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownJumpOnly : MonoBehaviour
{
    // Jump-related variables
    public float jumpForce = 5f; // How high the player jumps
    public float gravity = -9.8f; // Downward pull
    private float verticalVelocity = 0f; // Current jump/fall velocity
    private bool isGrounded = true; // Whether the player is on the ground

    // Visual adjustment
    private Vector3 originalPosition; // Original 2D position

    void Start()
    {
        // Save the original position of the player
        originalPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        ApplyGravity();
        UpdateDisplayPosition();
    }

    void HandleInput()
    {
        // Jumping input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce; // Set upward velocity
            isGrounded = false; // Player is no longer grounded
        }
    }

    void ApplyGravity()
    {
        // If not grounded, apply gravity
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // Update velocity
            originalPosition.z += verticalVelocity * Time.deltaTime; // Update height

            // Check if player lands
            if (originalPosition.z <= 0)
            {
                originalPosition.z = 0; // Snap to ground level
                isGrounded = true; // Player is now grounded
                verticalVelocity = 0; // Reset velocity
            }
        }
    }

    void UpdateDisplayPosition()
    {
        // Update the visual position based on height
        // transform.position = new Vector3(originalPosition.x, originalPosition.y - originalPosition.z, 0);
        transform.position = new Vector3(originalPosition.x, originalPosition.y + originalPosition.z, 0); //my code
    }
}
