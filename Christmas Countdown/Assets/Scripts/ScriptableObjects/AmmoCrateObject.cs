using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Crate", menuName = "Equipment/Ammunition/Ammo crate")]
public class AmmoCrateObject : AmmunitionEquipmentObject
{
    protected override void OnEnable()
    {
        base.OnEnable();

        // Set target weapon to all to reload every weapon
        targetWeapon = TypeOfWeapon.All;
    }
}
