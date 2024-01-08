using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for equipement objects
/// Derives from ScriptableObject
/// </summary>
public abstract class BaseEquipmentObject : ScriptableObject
{
    // Type of this equipment
    private EquipmentType equipmentType;

    [Tooltip("Name of the object")]
    [SerializeField] private string equipmentName;

    [Tooltip("Distance from where player is able to equip this object")]
    [SerializeField] private float equipDistance;

    [Tooltip("Description of the equipment object, optionally")]
    [TextArea(5, 20)]
    [SerializeField] private string equipmentDescription;

    /// <summary>
    /// Read only property to get the name of the equipment <br/>
    /// </summary>
    public string EquipmentName
    {
        get { return equipmentName; }
    }

    /// <summary>
    /// Read only property used for getting the distance from where to equip this object
    /// </summary>
    public float EquipDistance
    {
        get { return equipDistance; }
    }

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
    /// Protected abstract OnEnable function to set individual equipment types on enable
    /// </summary>
    protected abstract void OnEnable();
}

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