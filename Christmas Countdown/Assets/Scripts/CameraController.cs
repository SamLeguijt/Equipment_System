using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Forward orientation object attached to the player object")]
    [SerializeField] private Transform playerForwardOrientation;

    [Header("Sensitivity of the mouse in both directions")]
    [SerializeField] private float mouseSensitivityX;
    [SerializeField] private float mouseSensitivityY;

    [Space]
    [Tooltip("Max angle the camera is allowed to move up and down")]
    [SerializeField] private float maxVerticalAngle = 90f;

    // Private variables used for setting the rotations
    private float rotationX;
    private float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor on start 
        Cursor.lockState = CursorLockMode.Locked;

        // Disable the hardware pointer being visible
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        MaintainPosition();
    }

    /// <summary>
    /// Method for rotating the object according to mouse inputs
    /// Rotates this game object and the player's orientation object. 
    /// </summary>
    private void Rotate()
    {
        // Get mouse inputs
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Get our targets by multiplying with delta time and sensitivity
        float targetX = mouseX * Time.deltaTime * mouseSensitivityX;
        float targetY = mouseY * Time.deltaTime * mouseSensitivityY;

        // Add target values to our player rotation variables
        rotationY += targetX;
        rotationX -= targetY;

        // Clamp the vertical rotation to x degrees
        rotationX = Mathf.Clamp(rotationX, -maxVerticalAngle, maxVerticalAngle);

        // Rotate this object around the target rotations.
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Rotate the player's orientation object around the target rotations.
        playerForwardOrientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    /// <summary>
    /// Method for staying in the correct position
    /// Sets the position to the player's forward orientation position. 
    /// </summary>
    private void MaintainPosition()
    {
        transform.position = playerForwardOrientation.position;
    }
}
