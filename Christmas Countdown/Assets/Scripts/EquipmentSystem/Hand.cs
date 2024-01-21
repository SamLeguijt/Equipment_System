using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class for Hand object. <br/>
/// Has a reference to an Equipment object to see if hand is currently equipped. 
/// </summary>
public class Hand : MonoBehaviour
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Tooltip("What type does this hand object have?")]
    [SerializeField] private TypeOfHand handType;

    // Reference to the camera controller script, used for orientation and rotation
    private CameraController camController;

    // Center target attached to the camera
    private Transform camCenter;

    // Reference to the current equipped item for this hand
    private EquipmentBehaviour currentEquipment;

    public KeyCode equipDropKey;
    public KeyCode activationKey;

    /// <summary>
    /// Read-only property to get the hand type of this object
    /// </summary>
    public TypeOfHand HandType
    {
        get { return handType; }
    }

    /// <summary>
    /// Public property to get and set the Current Equipment of this hand
    /// </summary>
    public EquipmentBehaviour CurrentEquipment
    {
        get { return currentEquipment; }
        set { currentEquipment = value; }
    }

    private void Start()
    {
        camController = Camera.main.GetComponent<CameraController>();
        camCenter = camController.CenterTarget;

        // Check if camera components are found, otherwise log error and destroy this script 
        if (camController == null || camCenter == null)
        {
            Debug.LogError($"Camera components not set up correctly, destroying Hand script for {gameObject.name} ");
            Destroy(this);
        }
    }

    private void Update()
    {
        // Return first if no CurrentEquipment
        if (CurrentEquipment == null) return;
        else // We should rotate this object to the mouse to aim our equipment in correct direction
        {
            // Call method to rotate this object to mouse
            RotateToMouse();
        }
    }

    /// <summary>
    /// Method that rotates this object to the mouse cursor (camCenter) 
    /// </summary>
    private void RotateToMouse()
    {
        // Calculate the rotation needed to point towards the target point
        Quaternion targetRotation = Quaternion.LookRotation(camCenter.position - transform.position);

        // Smoothly rotate the weapon towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentEquipment.EquipmentData.RotateToMouseSpeed);
    }

    /// <summary>
    /// Method for setting the equipment object to this hand's position as a child <br/>
    /// Determines the correct position by looking in param data and this HandType
    /// </summary>
    /// <param name="_equipment"> The behaviour who's position needs to be set to hand </param>
    /// <param name="_setParentOfBehaviour"> True for getting the MainEquipmentObject of the behaviour, false for setting transform of _behaviour itself</param>
    public void SetObjectToHandPosition(EquipmentBehaviour _equipment, bool _setParentOfBehaviour = true)
    {
        // Local vector for position offset 
        Vector3 handPosOffset = Vector3.zero;

        // Switch statement for each hand type
        switch (HandType)
        {
            // Is left hand, so get the left hand pos offset from the data
            case TypeOfHand.Left:
                handPosOffset = _equipment.EquipmentData.LeftHandPositionOffset;
                break;
            // Is right hand, so get the left hand pos offset from the data
            case TypeOfHand.Right:
                handPosOffset = _equipment.EquipmentData.RightHandPositionOffset;
                break;
            default: 
                // Other hand type case:
                Debug.LogError($"No HandType assigned to {gameObject.name}, can not get in hand position from data! ");
                break;
        }

        // Conditions based on bool value
        if (_setParentOfBehaviour) // Should set the MainEquipmentOBject of the _behaviour
        {
            // First set parent before adjusting local position
            _equipment.MainEquipmentObject.transform.SetParent(transform);
            _equipment.MainEquipmentObject.transform.localPosition = handPosOffset; // Set local position to relative hand position offset (Zero is hand.pos, so only the offset)
        }
        else // Should not set the MainEquipmentObject of _behaviour, so transform of behaviour self
        {
            _equipment.transform.localPosition = handPosOffset;
            _equipment.transform.SetParent(transform);
        }
    }
}

/// <summary>
/// Enum for setting the type of hand
/// </summary>
public enum TypeOfHand
{
    Left,
    Right
}
