using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for types of Equipment objects
/// </summary>
public enum EquipmentType
{
    Weapon,
    Ammunition,
    Throwable,
    Tool,
    Apparel
}

/// <summary>
/// Base class for equipement objects
/// Derives from ScriptableObject
/// </summary>
public abstract class BaseEquipmentObject : ScriptableObject
{
    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    // Type of this equipment
    private EquipmentType equipmentType;

    [Space]
    [Header("Descriptive variables")]
    [Space]

    [Tooltip("Name of the object")]
    [SerializeField] private string equipmentName;

    [Tooltip("Description of the equipment object, optionally")]
    [TextArea(5, 20)]
    [SerializeField] private string equipmentDescription;

    [Space] [Space] 
    [Header("Position variables")]
    [Space]
   
    [Tooltip("Position offset for equipped in left hand")]
    [SerializeField] private Vector3 leftHandPositionOffset;

    [Tooltip("Position offset for equipped in left hand")]
    [SerializeField] private Vector3 rightHandPositionOffset;

    [Space] [Space]
    [Header("Rotation variables")]
    [Space]

    [Tooltip("Vector representing the quaternion of the rotation of the object when equipped")]
    [SerializeField] private Vector3 equippedRotation;

    [Tooltip("Vector representing the quaternion of the rotation of the object when not equipped")]
    [SerializeField] private Vector3 unequippedRotation;

    [Range(0f, 50f)]
    [Tooltip("Speed of which to rotate this equipment toward the cursor")]
    [SerializeField] private float rotationSpeed;
    
    [Space] [Space]
    [Header("EquipmentSystem and behaviour variables")]
    [Space]

    [Tooltip("Force of which to drop the equipment with, applies the force in upward-forward motion to throw equipment on drop")]
    [SerializeField] private Vector2 equipmentDropForce;

    [Tooltip("Distance from where player is able to equip this object")]
    [SerializeField] private float equipDistance;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /// <summary>
    /// Type Property of the equipment <br/>
    /// Protected set to set value in child classes
    /// </summary>
    public EquipmentType EquipmentType
    {
        get { return equipmentType; }
        protected set { equipmentType = value; }
    }


    /// <summary>
    /// Read only property to get the name of the equipment <br/>
    /// </summary>
    public string EquipmentName
    {
        get { return equipmentName; }
    }

    /// <summary>
    /// Read only property to get the desription of this object
    /// </summary>
    public string EquipmentDescription
    {
        get { return equipmentDescription; }
    }

    /// <summary>
    /// Property to get or set the left hand position offset of this equipment <br/>
    /// Protected set to enable setting in child classes
    /// </summary>
    public Vector3 LeftHandPositionOffset
    {
        get { return leftHandPositionOffset; }
        protected set { leftHandPositionOffset = value; }
    }

    /// <summary>
    /// Property to get or set the right hand position offset of this equipment <br/>
    /// Protected set to enable setting in child classes
    /// </summary>
    public Vector3 RightHandPositionOffset
    {
        get { return rightHandPositionOffset; }
        protected set {  rightHandPositionOffset = value; }
    }

    /// <summary>
    /// Property to define the rotation of this equipment when in hand
    /// Protected set to set for child objects
    /// </summary>
    public Vector3 EquippedRotation
    {
        get { return equippedRotation; }
        protected set { equippedRotation = value; }
    }

    /// <summary>
    /// Property to define the rotation of this equipment when not in hand (on ground)
    /// Protected set to set for child objects
    /// </summary>
    public Vector3 UnequippedRotation
    {
        get { return unequippedRotation; }
        protected set { unequippedRotation = value; }
    }

    /// <summary>
    /// Property to define the speed to rotate towards the mouse cursor for this object <br/>
    /// Protected set to define for each type of equipment
    /// </summary>
    public float RotationSpeed
    {
        get { return rotationSpeed; }
        protected set { rotationSpeed = value; }
    }

    /// <summary>
    /// Property to define the force of which to apply to the equipment when dropping it <br/>
    /// Vector2 for forward-upward forces only <br/>
    /// Protected set to enable defining in child classes
    /// </summary>
    public Vector2 EquipmentDropForce
    {
        get { return equipmentDropForce; }
        protected set { equipmentDropForce = value; }
    }

    /// <summary>
    /// Read only property used for getting the distance from where to equip this object
    /// </summary>
    public float EquipDistance
    {
        get { return equipDistance; }
    }


    /* ------------------------------------------  METHODS ------------------------------------------- */


    /// <summary>
    /// Protected abstract OnEnable function to set individual equipment types on enable
    /// </summary>
    protected abstract void OnEnable();
}
