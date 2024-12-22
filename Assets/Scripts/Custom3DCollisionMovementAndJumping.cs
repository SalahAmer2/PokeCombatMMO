using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom3DCollisionMovementAndJumping : MonoBehaviour
{
    // Terrain dimensions and the terrain byte array
    public int width = 10;
    public int height = 10;
    public int depth = 10;
    public byte[,,] terrain;

    // Player's position in 3D space
    public Vector3 position3D;

    // Player movement speed in 3D space
    public float moveSpeed = 5f;

    // Jumping and gravity values
    public float jumpForce = 5f;
    public float gravity = -9.8f;
    private float velocityY = 0f;  // Y-axis velocity for jumping

    // Ground detection
    public float groundLevel = 0f;  // The Y position of the ground in 3D
    private bool isGrounded = false;

    void Start()
    {
        // Initialize the terrain as a 3D byte array
        terrain = new byte[width, height, depth];

        // Fill the terrain with random data for testing (1 = ground, 0 = air)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    // Simple terrain generation: 1 is ground, 0 is air
                    if (y == 0) terrain[x, y, z] = 1; // Ground at y = 0
                    else terrain[x, y, z] = 0; // Air otherwise
                }
            }
        }
    }

    void Update()
    {
        // Handle 3D movement (X, Y, and Z axes)
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Handle Jumping (affects Y and Z axis in 3D)
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // Spacebar to jump
        {
            velocityY = jumpForce; // Apply initial jump force
            // velocityY += gravity * Time.deltaTime;
        }

        // Apply gravity (affecting Y-axis)
        if (!isGrounded)
        {
            velocityY += gravity * Time.deltaTime;
        }
        else
        {
            velocityY = 0f; // Reset Y velocity if on the ground
        }

        // Update player position in 3D space (including jumping)
        position3D += new Vector3(moveX, velocityY * Time.deltaTime, moveZ);

        // Check if the player is on the ground (colliding with terrain)
        isGrounded = IsCollidingWithTerrain(position3D);

        // Convert the 3D position to 2D for display (using the formula: 2D.y = 3D.y + 3D.z)
        Vector2 displayPosition = ConvertTo2D(position3D);

        // Update the GameObject's position in the scene (using 2D for display)
        transform.position = displayPosition;

        // Debug: visualize the player's 3D position
        Debug.DrawLine(new Vector3(position3D.x, 0, position3D.z), new Vector3(position3D.x, 10, position3D.z), Color.green);
    }

    // Check if the player is colliding with terrain (simple collision check)
    bool IsCollidingWithTerrain(Vector3 position)
    {
        // Convert position to integer grid coordinates
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        // Ensure we are within the bounds of the terrain
        // if (x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth)
        // if (y >= 0) in bounds
        // if (y <= 0) index out of bounds error
        {
            // Check if there's a solid block (1 = ground, etc.)
            return terrain[x, y, z] != 0; // Non-zero indicates collision
        }

        return false; // No collision if out of bounds
    }

    // Convert 3D position to 2D (using the formula: 2D.y = 3D.y + 3D.z)
    Vector2 ConvertTo2D(Vector3 position3D)
    {
        // Custom 2D conversion (using the formula: 2D.y = 3D.y + 3D.z)
        float convertedY = position3D.y + position3D.z;
        return new Vector2(position3D.x, convertedY);
    }
}
