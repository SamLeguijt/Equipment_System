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

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Space]
    [Header("-----------------------  WeaponEquipment specifics   -----------------------------------")]
    [Space]

    [Tooltip("The bullet prefab to fire for this weapon")]
    [SerializeField] private GameObject bulletToFire;

    [Tooltip("Maximum range bullets from this weapon can fire to")]
    [SerializeField] private float maxHitDistance;

    [Tooltip("Speed of what the bullet from this weapon flies with")]
    [SerializeField] private float bulletSpeed;

    [Tooltip("Start rotation of the bullet when flying out of the weapon")]
    [SerializeField] private Vector3 bulletStartRotation;

    [Tooltip("Maximum amount of bullets per clip for this weapon")]
    [SerializeField] private int maxClipCapacity;



    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Read only reference to the bullet to fire from this weapon
    /// </summary>
    public GameObject BulletToFire { get { return bulletToFire; } }
    
    /// <summary>
    /// Read only reference to the max hit distance this weapon can shoot
    /// </summary>
    public float MaxHitDistance { get { return maxHitDistance; } }
    
    /// <summary>
    /// Read only reference to the bullet speed for this weapon
    /// </summary>
    public float BulletSpeed { get { return bulletSpeed; } }

    /// <summary>
    /// Read only reference to the start rotation of this weapon's bullets
    /// </summary>
    public Vector3 BulletStartRotation {  get { return bulletStartRotation; } }


    public int MaxClipCapacity { get { return maxClipCapacity; } }  

    /* ------------------------------------------  METHODS ------------------------------------------- */


    /// <summary>
    /// OnEnable method to set weapon type automatically when creating new object of this type
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Weapon;
    }
}
