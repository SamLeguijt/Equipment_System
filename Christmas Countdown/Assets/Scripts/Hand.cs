using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public class for Hand object. <br/>
/// Has a reference to an Equipment object to see if hand is currently equipped. 
/// </summary>
public class Hand : MonoBehaviour
{
    // Reference to the current equipped item for this hand
    private EquipmentBehaviour currentEquipment;

    /// <summary>
    /// Public property to get and set the Current Equipment of this hand
    /// </summary>
    public EquipmentBehaviour CurrentEquipment
    {
        get { return currentEquipment; }
        set { currentEquipment = value; }
    }

    private void Update()
    {

        CameraController cam = Camera.main.GetComponent<CameraController>();
        Transform camCenter = cam.CenterTarget;

        if (cam != null && camCenter != null && CurrentEquipment != null)
        {
            Transform targetPoint = cam.CenterTarget;

            if (targetPoint != null)
            {
                // Calculate the rotation needed to point towards the target point
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint.position - transform.position);

                // Smoothly rotate the weapon towards the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentEquipment.EquipmentData.RotationSpeed);
            }
        }
    }
}
