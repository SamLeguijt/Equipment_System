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

    [SerializeField] private CameraController cameraController; // CameraController used for orientation

    /* Variables used for properties */
    [SerializeField] private EquipmentBehaviour equipmentBehaviour;
    [SerializeField] private Collider objectCollider;
    [SerializeField] private Rigidbody rb;

    private int layerToCheckEnvironmentCollisions;
    private float secondsGroundedAfterCollision = 0.5f;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Read-onl property for reference to this object's equipment behaviour
    /// </summary>
    public EquipmentBehaviour EquipmentBehaviour
    {
        get { return equipmentBehaviour; }
    }

    /// <summary>
    /// Read-only property for reference to the kind of collider used for this equipment
    /// </summary>
    public Collider ObjectCollider
    {
        get { return objectCollider; }
    }

    /// <summary>
    /// Read-only Property for reference to the rigidbody of this object
    /// </summary>
    public Rigidbody Rb
    {
        get { return rb; }
    }

    /// <summary>
    /// Read-only property for getting the layer to check this object's colliion with
    /// </summary>
    public int LayerToCheckEnvironmentCollisions
    {
        get { return layerToCheckEnvironmentCollisions; }
    }

    /// <summary>
    /// Public property to get or set the time it takes between hitting ground and enable picking up object again
    /// </summary>
    public float SecondsGroundedAfterCollision
    {
        get { return secondsGroundedAfterCollision; }
        set { secondsGroundedAfterCollision = value; }
    }

    /* ------------------------------------------  METHODS ------------------------------------------- */


    private void Start()
    {
        if (DeveloperSettings.instance.AutoReferenceEquipmentComponents_OnStart)
        {
            // Get the equipmentbehaviour by looking in our children (this gets added by the script to parent, so must be in children)
            equipmentBehaviour = GetComponentInChildren<EquipmentBehaviour>();

            // Get the collider attached to this object
            objectCollider = GetComponent<Collider>();

            cameraController = Camera.main.GetComponent<CameraController>();

        }

        if (DeveloperSettings.instance.AutoAddEquipmentComponents_OnStart)
        {
            // Add and assign rigidbody through here 
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Check if we found it to prevent manually putting this script on objects, or EquipBehaviour not as a child object
        if (equipmentBehaviour == null || objectCollider == null || rb == null || cameraController == null)
        {
            //gameObject.SetActive(false);
            //Show error message and destroy if not found in children
            throw new System.Exception($"One or more components and references are missing from {gameObject.name}! Please assign components, then re-run. Disabling {this.name} for now.");
        }
        else // Components found, everything good
        {
            // Initialize this class methods and properties
            InitializeEquipmentPhysicsManager();
        }
    }

    /// <summary>
    /// Method to start up this class and it's properties
    /// </summary>
    public void InitializeEquipmentPhysicsManager()
    {
        // Set the collision detection mode to prevent not calling correctly
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Assign value to layer which is used to check collision with ground and more
        layerToCheckEnvironmentCollisions = LayerMask.NameToLayer(equipmentBehaviour.EnvironmentLayerName);
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
                // Start coroutine to enable pick up again after touching ground
                StartCoroutine(SetGroundBoolTrueAfterSeconds(SecondsGroundedAfterCollision));
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

    /// <summary>
    /// Coroutine used to set the IsOnGround bool to true after the param in seconds <br/>
    /// Allows equipment to come (close) to stop 
    /// </summary>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    private IEnumerator SetGroundBoolTrueAfterSeconds(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        equipmentBehaviour.IsOnGround = true;
    }
}
