using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableActivation : MonoBehaviour, IEquipmentActivation
{
    /*--- All private because no need to access or edit them --- */

    //Reference to the equipment behaviour for this object
    private EquipmentBehaviour equipmentBehaviour;

    // Reference to this object's scriptable object as data
    private ThrowablesEquipmentObject throwableData;

    // Reference to the equipment controller for input and key bindings
    private EquipmentSystemController equipmentController;

    private bool isThrowActivated;
    /// <summary>
    /// Initializes this activation by getting neccesary components
    /// </summary>
    /// <param name="_equipmentBehaviour"></param>
    public void Initialize(EquipmentBehaviour _equipmentBehaviour)
    {
        equipmentBehaviour = _equipmentBehaviour;
        equipmentBehaviour.activationLogic = this;

        throwableData = (ThrowablesEquipmentObject)equipmentBehaviour.EquipmentData;
        equipmentController = equipmentBehaviour.EquipmentSystemController;
    }

    /// <summary>
    /// Gets called upon receiving activation key input for one of the hands when object is equipped in hand
    /// </summary>
    public void Activate()
    {
        // Check if the current hand is recognized by the controller
        if (equipmentController.FullHandKeyBindings.ContainsKey(equipmentBehaviour.CurrentHand))
        {
            // Get the ascociated activation key for that hand
            KeyCode targetKey = equipmentController.FullHandKeyBindings[equipmentBehaviour.CurrentHand].ActivationKey;

            // Check if the key is down and if the throw did not activate yet
            if (Input.GetKey(targetKey) && !isThrowActivated)
            {
                // If down, activate this so call method to throw equipment
                StartCoroutine(ThrowEquipment());

                // Set true to prevent starting coroutine multiple times
                isThrowActivated = true;
            }
        }
    }

    /// <summary>
    /// Coroutine used for throwing the equipment object by applying forces
    /// </summary>
    /// <returns></returns>
    private IEnumerator ThrowEquipment()
    {
        // First wait a delay time before applying the forces
        yield return new WaitForSeconds(throwableData.ThrowDelaySeconds);

        // Get the rigidbody of the actual object to throw it
        Rigidbody rb = equipmentBehaviour.MainEquipmentObject.GetComponent<Rigidbody>();

        // Call method to get the targetPoint, depending on mouse pos and max throw dist
        Vector3 targetPoint = GetTargetPoint();

        // Store direction between targetpoint and the hand
        Vector3 direction = (targetPoint - equipmentBehaviour.CurrentHand.transform.position).normalized;

        // Calculate distance between the targetpoint and the hand
        float distance = Vector3.Distance(targetPoint, equipmentBehaviour.CurrentHand.transform.position);

        // Calculate random torque using our data max values
        Vector3 dataTorque = throwableData.MaxRotateTorqueSpeed;
        Vector3 randomTorque = new Vector3(Random.Range(0, dataTorque.x), Random.Range(0, dataTorque.y), Random.Range(0, dataTorque.z));

        // Call method to drop the euqipment from the hand, not applying drop forces
        equipmentController.Drop(equipmentBehaviour, equipmentBehaviour.CurrentHand, false);

        //Apply force to the Rigidbody in the direction of target point
        rb.AddForce(direction * throwableData.ThrowForceValue, ForceMode.Impulse);

        // Apply force upwards to create a curve trajectory, based on distance (scaled by dividing with constant)
        rb.AddForce(Vector3.up * (distance / throwableData.DistanceDivider), ForceMode.Impulse);

        // Apply random torque
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        // Set bool false to enable throwing again
        isThrowActivated = false;
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


        // Perform the raycast to see if it hits
        if (Physics.Raycast(ray, out RaycastHit hit, throwableData.MaxThrowDistance))
        {
            // If the ray hits an object within the max distance range, return the hit point
            return hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, return a point at the maximum distance
            return ray.GetPoint(throwableData.MaxThrowDistance);
        }
    }
}