using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.XR;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Public class for behaviour tasks of Equipments. <br/>
/// Uses a ref to a scriptable object for it's equipment data, and reference to EquipmentSystemController for equip and drop actions
/// </summary>
public class EquipmentBehaviour : MonoBehaviour
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Header("References to components in scene")]
    [Space]

    [Tooltip("Drag in object with the EquipmentSystemController attached")]
    [SerializeField] private EquipmentSystemController equipmentSystemController;

    [Tooltip("Object that holds the EquipmentUI script")]
    [SerializeField] private EquipmentUI equipmentUI;

    [Space]
    [Header("Layer names")]
    [Space]

    [Tooltip("Name of the layer for environmental objects")]
    [SerializeField] private string environmentLayerName;

    [Tooltip("Name of the layer for equipment objects")]
    [SerializeField] private string mainEquipmentLayerName;

    [Tooltip("Name of the layer for mouse detection")]
    [SerializeField] private string mouseDetectionLayerName;

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
    public Collider mouseDetectCollider;
    private Transform player; // Reference to the player for distance and orientation
    private Hand currentHand;
    public IEquipmentActivation activationLogic;

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
        get { return equipmentSystemController; }
    }

    // Read only property to reference the UI object on this behaviour
    public EquipmentUI EquipmentUI
    {
        get { return equipmentUI; }
    }

    /// <summary>
    /// Read only property to get the name of the environment layer
    /// </summary>
    public string EnvironmentLayerName
    {
        get { return environmentLayerName; }
    }

    /// <summary>
    /// Read only property representing the name of the layer for Main Equipment
    /// </summary>
    public string MainEquipmentLayerName
    {
        get { return mainEquipmentLayerName; }
    }

    /// <summary>
    /// Read only property representing the name of the layer for mouse detection
    /// </summary>
    public string MouseDetectionLayerName
    {
        get { return mouseDetectionLayerName; }
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
        get { return equipmentData; }
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

    public Collider MouseDetectCollider
    {
        get { return mouseDetectCollider; }
    }
    /// <summary>
    /// Private player property used for calculating distance from this object to player
    /// </summary>
    private Transform Player
    {
        // Get player if not null, else throw null reference exception with message
        get { return player; }
    }

    public Hand CurrentHand
    {
        get { return currentHand; }
        private set {  currentHand = value; }   
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
            equipmentSystemController = FindObjectOfType<EquipmentSystemController>();

        if (equipmentUI == null)
            equipmentUI = FindObjectOfType<EquipmentUI>();

        // First set parent object to the main object slot
        mainEquipmentObject = transform.parent.gameObject;

        // Add and assign the PhysicsManager to the parent object
        equipmentPhysicsManager = mainEquipmentObject.AddComponent<EquipmentPhysicsManager>();

        // Get the parent's collider to detect mouse
        parentCollider = mainEquipmentObject.GetComponent<Collider>();

        // Check if any components and references are missing
        if (equipmentSystemController == null || equipmentUI == null || mainEquipmentObject == null || EquipmentData == null || equipmentPhysicsManager == null || player == null || parentCollider == null || mouseDetectCollider == null)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            throw new System.Exception($"One or more components and references are missing from {gameObject.transform.parent.name}! Please assign components, then re-run. Disabling object for now.");
        }
        else // All components and references are present, so initialize
        {
            InititializeEquipmentBehaviour();
        }

    }


    /// <summary>
    /// Method for initializing the components needed for the equipment items <br/>
    /// Adds PhysicsManager to parent as well, stores the collider of the parent and sets rotation of parent object.
    /// </summary>
    private void InititializeEquipmentBehaviour()
    {
        // Set our parent to the equipment layer to be able to pick up when needed
        mainEquipmentObject.gameObject.layer = LayerMask.NameToLayer(MainEquipmentLayerName);

        // Set this object's layer to the mouse detect layer for mouse detection
        gameObject.layer = LayerMask.NameToLayer(MouseDetectionLayerName);

        // Set rotation and scale of parent object to it's data values
        SetObjectRotation(MainEquipmentObject.transform, EquipmentData.UnequippedRotation);
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
    private void SetObjectRotation(Transform _targetObject, Vector3 _targetRotation)
    {
        // Set local rotation to param values
        _targetObject.transform.localRotation = Quaternion.Euler(_targetRotation);
    }

    /// <summary>
    /// Method to set the scale of the target object to the target scaleparent object
    /// </summary>
    /// <param name="_targetScale"></param>
    private void SetObjectScale(Transform _targetObject, Vector3 _targetScale)
    {
        _targetObject.transform.localScale = _targetScale;
    }

    /// <summary>
    /// Handles equipment being available to pick up 
    /// </summary>
    private void Update()
    {
        // return if not within range, or if not targeting the object's collider
        if (!IsMouseOverCollider(mouseDetectCollider) || !IsWithinEquipRange())
        {
            return;
        }
        else // Mouse is over equipment, or player is within equiprange
        {
            // First check if both conditions are true
            if (IsMouseOverCollider(mouseDetectCollider) && IsWithinEquipRange())
            {
                // Enable the ui if in range and mouse targetting equipment
                equipmentUI.UpdateEquipmentInfo(this);

                // Check if the item is not equipped yet, and if it's not in the air
                if (!IsEquipped && IsOnGround)
                {
                    // Send message that equipment can be equipped
                    equipmentSystemController.TryEquip(this);
                }
            }
            else return; // Return if only one of the conditions is met
        }
    }

    /// <summary>
    /// Public method called when equipping this object to hand <br/>
    /// Repositions object to hand, sets parent, and sets IsEquipped and CanDrop bool values true
    /// </summary>
    public void OnEquip(Hand _targetHand)
    {
        // Set transform properties
        _targetHand.SetObjectToHandPosition(this); // Call method to reposition and set hand as parent
        SetObjectScale(MainEquipmentObject.transform, EquipmentData.EquippedLocalScale); // First set scale before parenting
        SetObjectRotation(MainEquipmentObject.transform, EquipmentData.EquippedRotation); // Lastly, set the local rotation when in hand

        CurrentHand = _targetHand;
        // Set value of bools true
        IsEquipped = true;
        IsOnGround = false;
        equipmentPhysicsManager.Rb.isKinematic = true; // Set kinematic true to prevent gravity and other forces impacting object

        // Disable colliders to prevent collision events 
        parentCollider.enabled = false;
        mouseDetectCollider.enabled = false;

        // Start coroutine to set value of CanDrop bool after this frame ends
        StartCoroutine(EnableDropAfterFrame());
    }

    /// <summary>
    /// Public method called when dropping this equipment from hand <br/>
    /// Removes hand as parent, repositions object and sets IsEquipped and CanDrop bool values to false
    /// </summary>
    public void OnDrop(bool _applyForces = true)
    {
        // Set values of booleans first
        IsEquipped = false;
        CanDrop = false; // Set false to prevent calling again
        equipmentPhysicsManager.Rb.isKinematic = false; // Set false to apply gravity and other forces to parent object

        // Enable colliders to enable collision events
        parentCollider.enabled = true;
        mouseDetectCollider.enabled = true;

        // Set transform properties
        mainEquipmentObject.transform.parent = null; // First drop the parent
        SetObjectRotation(MainEquipmentObject.transform, EquipmentData.UnequippedRotation); // Set local rotation, not relative to parent anymore
        SetObjectScale(MainEquipmentObject.transform, EquipmentData.UnequippedLocalScale); // Set local scale to original, not relative to parent anymore

        // Call method to throw equipment
        if (_applyForces) equipmentPhysicsManager.ThrowEquipment(); // Note: Notice isKinematic = false before calling method
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

