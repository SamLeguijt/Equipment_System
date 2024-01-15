using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating new Apparel equipment scriptable objects
/// Deriving from base equipment scriptable object class
/// Sets type automatically on enable
/// </summary>
[CreateAssetMenu(fileName = "New Apparel Equipment", menuName = "Equipment/Apparel")]
public class ApparelEquipmentObject : BaseEquipmentObject
{
    /// <summary>
    /// Implement abstract OnEnable function from base class to set type when creating object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Apparel;
    }

    public override void Activate()
    {
        Debug.Log("I am apparel activate");
    }
}
