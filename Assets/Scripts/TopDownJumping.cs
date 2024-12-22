using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownJumping : MonoBehaviour
{
    public int terrainWidth = 10; // Dimensions of the terrain
    public int terrainHeight = 10;
    public int terrainDepth = 10;

    public byte[,,] terrain; // 3D byte array for terrain
    public GameObject groundBlockPrefab; // Prefab for ground blocks
    public GameObject player; // Player GameObject

    public Vector3Int playerPosition = new Vector3Int(5, 5, 5); // Player's 3D position
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    private float verticalVelocity = 0f; // Vertical velocity for jumping
    private bool isGrounded = false; // Check if the player is on the ground

    void Start()
    {
        // Initialize terrain
        terrain = new byte[terrainWidth, terrainHeight, terrainDepth];
        GenerateFlatTerrain();

        // Render terrain as 2D blocks
        RenderTerrain();

        // Update the player’s position in 2D
        UpdatePlayerDisplayPosition();
    }

    void Update()
    {
        HandleInput();
        ApplyGravity();
        CheckGroundCollision();
        UpdatePlayerDisplayPosition();
    }

    void GenerateFlatTerrain()
    {
        // Create a flat terrain at height 0
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int z = 0; z < terrainDepth; z++)
            {
                terrain[x, 0, z] = 1; // Set ground level to 1
            }
        }
    }

    void RenderTerrain()
    {
        // Iterate through the 3D terrain array and instantiate blocks for each ground cell
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                for (int z = 0; z < terrainDepth; z++)
                {
                    if (terrain[x, y, z] == 1) // If the block is ground
                    {
                        Vector3 position = new Vector3(x, y + z, 0); // Convert 3D to 2D
                        Instantiate(groundBlockPrefab, position, Quaternion.identity);
                    }
                }
            }
        }
    }

    void HandleInput()
    {
        // Horizontal movement
        if (Input.GetKey(KeyCode.A)) playerPosition.x--;
        if (Input.GetKey(KeyCode.D)) playerPosition.x++;

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce; // Add jump force
            isGrounded = false;
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity
            playerPosition.y += Mathf.FloorToInt(verticalVelocity * Time.deltaTime); // Update Y position
        }
    }

    void CheckGroundCollision()
    {
        // If below terrain or standing on ground block
        if (playerPosition.y <= 0 || terrain[playerPosition.x, playerPosition.y - 1, playerPosition.z] == 1)
        {
            isGrounded = true; // Player is grounded
            verticalVelocity = 0; // Reset vertical velocity
            playerPosition.y = Mathf.Max(playerPosition.y, 1); // Snap to ground level
        }
        else
        {
            isGrounded = false;
        }
    }

    void UpdatePlayerDisplayPosition()
    {
        // Convert 3D position to 2D display position
        Vector3 displayPosition = new Vector3(playerPosition.x, playerPosition.y + playerPosition.z, 0);
        player.transform.position = displayPosition; // Update player’s 2D position
    }
}

