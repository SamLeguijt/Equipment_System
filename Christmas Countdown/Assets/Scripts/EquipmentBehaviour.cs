using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentBehaviour : MonoBehaviour
{
    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Tooltip("Drag in object with the EquipmentSystemController attached")]
    [SerializeField] private EquipmentSystemController equipmentSystemController;

    [Tooltip("Scriptable Object with this object's data")]
    [SerializeField] private BaseEquipmentObject equipmentData;

    [Tooltip("Player object in scene, used for calculating equip distance")] 
    [SerializeField] private Transform player;

    // Bools for checking status of this object, used for properties
    private bool isEquipped;
    private bool canDrop;

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

    /// <summary>
    /// Public method called when equipping this object to hand <br/>
    /// Sets status of bools
    /// </summary>
    public void OnEquip()
    {
        // Start coroutine to set value of CanDrop bool
        if (!IsEquipped) StartCoroutine(EnableDropAfterFrame());


        IsEquipped = true;
    }

    /// <summary>
    /// Public method called when dropping this equipment from hand
    /// </summary>
    public void OnDrop()
    {
        // Set values of booleans
        IsEquipped = false;
        CanDrop = false;    
    }


    /// <summary>
    /// Method gets called while the mouse cursor is over this object's collider <br/>
    /// Sends message to EquipSystemController to be equipped if not already
    /// </summary>
    private void OnMouseOver()
    {
        if (!IsEquipped)
        {
            equipmentSystemController.OnCursorOver(this);
        }
    }

    /// <summary>
    /// IEnumerator that sets the value of CanDrop to true after end of frame <br/>
    /// Prevents dropping equipment same frame as equipping it
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableDropAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        CanDrop = true;
    }

    /// <summary>
    /// Pubic bool method that returns true if this object is within { equip distance } of player
    /// </summary>
    /// <returns></returns>
    public bool IsWithinEquipRange()
    {
        return (DistanceFromPlayer() < equipmentSystemController.equipDistance);
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
