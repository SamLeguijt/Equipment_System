using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating new Ammunition scriptable objects
/// Deriving from base scriptable object equipment class
/// </summary>
[CreateAssetMenu(fileName = "New Ammunition Equipment", menuName = "Equipment/Ammunition")]
public class AmmunitionEquipmentObject : BaseEquipmentObject
{


    /// <summary>
    /// Implement abstract method from base class and set the equipment type of this object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Ammunition;
    }
}
