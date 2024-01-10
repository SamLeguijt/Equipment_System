using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.XR;
using Unity.VisualScripting.Antlr3.Runtime;

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
    [Tooltip("Name of the layer for environmental objects")]
    [SerializeField] private string environmentLayerName;

    [Tooltip("Name of the layer for equipment objects")]
    [SerializeField] private string equipmentLayerName;

    [Space]
    [Header("Equipment specifics")]
    [Space]

    [Tooltip("Model of equipment as parent of this object")]
    [SerializeField] private GameObject mainEquipmentObject;

    [Tooltip("Scriptable Object with this object's data")]
    [SerializeField] private BaseEquipmentObject equipmentData;

    /* --- PRIVATE HIDDEN VARIABLES ---*/
    private EquipmentPhysicsManager equipmentPhysicsManager; // Reference to this object's physics manager
    private Collider parentCollider; // Store collider of the parent object
    private Transform player; // Reference to the player for distance and orientation

    // Bools for checking status of this object, used for properties
    private bool isEquipped; // Status of this object
    private bool canDrop; // Flag used for checking if object can be dropped
    private bool isOnGround; // Flag to determine if the equipment is grounded


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /// <summary>
    /// Public read only property of the EquipmentSystemController
    /// </summary>
    public EquipmentSystemController EquipmentSystemController
    {
        get { return equipmentSystemController ?? throw new System.NullReferenceException("EquipmentSystemController is not assigned!"); }
    }

    /// <summary>
    /// Read only property to get the name of the environment layer
    /// </summary>
    public string EnvironmentLayerName
    {
        get { return environmentLayerName; }
    }

    /// <summary>
    /// Read only property for the parent object
    /// </summary>
    public GameObject MainEquipmentObject
    {
        get { return mainEquipmentObject; }
        private set { mainEquipmentObject = value; }
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
    /// Public read-only property for the physics manager
    /// </summary>
    public EquipmentPhysicsManager EquipmentPhysicsManager
    {
        get { return equipmentPhysicsManager; }
    }

    /// <summary>
    /// Public read-only property to get info about parent's collider
    /// </summary>
    public Collider ParentCollider
    {
        get { return parentCollider; }
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
    /// Public property to check or set if the object is on ground
    /// </summary>
    public bool IsOnGround
    {
        get { return isOnGround; }
        set { isOnGround = value; }
    }


    /* ------------------------------------------  METHODS ------------------------------------------- */


    private void Start()
    {
        // Find player (no dragging in player for each object)
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Find EquipmentSystemController instead of dragging, yay (probably temp only) 
        if (equipmentSystemController == null)
            equipmentSystemController = GameObject.FindObjectOfType<EquipmentSystemController>();

        // Initialize
        InititializeEquipment();
    }


    /// <summary>
    /// Method for initializing the components needed for the equipment items <br/>
    /// Adds PhysicsManager to parent as well, stores the collider of the parent and sets rotation of parent object.
    /// </summary>
    private void InititializeEquipment()
    {
        // First assign our parent object to the main object slot
        mainEquipmentObject = transform.parent.gameObject;
        
        // Set our parent to the equipment layer to be able to pick up when needed
        mainEquipmentObject.gameObject.layer = LayerMask.NameToLayer(equipmentLayerName);

        // Add and assign the PhysicsManager to the parent object
        equipmentPhysicsManager = mainEquipmentObject.AddComponent<EquipmentPhysicsManager>();

        // Get the parent's collider to detect mouse
        parentCollider = mainEquipmentObject.GetComponent<Collider>();

        // Set rotation and scale of parent object to it's data values
        SetObjectRotation(MainEquipmentObject.transform,EquipmentData.UnequippedRotation);
        SetObjectScale(MainEquipmentObject.transform, EquipmentData.UnequippedLocalScale);

        // Set position of this object to the main object position as reset
        SetObjectPosition(gameObject.transform, mainEquipmentObject.transform.position);
    }

    /// <summary>
    /// Method to set a transform's position to that of the given vector
    /// </summary>
    /// <param name="_targetObject"></param>
    /// <param name="_targetPos"></param>
    private void SetObjectPosition(Transform _targetObject, Vector3 _targetPos)
    {
        _targetObject.transform.position = _targetPos;
    }

    /// <summary>
    /// Method to set the rotation of target object to target rotation
    /// </summary>
    /// <param name="_targetRotation"></param>
    private void SetObjectRotation(Transform _targetObject ,Vector3 _targetRotation)
    {
        // Set local rotation to param values
        _targetObject.transform.localRotation = Quaternion.Euler(_targetRotation);
    }

    /// <summary>
    /// Method to set the scale of the target object to the target scaleparent object
    /// </summary>
    /// <param name="_targetScale"></param>
    private void SetObjectScale(Transform _targetObject,Vector3 _targetScale)
    {
        _targetObject.transform.localScale = _targetScale;
    }

    /// <summary>
    /// Handles equipment being available to pick up 
    /// </summary>
    private void Update()
    {
        // Return if the collider is not being targeted by the mouse, or if the player is not within equipdistance
        if (!IsMouseOverCollider(parentCollider) || !IsWithinEquipRange())
        {
            return;
        }
        else // Mouse is over equipment, and player is within equiprange
        {
            // Check if the item is not equipped yet, and if it's not in the air
            if (!IsEquipped && IsOnGround)
            {
                // Send message that 
                equipmentSystemController.TryEquip(this);
            }
        }
    }

    /// <summary>
    /// Public method called when equipping this object to hand <br/>
    /// Repositions object to hand, sets parent, and sets IsEquipped and CanDrop bool values true
    /// </summary>
    public void OnEquip(Hand _targetHand)
    {
        // Set transform properties
        SetObjectScale(MainEquipmentObject.transform, EquipmentData.EquippedLocalScale); // First set scale before parenting
        _targetHand.SetObjectToHandPosition(this); // Call method to reposition and set hand as parent
        SetObjectRotation(MainEquipmentObject.transform, EquipmentData.EquippedRotation); // Lastly, set the local rotation when in hand

        // Set value of bools true
        IsEquipped = true;
        IsOnGround = false;
        equipmentPhysicsManager.Rb.isKinematic = true; // Set kinematic true to prevent gravity and other forces impacting object

        // Start coroutine to set value of CanDrop bool after this frame ends
        StartCoroutine(EnableDropAfterFrame());
    }

    /// <summary>
    /// Public method called when dropping this equipment from hand <br/>
    /// Removes hand as parent, repositions object and sets IsEquipped and CanDrop bool values to false
    /// </summary>
    public void OnDrop(Hand _ownerHand)
    {   
        // Set values of booleans first
        IsEquipped = false;
        CanDrop = false; // Set false to prevent calling again
        equipmentPhysicsManager.Rb.isKinematic = false; // Set false to apply gravity and other forces to parent object

        // Set transform properties
        mainEquipmentObject.transform.parent = null; // First drop the parent
        SetObjectRotation(MainEquipmentObject.transform, EquipmentData.UnequippedRotation); // Set local rotation, not relative to parent anymore
        SetObjectScale(MainEquipmentObject.transform, EquipmentData.UnequippedLocalScale); // Set local scale to original, not relative to parent anymore

        // Call method to throw equipment
        equipmentPhysicsManager.ThrowEquipment(); // Note: Notice isKinematic = false before calling method
    }

    /// <summary>
    /// Returns true if the mouse cursor is currently on the param collider
    /// </summary>
    /// <param name="_colliderToCheck"></param>
    /// <returns></returns>
    private bool IsMouseOverCollider(Collider _colliderToCheck)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast a ray from the mouse position
        if (Physics.Raycast(ray, out hit) && hit.collider == _colliderToCheck)
        {
            // Mouse is over the specified collider
            return true;
        }

        // Mouse is not over the specified collider
        return false;
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
    /// Pubic bool method that returns true if this object is within { equip distance } of player
    /// </summary>
    /// <returns></returns>
    public bool IsWithinEquipRange()
    {
        return (DistanceFromPlayer() < equipmentData.EquipDistance);
    }

    /// <summary>
    /// Returns the velocity of the player if rigidbody is found
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public Vector3 GetPlayerVelocity()
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

