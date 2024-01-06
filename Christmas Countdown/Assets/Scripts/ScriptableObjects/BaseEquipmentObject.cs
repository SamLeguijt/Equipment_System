using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for equipement objects
/// Derives from ScriptableObject
/// </summary>
public class BaseEquipmentObject : ScriptableObject
{
    // Type of this equipment
    private EquipmentType equipmentType;

    [Tooltip("Name of the object")]
    [SerializeField] private string equipmentName;

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
    /// Type Property of the equipment <br/>
    /// Protected set to set value in child classes
    /// </summary>
    public EquipmentType EquipmentType
    {
        get { return equipmentType; }
        protected set { equipmentType = value; }
    }
}

/// <summary>
/// Enum for types of Equipment objects
/// </summary>
public enum EquipmentType
{
    Weapon,
    Ammunition,
    Throwables,
    Tools,
    Apparel
}