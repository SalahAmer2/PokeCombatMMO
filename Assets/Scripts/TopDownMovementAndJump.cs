using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovementAndJump : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;

    // Jump variables
    public float jumpForce = 5f;
    public float gravity = -9.8f;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;

    // Position tracking
    private Vector3 originalPosition; // Tracks x, y, and z positions
    private Transform shadowTransform; // Reference to the shadow for height visualization

    void Start()
    {
        // Initialize the player's original position
        originalPosition = transform.position;

        // Shadow setup (optional: create a shadow object manually and assign it here)
        shadowTransform = transform.Find("Shadow");
        if (shadowTransform == null)
        {
            Debug.LogWarning("No shadow found. Add a child object named 'Shadow' for height visualization.");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        UpdateDisplayPosition();
    }

    void HandleMovement()
    {
        // Get input for horizontal and vertical movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Update the player's position (x, y plane)
        originalPosition.x += moveX * moveSpeed * Time.deltaTime;
        originalPosition.y += moveY * moveSpeed * Time.deltaTime;
    }

    void HandleJump()
    {
        // Check for jump input and initiate jump if grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }
    }

    void ApplyGravity()
    {
        // Apply gravity if the player is not grounded
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // Update velocity
            originalPosition.z += verticalVelocity * Time.deltaTime; // Update vertical position

            // Ground check
            if (originalPosition.z <= 0)
            {
                originalPosition.z = 0; // Snap to the ground
                verticalVelocity = 0; // Reset velocity
                isGrounded = true; // Set grounded status
            }
        }
    }

    void UpdateDisplayPosition()
    {
        // Update the player's visual position
        // transform.position = new Vector3(originalPosition.x, originalPosition.y - originalPosition.z, 0);
        transform.position = new Vector3(originalPosition.x, originalPosition.y + originalPosition.z, 0); //my code

        // Adjust shadow position and size if shadow is present
        if (shadowTransform != null)
        {
            shadowTransform.position = new Vector3(originalPosition.x, originalPosition.y, 0);
            float shadowScale = 1f - (originalPosition.z / 10f); // Scale based on height
            shadowTransform.localScale = new Vector3(shadowScale, shadowScale, 1f);
        }
    }
}
