using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerandJump : MonoBehaviour
{
    public float jumpHeight = 1.5f; // Height of the jump
    public float jumpDuration = 0.5f; // Time it takes to complete the jump
    private bool isJumping = false;
    private float jumpTimer = 0f;

    private Vector3 originalScale; // To simulate vertical displacement visually
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
        }

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;

            // Calculate vertical displacement (parabolic arc)
            float progress = jumpTimer / jumpDuration;
            float verticalOffset = Mathf.Sin(Mathf.PI * progress) * jumpHeight;

            // Apply visual jump effect
            transform.localScale = new Vector3(originalScale.x, originalScale.y + verticalOffset, originalScale.z);

            // End jump
            if (jumpTimer >= jumpDuration)
            {
                isJumping = false;
                transform.localScale = originalScale; // Reset scale
            }
        }
    }

}
