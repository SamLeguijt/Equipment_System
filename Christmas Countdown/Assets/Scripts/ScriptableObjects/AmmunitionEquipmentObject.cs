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

    public int bulletsAmount;

    public int bulletsPerShot;
    public int bulletSpread;

    public GameObject bulletPrefab;

    public Bullet bulletInfo;

    public float destroyAfterActivationDelay;

    // Reference to bullet object (prefab
    // Reference to the bullet script
    // Reference to bullet script object 

    // Change bullet script with scriptable object of bullet (later)


    /// <summary>
    /// Implement abstract method from base class and set the equipment type of this object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Ammunition;

        bulletInfo = bulletPrefab.GetComponent<Bullet>();
    }
}
