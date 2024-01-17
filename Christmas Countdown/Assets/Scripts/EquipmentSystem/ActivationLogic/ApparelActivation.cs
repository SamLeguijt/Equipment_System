using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparelActivation : MonoBehaviour, IEquipmentActivation
{
    public Transform headTransform;
    private EquipmentBehaviour myEquipBehaviour;

    private Transform camTransform;
    private ApparelEquipmentObject myData;

    private bool isOnHead;
    /* We want: 
 * Pick up hat, activate ->
 * Activate removes from hand, puts on head pos
 * Player can equip new item in hand while wearing hat 
 * If player has 2nd hat, swap hats in hand
 * if player has full hands, cant drop the hat 
 * If hat is on head, and empty hand, activate empty hand to unequip from head in hand
 */
    public void Initialize(EquipmentBehaviour _equipment, Transform _targetTransform)
    {
        // Set behaviour script references
        myEquipBehaviour = _equipment;
        myEquipBehaviour.activationLogic = this;

        // Set our head transform target to the param
        headTransform = _targetTransform;

        // Get the data of this object via behaviour script
        myData = (ApparelEquipmentObject)myEquipBehaviour.EquipmentData;

        // Get transform of the camera
        camTransform = Camera.main.transform;

        // Set camera and paren to the camera transform (FPS camera is located on player's head, so camera is our head)
        headTransform.position = camTransform.position; 
        headTransform.SetParent(camTransform); // Set as parent to rotate and stay in position with ease without Update
    }
    public void Activate()
    {
        headTransform.position = camTransform.position;
        headTransform.SetParent(camTransform);

        if (!isOnHead)
        {
            EquipToHead(headTransform);
            isOnHead = true;
        }
        else
        {
            Debug.Log("Test call");
            //EquipToHand();
        }
    }



    /// <summary>
    /// Method to equip the hat to the head position 
    /// </summary>
    /// <param name="_target"></param>
    private void EquipToHead(Transform _target)
    {
        // Short notation of the main equipment (hat object)
        GameObject hat = myEquipBehaviour.MainEquipmentObject;

        // Call method to drop the equipment from hand, so no longer equipped in the hand
        myEquipBehaviour.OnDrop(false);

        // Set kinematic to true to prevent collisions and gravity mispositioning our object 
        hat.GetComponent<Rigidbody>().isKinematic = true;

        // Set the parent to the param
        hat.transform.SetParent(_target);

        // Set transform values to data properties to influence the looks while on head
        hat.transform.localPosition = Vector3.zero + myData.onHeadPositionOffset;
        hat.transform.localRotation = Quaternion.Euler(myData.onHeadRotation);
        hat.transform.localScale = myData.onHeadScale;

    }

    private void EquipToHand()
    {
        /*  Calls upon activate input
         *  
         */


        EquipmentSystemController system = myEquipBehaviour.EquipmentSystemController;

        
        if (myEquipBehaviour.CurrentHand.HandType == Hand.TypeOfHand.Left)
        {
            system.Equip()
        }
      
    }
}
