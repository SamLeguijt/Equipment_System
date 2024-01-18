using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableActivation : MonoBehaviour, IEquipmentActivation
{

    private EquipmentBehaviour equipmentBehaviour;

    private ThrowablesEquipmentObject throwableData;
    public void Initialize(EquipmentBehaviour _equipmentBehaviour)
    {
        equipmentBehaviour = _equipmentBehaviour;
        equipmentBehaviour.activationLogic = this;

        throwableData = (ThrowablesEquipmentObject)equipmentBehaviour.EquipmentData;
    }
    public void Activate()
    {
        Hand activationHand = equipmentBehaviour.CurrentHand;

        EquipmentSystemController system = equipmentBehaviour.EquipmentSystemController;

        if (system.FullHandKeyBindings.ContainsKey(activationHand))
        {
            // Get the wanted input key by looking in the dict
            KeyCode targetKey = system.FullHandKeyBindings[activationHand].ActivationKey;

            if (Input.GetKey(targetKey))
            {
                StartCoroutine(ThrowEquipment());
            }
        }
    }

    private IEnumerator ThrowEquipment()
    {
        yield return new WaitForSeconds(throwableData.throwDelaySeconds);

        EquipmentSystemController system = equipmentBehaviour.EquipmentSystemController;

        Rigidbody rb = equipmentBehaviour.MainEquipmentObject.GetComponent<Rigidbody>();

        // Call method to drop the euqipment from the hand, not applying drop forces
        system.Drop(equipmentBehaviour, equipmentBehaviour.CurrentHand, false);

        // Call method to get the targetPoint, depending on mouse pos and max throw dist
        Vector3 targetPoint = GetTargetPoint();

        // Store direction between targetpoint and the hand
        Vector3 direction = (targetPoint - equipmentBehaviour.CurrentHand.transform.position).normalized;

        // Calculate distance between the targetpoint and the hand
        float distance = Vector3.Distance(targetPoint, equipmentBehaviour.CurrentHand.transform.position);

        //Apply force to the Rigidbody in the direction of target point
         rb.AddForce(direction * throwableData.throwForceValue, ForceMode.Impulse);

        // Apply force upwards to create a curve trajectory, based on distance (scaled by dividing with constant)
        rb.AddForce(Vector3.up * (distance / throwableData.distanceDivider) , ForceMode.Impulse);
    }


    /// <summary>
    /// Method to get a hit point on trajectory ray <br/>
    /// Returns a point as Vector3 representing either the end of our range, or the point where the ray collides with something
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    Vector3 GetTargetPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        // Create a ray from the main camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, throwableData.MaxThrowDistance))
        {
            // If the ray hits an object within the shooting range, return the hit point
            return hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, return a point at the maximum shooting range
            return ray.GetPoint(throwableData.MaxThrowDistance);
        }
    }
}