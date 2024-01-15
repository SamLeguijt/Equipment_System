using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating new Throwables equipment scriptable objects
/// Deriving from base equipment scriptable object class
/// Sets type automatically on enable
/// </summary>
[CreateAssetMenu(fileName = "New Throwable Equipment", menuName = "Equipment/Throwable")]
public class ThrowablesEquipmentObject : BaseEquipmentObject
{
    /// <summary>
    /// Implement abstract OnEnable function from base class to set type
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Throwable;
    }

    public override void Activate()
    {
        Debug.Log("I am throwable activate");
    }
}
