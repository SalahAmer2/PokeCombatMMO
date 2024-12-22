using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Notes for later: 
    //top border of area: 
    //if isGrounded, "cannot" pass the top border collider if and !isGrounded "can" pass by disabling collider
    //and if over top border; up movement disabled; by (if) !isGrounded and collider disabled for that specific player (then) up movement disabled
    
    // public float moveSpeed;

    private Animator anim;

    private bool playerMoving;
    private Vector2 lastMove;
    private float playerPkmnLastMove = 1;
    //private float opponentPkmnLastMove;

    ////////////////////////////////////////////////////////////////////
    
    // Movement variables
    public float moveSpeed = 5f;

    // Jump variables
    public float jumpForce = 5f;
    public float gravity = -9.8f;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;

    // Position tracking
    // public Vector3 originalPosition; // only for clarification
    private Vector3 originalPosition; // Tracks x, y, and z positions
    private Transform shadowTransform; // Reference to the shadow for height visualization

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // Initialize the player's original position
        originalPosition = transform.position;

        // Shadow setup (optional: create a shadow object manually and assign it here)
        shadowTransform = transform.Find("Shadow");
        if (shadowTransform == null)
        {
            Debug.LogWarning("No shadow found. Add a child object named 'Shadow' for height visualization.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // HandleMovement();
        HandleJump();
        ApplyGravity();
        UpdateDisplayPosition();

        playerMoving = false;
        //playerPkmnLastMove = 1;
        //opponentPkmnLastMove = -1;

        // HandleMovement() is done by the following
        ////////////////////////////
        if(Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            // transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            originalPosition.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            playerPkmnLastMove = Input.GetAxisRaw("Horizontal");
            //opponentPkmnLastMove = Input.GetAxisRaw("Horizontal");
        }

        if(Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            // transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            originalPosition.y += Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        ////////////////////////////

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        anim.SetFloat("PlayerPkmnLastMove", playerPkmnLastMove);
        //anim.SetFloat("OpponentPkmnLastMove", opponentPkmnLastMove);

        //The following code prevents diagonal movement, but doesn't restrict movement to tileset squares like in Pokemon
        //if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        //{
        //    transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
        //}
        //else if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        //{
        //    transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
        //}
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

        // Custom 2D conversion (using the formula: 2D.y = 3D.y + 3D.z) <- same being done below technically
        transform.position = new Vector3(originalPosition.x, originalPosition.y + originalPosition.z, 0); //my code

        // Adjust shadow position and size if shadow is present
        if (shadowTransform != null)
        {
            shadowTransform.position = new Vector3(originalPosition.x, originalPosition.y, 0);
            float shadowScale = 1f - (originalPosition.z / 10f); // Scale based on height
            shadowTransform.localScale = new Vector3(shadowScale, shadowScale, 1f);
        }
    }

    // Old HandleMovement() 
    // void HandleMovement()
    // {
    //     // Get input for horizontal and vertical movement
    //     float moveX = Input.GetAxis("Horizontal");
    //     float moveY = Input.GetAxis("Vertical");

    //     // Update the player's position (x, y plane)
    //     originalPosition.x += moveX * moveSpeed * Time.deltaTime;
    //     originalPosition.y += moveY * moveSpeed * Time.deltaTime;
    // }
}

// public class PlayerController : MonoBehaviour
// {
//     public float moveSpeed;

//     private Animator anim;

//     private bool playerMoving;
//     private Vector2 lastMove;
//     private float playerPkmnLastMove = 1;
//     //private float opponentPkmnLastMove;

//     // Start is called before the first frame update
//     void Start()
//     {
//         anim = GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         playerMoving = false;
//         //playerPkmnLastMove = 1;
//         //opponentPkmnLastMove = -1;

//         if(Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
//         {
//             transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
//             playerMoving = true;
//             lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
//             playerPkmnLastMove = Input.GetAxisRaw("Horizontal");
//             //opponentPkmnLastMove = Input.GetAxisRaw("Horizontal");
//         }

//         if(Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
//         {
//             transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
//             playerMoving = true;
//             lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
//         }

//         anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
//         anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
//         anim.SetBool("PlayerMoving", playerMoving);
//         anim.SetFloat("LastMoveX", lastMove.x);
//         anim.SetFloat("LastMoveY", lastMove.y);
//         anim.SetFloat("PlayerPkmnLastMove", playerPkmnLastMove);
//         //anim.SetFloat("OpponentPkmnLastMove", opponentPkmnLastMove);

//         //The following code prevents diagonal movement, but doesn't restrict movement to tileset squares like in Pokemon
//         //if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
//         //{
//         //    transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
//         //}
//         //else if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
//         //{
//         //    transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
//         //}
//     }
// }

// public class TopDownMovementAndJump : MonoBehaviour
// {
//     // Movement variables
//     public float moveSpeed = 5f;

//     // Jump variables
//     public float jumpForce = 5f;
//     public float gravity = -9.8f;
//     private float verticalVelocity = 0f;
//     private bool isGrounded = true;

//     // Position tracking
//     private Vector3 originalPosition; // Tracks x, y, and z positions
//     private Transform shadowTransform; // Reference to the shadow for height visualization

//     void Start()
//     {
//         // Initialize the player's original position
//         originalPosition = transform.position;

//         // Shadow setup (optional: create a shadow object manually and assign it here)
//         shadowTransform = transform.Find("Shadow");
//         if (shadowTransform == null)
//         {
//             Debug.LogWarning("No shadow found. Add a child object named 'Shadow' for height visualization.");
//         }
//     }

//     void Update()
//     {
//         HandleMovement();
//         HandleJump();
//         ApplyGravity();
//         UpdateDisplayPosition();
//     }

//     void HandleMovement()
//     {
//         // Get input for horizontal and vertical movement
//         float moveX = Input.GetAxis("Horizontal");
//         float moveY = Input.GetAxis("Vertical");

//         // Update the player's position (x, y plane)
//         originalPosition.x += moveX * moveSpeed * Time.deltaTime;
//         originalPosition.y += moveY * moveSpeed * Time.deltaTime;
//     }

//     void HandleJump()
//     {
//         // Check for jump input and initiate jump if grounded
//         if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//         {
//             verticalVelocity = jumpForce;
//             isGrounded = false;
//         }
//     }

//     void ApplyGravity()
//     {
//         // Apply gravity if the player is not grounded
//         if (!isGrounded)
//         {
//             verticalVelocity += gravity * Time.deltaTime; // Update velocity
//             originalPosition.z += verticalVelocity * Time.deltaTime; // Update vertical position

//             // Ground check
//             if (originalPosition.z <= 0)
//             {
//                 originalPosition.z = 0; // Snap to the ground
//                 verticalVelocity = 0; // Reset velocity
//                 isGrounded = true; // Set grounded status
//             }
//         }
//     }

//     void UpdateDisplayPosition()
//     {
//         // Update the player's visual position
//         // transform.position = new Vector3(originalPosition.x, originalPosition.y - originalPosition.z, 0);
//         transform.position = new Vector3(originalPosition.x, originalPosition.y + originalPosition.z, 0); //my code

//         // Adjust shadow position and size if shadow is present
//         if (shadowTransform != null)
//         {
//             shadowTransform.position = new Vector3(originalPosition.x, originalPosition.y, 0);
//             float shadowScale = 1f - (originalPosition.z / 10f); // Scale based on height
//             shadowTransform.localScale = new Vector3(shadowScale, shadowScale, 1f);
//         }
//     }
// }
