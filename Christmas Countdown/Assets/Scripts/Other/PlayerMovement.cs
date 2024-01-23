using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Orientation object attached to player gameobject")]
    [SerializeField] private Transform playerOrientation;

    [Tooltip("Speed to move with by the player")]
    [SerializeField ]private float movementSpeed;

    [Tooltip("Drag of the rigidbody")]
    [SerializeField] private float speedDrag; 

    // Reference to the rigidbody attached to player
    private Rigidbody rb;

    // Variables for player move inputs
    private float horizontalInput;
    private float verticalInput;

    // Vector for storing the direction to move ins
    private Vector3 moveDirection; 

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody and apply rotation freezes
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsManager.instance.panelSettingsUI.activeSelf) return;
        
        CheckMoveInput();
        LimitVelocity();
    }

    /// <summary>
    /// Method for moving the player
    /// Adds force and drag to the rigidbody, depending on the received input
    /// </summary>
    private void Move()
    {
        // Calculate move direction by getting the orientation axis' * relevant input
        moveDirection = ((playerOrientation.forward * verticalInput) + (playerOrientation.right * horizontalInput)).normalized; // Normalize for direction

        // Add force to the rigidbody in the direction with certain speed, using a continuous force
        rb.AddForce(moveDirection * movementSpeed, ForceMode.Force);

        // Apply drag to the rigidbody when moving
        rb.drag = speedDrag;
    }

    /// <summary>
    /// Method that limits the velocity to a maximum
    /// Checks if velocity is > 0, than limits it if magnitude is bigger than movementSpeed var
    /// Returns if stationary
    /// </summary>
    private void LimitVelocity()
    {
        // Prevent running code if player is not moving
        if (rb.velocity.magnitude > 0)
        {
            // Get the flat velocity by not looking at the y component
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.y);

            // Check if the length of the vector is bigger than the movementSpeed
            if (flatVelocity.magnitude > movementSpeed)
            {
                // Multiply direction vector (normalized) with movementSpeed scalar 
                Vector3 maxVelocity = flatVelocity.normalized * movementSpeed;

                // Set the new max velocity when exceeding the movementSpeed 
                rb.velocity = new Vector3(maxVelocity.x, rb.velocity.y, maxVelocity.z);
            }
        }
        // Return if player is stationary
        else return;
    }

    /// <summary>
    /// Method that checks for player input
    /// Calls Move function if receiving input
    /// </summary>
    private void CheckMoveInput()
    {
        // Get the input for horizontal and vertical axis'
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Use Mathf function to check if either input is received
        if (Mathf.Abs(horizontalInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f)
        {
            // Call method to move when receiving input
            Move(); 
        }
    }
}
