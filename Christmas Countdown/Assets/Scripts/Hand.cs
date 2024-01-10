using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class for Hand object. <br/>
/// Has a reference to an Equipment object to see if hand is currently equipped. 
/// </summary>
public class Hand : MonoBehaviour
{
    public enum TypeOfHand
    {
        left,
        right
    }

    [SerializeField] private TypeOfHand handType;

    private CameraController camController;
    private Transform camCenter; 

    // Reference to the current equipped item for this hand
    private EquipmentBehaviour currentEquipment;

    public TypeOfHand HandType
    {
        get { return handType; }
        private set { handType = value; }
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
        CameraController camController = Camera.main.GetComponent<CameraController>();
        Transform cameraCenter = camController.CenterTarget;
    }
    private void Update()
    {

        if (camController != null && camCenter != null && CurrentEquipment != null)
        {
            Transform targetPoint = camCenter;

            if (targetPoint != null)
            {
                // Calculate the rotation needed to point towards the target point
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint.position - transform.position);

                // Smoothly rotate the weapon towards the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentEquipment.EquipmentData.RotateToMouseSpeed);
            }
        }
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
            case TypeOfHand.left:
                 handPosOffset = _equipment.EquipmentData.LeftHandPositionOffset;
                break;
            // Is right hand, so get the left hand pos offset from the data
            case TypeOfHand.right:
                handPosOffset = _equipment.EquipmentData.RightHandPositionOffset;
                break;
            default:
                Debug.LogError($"No HandType assigned to {gameObject.name}, can not get in hand position from data! ");
                break;
        }

        // Conditions based on bool value
        if (_setParentOfBehaviour) // Should set the MainEquipmentOBject of the _behaviour
        {
            _equipment.MainEquipmentObject.transform.position = transform.position + handPosOffset;
            _equipment.MainEquipmentObject.transform.SetParent(transform);
        } 
        else // Should not set the MainEquipmentObject of _behaviour, so transform of behaviour self
        {
            _equipment.transform.position = transform.position + handPosOffset;
            _equipment.transform.SetParent(transform);
        }
    }
}
