using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparelActivation : MonoBehaviour, IEquipmentActivation
{
    public Transform headTransform;
    private Transform cameraTransform;
    private EquipmentBehaviour myEquipBehaviour;

    private ApparelEquipmentObject myData;

    public void Initialize(EquipmentBehaviour _equipment, Transform _targetTransform)
    {
        myEquipBehaviour = _equipment;
        myEquipBehaviour.activationLogic = this;

        myData = (ApparelEquipmentObject)myEquipBehaviour.EquipmentData;

        cameraTransform = Camera.main.transform;
        headTransform = _targetTransform;

    }
    public void Activate()
    {
        /* We want: 
         * Pick up hat, activate ->
         * Activate removes from hand, puts on head pos
         * Player can equip new item in hand while wearing hat 
         * If player has 2nd hat, swap hats in hand
         * if player has full hands, cant drop the hat 
         * If hat is on head, and empty hand, activate empty hand to unequip from head in hand
         */

        EquipToHead(headTransform);

        //   transform.parent = headTransform;
        //     transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (headTransform != null)
        {
            headTransform.position = cameraTransform.position + myData.headPosOffset;
            Debug.Log("not null");


        }

        GameObject hat = myEquipBehaviour.MainEquipmentObject;

        hat.transform.localPosition = Vector3.zero + myData.headPosOffset;

    }

    private void EquipToHead(Transform _target)
    {
        myEquipBehaviour.OnDrop(false);

        GameObject hat = myEquipBehaviour.MainEquipmentObject;

        hat.GetComponent<Rigidbody>().isKinematic = true;

        SetObjectScale(hat.transform, myData.EquippedLocalScale); // First set scale before parenting

        hat.transform.SetParent(cameraTransform);

        hat.transform.localPosition = Vector3.zero + myData.headPosOffset;

        SetObjectRotation(hat.transform, myData.EquippedRotation); // Lastly, set the local rotation when in hand

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
    /// Method to set the rotation of target object to target rotation
    /// </summary>
    /// <param name="_targetRotation"></param>
    private void SetObjectRotation(Transform _targetObject, Vector3 _targetRotation)
    {
        // Set local rotation to param values
        _targetObject.transform.rotation = Quaternion.Euler(_targetRotation);
    }

}
