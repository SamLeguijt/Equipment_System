using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Orientation object attached to player gameobject")]
    [SerializeField] private Transform playerOrientation;

    [Tooltip("Reference to the UI slider for adjusting movement speed")]
    [SerializeField] private Slider speedSliderUI;
    
    [Tooltip("Reference to the UI slider for adjusting movement speed")]
    [SerializeField] private Slider dragSliderUI;

    [Tooltip("Speed to move with by the player")]
    [SerializeField ]private float defaultSpeed;

    [Tooltip("Drag of the rigidbody")]
    [SerializeField] private float defaultDrag; 

    // Reference to the rigidbody attached to player
    private Rigidbody rb;
    
    // Ref to the current speed and speed
    private float currentSpeed;
    private float currentDrag;  

    // Variables for player move inputs
    private float horizontalInput;
    private float verticalInput;

    // Vector for storing the direction to move ins
    private Vector3 moveDirection; 

    /// <summary>
    /// Get the current speed of the player
    /// </summary>
    public float CurrentMovementSpeed
    {
        get { return currentSpeed; } 
    }

    /// <summary>
    /// Get the current drag applied to the RB of the player
    /// </summary>
    public float CurrentDrag
    {
        get { return currentDrag; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody and apply rotation freezes
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Set start values if sliders are not null
        if (speedSliderUI != null && dragSliderUI != null)
        {
            speedSliderUI.value = defaultSpeed;
            dragSliderUI.value = defaultDrag; 

            // Add listener to get slider changes
            speedSliderUI.onValueChanged.AddListener(OnSpeedSliderChanged);
            dragSliderUI.onValueChanged.AddListener(OnDragSliderChanged);
        }

        // Set speed to default speed
        currentSpeed = defaultSpeed;
        currentDrag = defaultDrag;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop input and movement if settings menu is open
        if (SettingsManager.instance.IsOpenSettingsMenu()) return;
        
        CheckMoveInput();
        LimitVelocity();
    }


    /// <summary>
    /// Method that changes the current speed of the payer to param
    /// </summary>
    /// <param name="value"></param>
    private void OnSpeedSliderChanged(float _targetSpeed)
    {
        currentSpeed = _targetSpeed;
    }

    /// <summary>
    /// Method that changes the current speed of the payer to param
    /// </summary>
    /// <param name="value"></param>
    private void OnDragSliderChanged(float _targetDrag)
    {
        currentDrag = _targetDrag;
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
        rb.AddForce(moveDirection * currentSpeed, ForceMode.Force);

        // Apply drag to the rigidbody when moving
        rb.drag = currentDrag;
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
            if (flatVelocity.magnitude > currentSpeed)
            {
                // Multiply direction vector (normalized) with movementSpeed scalar 
                Vector3 maxVelocity = flatVelocity.normalized * currentSpeed;

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
