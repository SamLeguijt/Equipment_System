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

    /* ------------------------------------------  VARIABLES ------------------------------------------- */

    [Space]
    [Header("-----------------------  AmmunitionEquipment specifics   -----------------------------------")]
    [Space]

    [Tooltip("Reference to the prefab that will be instantiated after reloading with this ammo")]
    [SerializeField] private GameObject bulletPrefab;
   
    [Tooltip("How many bullets this ammunition object holds")]
    [SerializeField] private int bulletsAmount;

    [Tooltip("How many bullets per fire will be shot using this ammo clip")]
    [SerializeField] private int bulletsPerFire;

    [Tooltip("How wide the bullets spread when firing multiple bullets")]
    [SerializeField] private float bulletSpread;

    [Tooltip("Delay in seconds to destroy this object after activating")]
    [SerializeField] private float destroyAfterActivationDelay;


    // Ref to the ScriptableObject holding the bullet's data, Will be assigned by looking at prefab
    private BulletObject bulletData;



    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /// <summary>
    /// Reference to the prefab that gets shot using this ammo, Read- only
    /// </summary>
    public GameObject BulletPrefab { get { return bulletPrefab; } }

    /// <summary>
    /// Reference to the Bullet's data used for this AmmunitionObject.
    /// </summary>
    public BulletObject BulletData { get { return bulletData; } }

    /// <summary>
    /// Reference to the amount of bullets this ammunition holds, read-only
    /// </summary>
    public int BulletsAmount { get { return bulletsAmount; } }

    /// <summary>
    /// Reference to how many bullets will be fired per shot, read-only
    /// </summary>
    public int BulletsPerFire {  get { return bulletsPerFire; } }

    /// <summary>
    /// Reference to the spread between bullets, read-only
    /// </summary>
    public float BulletSpread { get { return bulletSpread; } }

    /// <summary>
    /// Reference to the delay to destroy this object after activating, read- only
    /// </summary>
    public float DestroyAfterActivationDelay {  get { return destroyAfterActivationDelay; } }



    /* ------------------------------------------  METHODS ------------------------------------------- */


    /// <summary>
    /// Implement abstract method from base class and set the equipment type of this object
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Ammunition;

        // Assign the data value by looking in the prefab
        bulletData = bulletPrefab.GetComponent<BulletBehaviour>().BulletData;
    }
}
