using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Clip", menuName = "Equipment/Ammunition/Ammo clip")]
public class AmmoClipObject : AmmunitionEquipmentObject
{
    [Space]
    [Header("Ammo clip specifics")]
    [Space] 

    [Tooltip("The weapon type this ammo belongs to")]
    [SerializeField] private TypeOfWeapon weaponTarget;
    
    /// <summary>
    /// Override on enable method to set type of equipment and bullet data
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();

        // Set target weapon to our weapon target
        targetWeapon = weaponTarget;
    }
}
