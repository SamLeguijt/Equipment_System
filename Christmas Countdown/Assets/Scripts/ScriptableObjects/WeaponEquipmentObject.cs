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
    [Space]
    [Header("WeaponEquipment specifics")]
    public GameObject bulletToFire;

    public float maxHitRange;

    public float bulletSpeed;

    /// <summary>
    /// OnEnable method to set weapon type automatically when creating new object of this type
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Weapon;
    }
}
