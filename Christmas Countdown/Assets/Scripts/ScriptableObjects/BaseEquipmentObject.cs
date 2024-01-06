using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for equipement objects
/// Derives from ScriptableObject
/// </summary>
public class BaseEquipmentObject : ScriptableObject
{
    [Tooltip("Name of the object")]
    private string equipmentName;

    // Type of this equipment
    private EquipmentType equipmentType;

    [Tooltip("Description of the equipment object, optionally")]
    [TextArea(10, 20)]
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
/// Enum for 
/// </summary>
public enum EquipmentType
{
    Weapon,
    Ammo,
    Throwables,
    Tools,
    Apparel
}