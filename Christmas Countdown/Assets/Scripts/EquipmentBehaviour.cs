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
    //[Tooltip("Prefab of this equipment model")]
    //[SerializeField] GameObject modelPrefab;

    [Tooltip("Scriptable Object with this object's data")]
    [SerializeField] private BaseEquipmentObject equipmentData;
    
    private Rigidbody modelRb;
    private Collider modelCollider;

    public Transform playerOrien;

    // Bools for checking status of this object, used for properties
    private bool isEquipped;
    private bool canDrop;

    public float rotateSpeed;
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

    /* ------------------------------------------  METHODS ------------------------------------------- */


    /* States: 
     * InHand
     * OutOfHand
     * if outOfHand > Collider, rb, rotated
     * if not in hand > no collider, no rb, rotated towards mouse
     * 
     * This script is attached to a prefab model object
     * prefab model has a rb, collider 
     */ 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        modelRb = GetComponent<Rigidbody>();


        //SetRotation();
    }

    private void SetRotation(Quaternion _targetRotation)
    {
        transform.rotation = _targetRotation;
    }
    /// <summary>
    /// Public method called when equipping this object to hand <br/>
    /// Repositions object to hand, sets parent, and sets IsEquipped and CanDrop bool values true
    /// </summary>
    public void OnEquip(Hand _targetHand)
    {
         modelRb.isKinematic = true;

        // Set position and parent to hand object
        transform.position = _targetHand.transform.position;
        gameObject.transform.SetParent(_targetHand.transform, true);

        // Set value of bool true
        IsEquipped = true;

        // Start coroutine to set value of CanDrop bool after this frame ends
        StartCoroutine(EnableDropAfterFrame());
    }

    /// <summary>
    /// Public method called when dropping this equipment from hand <br/>
    /// Removes hand as parent, repositions object and sets IsEquipped and CanDrop bool values to false
    /// </summary>
    public void OnDrop(Hand _ownerHand)
    {
       // modelCollider.enabled = true;
       modelRb.isKinematic = false;

        // Set values of booleans
        IsEquipped = false;
        CanDrop = false;

        // Set position and parent 
        transform.parent = null;
        transform.position = _ownerHand.transform.position;
    }


    /// <summary>
    /// Method gets called while the mouse cursor is over this object's collider <br/>
    /// Sends message to EquipSystemController to be equipped if not already
    /// </summary>
    private void OnMouseOver()
    {
        // First check if this is not already equipped
        if (!IsEquipped)
        {
            // Send message that mouse is targeting this object
            equipmentSystemController.OnMouseOverEquipment(this);
        }
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
    /// Private flaot method that returns the distance between the player and this object
    /// </summary>
    /// <returns></returns>
    private float DistanceFromPlayer()
    {
        return Vector3.Distance(Player.position, transform.position);
    }
}
