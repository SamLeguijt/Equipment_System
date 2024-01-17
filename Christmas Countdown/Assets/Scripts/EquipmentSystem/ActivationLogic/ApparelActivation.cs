using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApparelActivation : MonoBehaviour, IEquipmentActivation
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    // No need for inspector referencing, so leave to private 

    // Reference to the equipment's behaviour
    private EquipmentBehaviour myEquipBehaviour;

    // Reference to the data of the object
    private ApparelEquipmentObject myData;

    // Reference to the equipment controller, used for checking input to hands
    private EquipmentSystemController equipmentController;

    // Reference to the target transform as head
    private Transform targetHead;

    // Reference to the camera transform
    private Transform camTransform;

    // Private bool to track if the item is already equipped
    private bool isOnHead;

    /// <summary>
    /// Initializes class by assigning components and values
    /// </summary>
    /// <param name="_equipment"></param>
    /// <param name="_targetTransform"></param>
    public void Initialize(EquipmentBehaviour _equipment, Transform _targetTransform)
    {
        // Set behaviour script references
        myEquipBehaviour = _equipment;
        myEquipBehaviour.activationLogic = this;

        // Set our head transform target to the param
        targetHead = _targetTransform;

        // Get the data of this object via behaviour script
        myData = (ApparelEquipmentObject)myEquipBehaviour.EquipmentData;

        // Get the equippmentsystem reference by looking in our behaviour
        equipmentController = myEquipBehaviour.EquipmentSystemController;

        // Get transform of the camera
        camTransform = Camera.main.transform;

        // Log error and disable if any references are null
        if (myEquipBehaviour == null || targetHead == null || myData == null || equipmentController == null || camTransform == null)
        {
            Debug.LogError($"Not all references and components are found for {gameObject.name}, assign and run again. Disabling for now!");
            this.enabled = false;   
        }
        else
        {
            // Set camera and parent to the camera transform (FPS camera is located on player's head, so camera is our head)
            targetHead.position = camTransform.position;
            targetHead.SetParent(camTransform); // Set as parent to rotate and stay in position with ease without Update    
        }
    }

    /// <summary>
    /// Equips the hat to the head if not equipped yet
    /// </summary>
    public void Activate()
    {
        // Check if not already  on head
        if (!isOnHead)
        {
            // Call method to equip on head
            EquipToHead(targetHead);

            // Start coroutine to flip bool after x second
            StartCoroutine(SetOnHeadValueNextFrame(true)); // coroutine to prevent equipping/dequipping in same frame
        }
    }

    /// <summary>
    /// Handles placing the Apparal object back in one of the hands on input
    /// </summary>
    private void Update()
    {
        // Return if we're not on head
        if (!isOnHead) return;
        else
        {
            // Call method to handle equipping in hand for both hands referenced in the controller
            HandleEmptyHandEquip(equipmentController.LeftHand);
            HandleEmptyHandEquip(equipmentController.RightHand);
        }
    }

    /// <summary>
    /// Handles equipping the object into one of the hands on activation input
    /// </summary>
    /// <param name="_hand"></param>
    private void HandleEmptyHandEquip(Hand _hand)
    {
        // Check if the activation input for an empty hand is being pressed
        if (equipmentController.CheckEmptyHandActivationInput(_hand))
        {
            // If so, call method to equip the behaviour (hat item) into that hand
            equipmentController.Equip(myEquipBehaviour, _hand); // Equipping = equip to hand, so removes from head

            // Flip bool to false
            isOnHead = false;
        }
    }



    /// <summary>
    /// Method to equip the hat to the param position 
    /// </summary>
    /// <param name="_target"></param>
    private void EquipToHead(Transform _target)
    {
        // Position and reparent to the camTransform (cam == player head since its FP view)
        targetHead.position = camTransform.position;
        targetHead.SetParent(camTransform);

        // Drop the equipmentbehaviour from its hand (remove from hand to head)
        equipmentController.Drop(myEquipBehaviour, myEquipBehaviour.CurrentHand, false);

        // Short notation of the main equipment (hat object)
        GameObject hat = myEquipBehaviour.MainEquipmentObject;

        // Set kinematic to true to prevent collisions and gravity mispositioning our object 
        hat.GetComponent<Rigidbody>().isKinematic = true;

        // Set the parent to the param
        hat.transform.SetParent(_target);

        // Set transform values to data properties to influence the looks while on head
        hat.transform.localPosition = Vector3.zero + myData.OnHeadPositionOffset;
        hat.transform.localRotation = Quaternion.Euler(myData.OnHeadRotation);
        hat.transform.localScale = myData.OnHeadScale;
    }

    /// <summary>
    /// Coroutine that flips param bool to true next second
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetOnHeadValueNextFrame(bool _targetValue)
    {
        // Wait till next frame
        yield return null;

        // Set true
        isOnHead = _targetValue;
    }
}
