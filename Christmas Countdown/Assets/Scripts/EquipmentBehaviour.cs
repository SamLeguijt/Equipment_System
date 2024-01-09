using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.XR;

/// <summary>
/// Public class for behaviour tasks of Equipments. <br/>
/// Uses a ref to a scriptable object for it's equipment data, and reference to EquipmentSystemController for equip and drop actions
/// </summary>
public class EquipmentBehaviour : MonoBehaviour
{
    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Space]
    [Tooltip("Drag in object with the EquipmentSystemController attached")]
    [SerializeField] private EquipmentSystemController equipmentSystemController;

    [Space]
    [Tooltip("Player object in scene, used for calculating equip distance")]
    [SerializeField] private Transform player;

    [Space]
    [Header("Equipment specifics")]
    [Tooltip("Scriptable Object with this object's data")]
    [SerializeField] private BaseEquipmentObject equipmentData;

    [Tooltip("Name of the layer for environmental objects")]
    [SerializeField] private string environmentLayerName;

    //Reference to the rigidbody for physics
    private Rigidbody rb;

    [SerializeField] private Collider colliderToCheck;

    // Bools for checking status of this object, used for properties
    private bool isEquipped;
    private bool canDrop;
    private bool isOnGround;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Public read only property of the EquipmentSystemController
    /// </summary>
    public EquipmentSystemController EquipmentSystemController
    {
        get { return equipmentSystemController ?? throw new System.NullReferenceException("EquipmentSystemController is not assigned!"); }
    }

    /// <summary>
    /// Public property with private set to get access to object's data without changing it (Scriptable Object ref)
    /// </summary>
    public BaseEquipmentObject EquipmentData
    {
        get { return equipmentData ?? throw new System.NullReferenceException("No Scriptable Object assigned!"); }
        private set { equipmentData = value; }
    }

    /// <summary>
    /// Private player property used for calculating distance from this object to player
    /// </summary>
    private Transform Player
    {
        // Get player if not null, else throw null reference exception with message
        get { return player ?? throw new System.NullReferenceException("Player is not assigned!"); }
    }

    /// <summary>
    /// Public property with private set to prevent setting its value outside this class <br/><br/>
    /// Used for determining if this object is currently equipped by player
    /// </summary>
    public bool IsEquipped
    {
        get { return isEquipped; }
        private set { isEquipped = value; }
    }

    /// <summary>
    /// Public property with private set to prevent setting its value outside of class <br/><br/>
    /// Used for determining if an object can be dropped 
    /// </summary>
    public bool CanDrop
    {
        get { return canDrop; }
        private set { canDrop = value; }
    }

    /// <summary>
    /// Public property to check if the object is on ground
    /// </summary>
    public bool IsOnGround
    {
        get { return isOnGround; }
        private set { isOnGround = value; }
    }

    /* ------------------------------------------  METHODS ------------------------------------------- */

    /* TODO THIS BRANCH :
     * Make HandleCollision method that uses switch statement to handle different kind of colliders
     * Physics.Overlap <- collider type
     * foreach collider in ^^ { 
     * if layer = 6 -> collision with ground 
     * IsOnGround true
     * Test everything
     * 
     * if it all works:
     * clean up code, add private collider and drag parent in inspector
     * Add rigidbody to collider in start
     * Make parent object a child of hand instead of this. 
     * Rotate parent object to correct thing
     * MAYBE: 
     * Add sphere collider to this object, then get the IsMouseOver collision for that collider if IsOnGround !IsEquipped 
     * Sphere collider should be trigger, to prevent colliding with ground
     * 
     * If all works good, make empty gameobject, reset position and add the sphere collider with this script to it. 
     * Add correct layer and make a prefab 
     * Maybe add some things: auto layering in start for parent obj, set correct position (parent pos)
     * Make sure position and rotation of equipment object gets reset to equipmentData properties on Equip.
     */

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        rb = GetComponent<Rigidbody>();

        // Set rotation of this object to the data's values
        SetRotation(EquipmentData.UnequippedRotation);

        HandleCollision(colliderToCheck, 1);
    }

    /// <summary>
    /// Method to set the rotation of the model object 
    /// </summary>
    /// <param name="_targetRotation"></param>
    private void SetRotation(Vector3 _targetRotation)
    {
        transform.rotation = Quaternion.Euler(_targetRotation);
    }

    /// <summary>
    /// Public method called when equipping this object to hand <br/>
    /// Repositions object to hand, sets parent, and sets IsEquipped and CanDrop bool values true
    /// </summary>
    public void OnEquip(Hand _targetHand)
    {
        // Set position and parent to hand object
        transform.position = _targetHand.transform.position;
        gameObject.transform.SetParent(_targetHand.transform, true);
        SetRotation(EquipmentData.EquippedRotation);

        // Set value of bools true
        IsEquipped = true;
        IsOnGround = false;
        rb.isKinematic = true;

        // Start coroutine to set value of CanDrop bool after this frame ends
        StartCoroutine(EnableDropAfterFrame());
    }

    /// <summary>
    /// Public method called when dropping this equipment from hand <br/>
    /// Removes hand as parent, repositions object and sets IsEquipped and CanDrop bool values to false
    /// </summary>
    public void OnDrop(Hand _ownerHand)
    {
        // Set rotation back 
        SetRotation(EquipmentData.UnequippedRotation);

        // Set values of booleans
        IsEquipped = false;
        CanDrop = false;
        rb.isKinematic = false;

        // Call method to throw equipment
        ThrowEquipment(); // Note: Notice isKinematic = false before calling method

        // Set position and parent 
        transform.parent = null;
        transform.position = _ownerHand.transform.position;
    }

    private void Update()
    {
        // Return if the collider is not being targeted by the mouse, or if the player is not within equipdistance
        if (!IsMouseOverCollider(colliderToCheck) || IsWithinEquipRange())
        {
            Debug.Log("Return ");

            return;
        }
        else // Mouse is over equipment, and player is within equiprange
        {
            Debug.Log("Else "); 
            // Check if the item is not equipped yet, and if it's not in the air
            if (!IsEquipped && IsOnGround)
            {
                Debug.Log("TryEquip ");

                // Send message that 
                equipmentSystemController.TryEquip(this);
            }
        }

    }

    private bool IsMouseOverCollider(Collider colliderToCheck)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast a ray from the mouse position
        if (Physics.Raycast(ray, out hit) && hit.collider == colliderToCheck)
        {
            // Mouse is over the specified collider
            return true;
        }

        // Mouse is not over the specified collider
        return false;
    }
    /// <summary>
    /// Method that throws the equipment from hand with a certain force 
    /// </summary>
    private void ThrowEquipment()
    {
        // Set object rigidbody to the player's velocity to carry momentum
        rb.velocity = GetPlayerVelocity();

        // Get the dropForce vector
        Vector3 dropForce = GetDropForceVector();

        // Apply the dropforce using Impulse forcemode to throw equipment 
        rb.AddForce(dropForce, ForceMode.Impulse);
    }

    /// <summary>
    /// IEnumerator that sets the value of CanDrop to true after end of frame <br/>
    /// Prevents dropping equipment same frame as equipping it
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableDropAfterFrame()
    {
        // Wait for end of frame
        yield return new WaitForEndOfFrame();

        // Set value to true to enable dropping this object
        CanDrop = true;
    }

    /// <summary>
    /// OnCollisionEnter method to detect collision with ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(environmentLayerName))
        {
            IsOnGround = true;
        }
    }

    private void HandleCollision(Collider collider1, LayerMask _layer)
    {
        // Handle collision with different type of colliders 
        switch (collider1)
        {
            case BoxCollider:
                // Your custom logic for handling collisions with a BoxCollider goes here

                Debug.Log("Box");
                break;
            case SphereCollider:
                // Your custom logic for handling collisions with a SphereCollider goes here
                break;
            case CapsuleCollider:
                // Your custom logic for handling collisions with a CapsuleCollider goes here
                break;
            // Add more cases for other collider types as needed
            case Collider:
                // Your generic custom logic for handling collisions with any collider goes here
                break;
            default:
                Debug.Log("Collided with an unknown collider type: " + collider1.gameObject.name);
                break;
        }
    }

    /// <summary>
    /// Pubic bool method that returns true if this object is within { equip distance } of player
    /// </summary>
    /// <returns></returns>
    public bool IsWithinEquipRange()
    {
        return (DistanceFromPlayer() < equipmentData.EquipDistance);
    }

    /// <summary>
    /// Calculates and returns the drop force vector based on the player's orientation.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private Vector3 GetDropForceVector()
    {
        // Get the camera controller scipt to access the orientation of the player
        CameraController cameraController = Camera.main.GetComponent<CameraController>();

        // Check if found
        if (cameraController != null)
        {
            // Get the forward and upwards directions by looking at the player's orientation 
            Vector3 forwardDirection = cameraController.PlayerOrientation.forward;
            Vector3 upwardDirection = cameraController.PlayerOrientation.up;

            // Multiply the direction components with our dropForce vector components to apply force in correct directions
            Vector3 dropForceVector = forwardDirection * EquipmentData.EquipmentDropForce.x + upwardDirection * EquipmentData.EquipmentDropForce.y;

            // Return the calculated vector
            return dropForceVector;
        }
        else // CameraController not found on main camera
        {
            throw new System.Exception("CameraController is not found on main camera, cannot calculate drop force.");
        }
    }

    /// <summary>
    /// Returns the velocity of the player if rigidbody is found
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private Vector3 GetPlayerVelocity()
    {
        // Get player rigidbody
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        // Check if found
        if (playerRb != null)
        {
            // Return it's velocity if found
            return playerRb.velocity;
        }
        else // No rigidbody found on player transform
        {
            throw new System.Exception("Player rigidbody not found, cannot get player velocity");
        }
    }
    /// <summary>
    /// Private flaot method that returns the distance between the player and this object
    /// </summary>
    /// <returns></returns>
    private float DistanceFromPlayer()
    {
        return Vector3.Distance(Player.position, transform.position);
    }
}
