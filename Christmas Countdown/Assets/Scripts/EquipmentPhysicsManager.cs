using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for handling the physics of the Equipments <br/>
/// Gets added to parent object by EquipmentBehaviour
/// </summary>
public class EquipmentPhysicsManager : MonoBehaviour
{
    // All privaye variables to prevent access from inspector or other classes
    private EquipmentBehaviour equipmentBehaviour;
    private Collider objectCollider;
    private Rigidbody rb;
    private int layerToCheckEnvironmentCollisions;

    // Public get private set rb property to change rb properties in other classes
    public Rigidbody Rb
    {
        get { return rb; }
        private set { rb = value; }
    }

    private void Start()
    {
        // Get the equipmentbehaviour by looking in our children (this gets added by the script to parent, so must be in children)
        equipmentBehaviour = GetComponentInChildren<EquipmentBehaviour>();

        // Check if we found it to prevent manually putting this script on objects, or EquipBehaviour not as a child object
        if (equipmentBehaviour == null)
        {
            //Show error message and destroy if not found in children
            Debug.LogError("EquipmentPhysicsManager relies on component EquipmentBehaviour in order to be used, remove EquipmentPhysicsManager and add EquipmentBehaviour as child object!");
            Destroy(this);
        }
        else // Component found, everything good
        {
            // Initialize this class methods and properties
            Initialize();
        }
    }

    /// <summary>
    /// Method to start up this class and it's properties
    /// </summary>
    public void Initialize()
    {
        // Get the collider manually attached to this object
        objectCollider = GetComponent<Collider>();

        if (objectCollider == null)
        {
            Debug.LogError($"No collider attached to {gameObject.name}, add component and rerun!");
            Destroy(gameObject);
        }

        // Add and assign rigidbody through here 
        rb = gameObject.AddComponent<Rigidbody>();

        // Set the collision detection mode to prevent not calling correctly
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        
        // Assign value to layer which is used to check collision with ground and more
        layerToCheckEnvironmentCollisions = LayerMask.NameToLayer(equipmentBehaviour.environmentLayerName);
    }

    /// <summary>
    /// Checks when our collider hits an object on the Environment layer
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!equipmentBehaviour.IsEquipped && !equipmentBehaviour.IsOnGround)
        {
            // Check if collision was on right layer
            if (collision.gameObject.layer == layerToCheckEnvironmentCollisions)
            {
                // Set EquipmentBehaviour bool true to enable equipping
                equipmentBehaviour.IsOnGround = true;
            }
        }
        else return;
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
