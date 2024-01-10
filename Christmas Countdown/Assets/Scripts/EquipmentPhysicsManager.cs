using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for handling the physics of the Equipments <br/>
/// Gets added to parent object by EquipmentBehaviour
/// </summary>
public class EquipmentPhysicsManager : MonoBehaviour
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    // All privayte variables to prevent access from inspector or other classes
    private EquipmentBehaviour equipmentBehaviour;
    private CameraController cameraController; // CameraController used for orientation

    // Necessary collider for collision handling
    private Collider objectCollider; 

    // Necessary rigidbody for applying forces and torque
    private Rigidbody rb;

    // Layer to check collisins on
    private int layerToCheckEnvironmentCollisions;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    // Public get, private set rb property to change rb properties in other classes
    public Rigidbody Rb
    {
        get { return rb; }
        private set { rb = value; }
    }


    /* ------------------------------------------  METHODS ------------------------------------------- */


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

        // Add and assign rigidbody through here 
        rb = gameObject.AddComponent<Rigidbody>();

        // Set the collision detection mode to prevent not calling correctly
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Assign value to layer which is used to check collision with ground and more
        layerToCheckEnvironmentCollisions = LayerMask.NameToLayer(equipmentBehaviour.EnvironmentLayerName);

        cameraController = Camera.main.GetComponent<CameraController>();

        // Destroy object if no collider on object
        if (objectCollider == null)
        {
            Debug.LogError($"No collider attached to {gameObject.name}, add component and rerun!");
            Destroy(gameObject);
        }

        // Destroy object if camera controller not found
        if (cameraController == null)
        {
            Debug.LogError($"CameraController not found on main camera, cannot handle physics for {gameObject.name}! Destroying this object.");
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// Checks when our collider hits an object on the Environment layer
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // Only go through if equipment is not yet equipped, and if its not already marked as on ground
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
        // Get Player velocity
        Vector3 playerVelocity = equipmentBehaviour.GetPlayerVelocity();

        // Apply part of player momentum to this rb
        rb.velocity = GetForwardDirection() * playerVelocity.magnitude;

        // Apply dropforce using Impulse forcemode to throw equipment 
        rb.AddForce(GetDropForceVector(), ForceMode.Impulse);

        // Add torque to object to make it spinnn
        rb.AddTorque(GetRandomTorgueVector(), ForceMode.Impulse);
    }

    /// <summary>
    /// Calculates and returns the drop force vector based on the player's orientation.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private Vector3 GetDropForceVector()
    {
        // Get the forward and upwards directions by looking at the player's orientation 
        Vector3 forwardDirection = cameraController.PlayerOrientation.forward;
        Vector3 upwardDirection = cameraController.PlayerOrientation.up;

        // Multiply the direction components with our dropForce vector components to apply force in correct directions
        Vector3 dropForceVector = forwardDirection * equipmentBehaviour.EquipmentData.EquipmentDropForce.x + upwardDirection * equipmentBehaviour.EquipmentData.EquipmentDropForce.y;

        // Return the calculated vector
        return dropForceVector;
    }


    /// <summary>
    /// Vector returning a normalized vector of the player orientation's forward direction
    /// </summary>
    /// <returns></returns>
    private Vector3 GetForwardDirection()
    {
        Vector3 forwardDirection = cameraController.PlayerOrientation.forward;

        return forwardDirection;
    }

    /// <summary>
    /// Returns a random vector with values ranging from 0 - max equipment torgue speed
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomTorgueVector()
    {
        Vector3 maxTorgue = equipmentBehaviour.EquipmentData.MaxRotateTorqueSpeed;
        Vector3 randomTorque = new Vector3(Random.Range(0, maxTorgue.x), Random.Range(0, maxTorgue.y), Random.Range(0, maxTorgue.z));

        return randomTorque;
    }
}
