using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for creating Weapon scriptable objects
/// Automatically sets weapon type in OnEnable function
/// </summary>
[CreateAssetMenu(fileName = "New Weapon Equipment", menuName = "Equipment/Weapon")]
public class WeaponEquipmentObject : BaseEquipmentObject
{
    /// <summary>
    /// OnEnable method to set weapon type automatically when creating new object of this type
    /// </summary>
    private void OnEnable()
    {
        EquipmentType = EquipmentType.Weapon;
    }
}
