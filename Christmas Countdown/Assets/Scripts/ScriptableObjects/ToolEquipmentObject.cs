using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating new Tool equipment scriptable objects
/// Deriving from base equipment scriptable object class
/// Sets type automatically on enable
/// </summary>
[CreateAssetMenu(fileName = "New Tool Equipment", menuName = "Equipment/Tool")]
public class ToolEquipmentObject : BaseEquipmentObject
{
    /// <summary>
    /// Implement abstract OnEnable function from base class to set type when creating object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Tool;
    }
}
