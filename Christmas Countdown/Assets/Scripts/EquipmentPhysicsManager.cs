using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPhysicsManager : MonoBehaviour
{
    private EquipmentBehaviour equipmentBehaviour;
    private Collider objectCollider;
    private Rigidbody rb;

    private int layerToCheck;

    public Rigidbody Rb
    {
        get { return rb; }
        private set { rb = value; }
    }
    private void Start()
    {
        equipmentBehaviour = GetComponentInChildren<EquipmentBehaviour>();

        if (equipmentBehaviour == null)
        {
            Debug.LogError("EquipmentPhysicsManager relies on component EquipmentBehaviour in order to be used, remove EquipmentPhysicsManager and add EquipmentBehaviour as child object!");
            Destroy(this);
        }
        else
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        objectCollider = GetComponent<Collider>();

        rb = gameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        
        layerToCheck = LayerMask.NameToLayer(equipmentBehaviour.environmentLayerName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layerToCheck)
        {
            equipmentBehaviour.IsOnGround = true; 
        }
    }

    /// <summary>
    /// Method that throws the equipment from hand with a certain force 
    /// </summary>
    public void ThrowEquipment()
    {
        // Set object rigidbody to the player's velocity to carry momentum
        rb.velocity = equipmentBehaviour.GetPlayerVelocity();

        // Get the dropForce vector
        Vector3 dropForce = GetDropForceVector();

        // Apply the dropforce using Impulse forcemode to throw equipment 
        rb.AddForce(dropForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Calculates and returns the drop force vector based on the player's orientation.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private Vector3 GetDropForceVector()
    {
        // Get the camera controller scipt to access the orientation of the player
        CameraController cameraController = Camera.main.GetComponent<CameraController>();

        // Check if found
        if (cameraController != null)
        {
            // Get the forward and upwards directions by looking at the player's orientation 
            Vector3 forwardDirection = cameraController.PlayerOrientation.forward;
            Vector3 upwardDirection = cameraController.PlayerOrientation.up;

            // Multiply the direction components with our dropForce vector components to apply force in correct directions
            Vector3 dropForceVector = forwardDirection * equipmentBehaviour.EquipmentData.EquipmentDropForce.x + upwardDirection * equipmentBehaviour.EquipmentData.EquipmentDropForce.y;

            // Return the calculated vector
            return dropForceVector;
        }
        else // CameraController not found on main camera
        {
            throw new System.Exception("CameraController is not found on main camera, cannot calculate drop force.");
        }
    }
}
