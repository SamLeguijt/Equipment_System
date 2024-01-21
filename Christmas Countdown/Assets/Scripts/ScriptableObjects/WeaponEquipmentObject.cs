using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Tooltip("Starting ammo clip for this weapon")]
    [SerializeField] private AmmoClipObject baseAmmoClip;

    [Tooltip("Specify what type of weapon this object is")]
    [SerializeField] private TypeOfWeapon weaponType;

    [Tooltip("Maximum range bullets from this weapon can fire to")]
    [SerializeField] private float maxHitDistance;

    [Tooltip("Start rotation of the bullet when flying out of the weapon")]
    [SerializeField] private Vector3 bulletStartRotation;

    [Tooltip("Maximum amount of bullets per clip for this weapon")]
    [SerializeField] private int maxClipCapacity;

    [Tooltip("Array containing the possible fire modes for this weapon. First index is starting fire mode")]
    [SerializeField] private WeaponFireMode[] fireModes;

    [Tooltip("Represents how many shots a round of burst fires per activation")]
    [SerializeField] private int burstShootCount;

    [Tooltip("Delay between shots in burst fire mode")]
    [SerializeField] private float burstShootDelay;

    [Tooltip("Delay between shots in full auto fire mode")]
    [SerializeField] private float fullAutoShootDelay;
    
    // Reference to the key to swap fire modes (Not adjustable in inspector to set default key)
    private KeyCode fireModeSwapKey;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// The starting ammo clip of the weapon, read-only
    /// </summary>
    public AmmoClipObject BaseAmmoClip { get { return baseAmmoClip; } }

    /// <summary>
    /// Type of weapon this object is, read-only
    /// </summary>
    public TypeOfWeapon WeaponType { get { return weaponType; } }   

    /// <summary>
    /// Read only reference to the max hit distance this weapon can shoot
    /// </summary>
    public float MaxHitDistance { get { return maxHitDistance; } }

    /// <summary>
    /// Read only reference to the start rotation of this weapon's bullets
    /// </summary>
    public Vector3 BulletStartRotation { get { return bulletStartRotation; } }

    /// <summary>
    /// Maximum size of bullets this weapon holds per clip
    /// </summary>
    public int MaxAmmoCapacity { get { return maxClipCapacity; } }

    /// <summary>
    /// Possible fire modes for this weapon, stored in an array (read-only)
    /// </summary>
    public WeaponFireMode[] FireModes { get { return fireModes; } }

    /// <summary>
    /// Representing the amount of shots fired per burst activation, read-only
    /// </summary>
    public int BurstShootCount {  get { return burstShootCount; } } 

    /// <summary>
    /// Delay between shots in burst mode, read-only
    /// </summary>
    public float BurstShootDelay {  get { return burstShootDelay; } }

    /// <summary>
    /// Delay between shots in full-auto mode, read-only
    /// </summary>
    public float FullAutoShootDelay { get { return fullAutoShootDelay; } }

    /// <summary>
    /// Reference to the key to swap to another fire mode, read-only
    /// </summary>
    public KeyCode FireModeSwapKey {  get { return fireModeSwapKey; } }



    /* ------------------------------------------  METHODS ------------------------------------------- */

    /// <summary>
    /// OnEnable method to set weapon type automatically when creating new object of this type
    /// </summary>
    protected override void OnEnable()
    {
        EquipmentType = EquipmentType.Weapon;
        fireModeSwapKey = KeyCode.Space;
    }
}

/// <summary>
/// Enum for specifying the type of weapon
/// </summary>
public enum TypeOfWeapon
{
    AssaultRifle,
    Shotgun,
    Submachine,
    Pistol,
    Rocket,
    Sniper,
    All // To reload all kind of weapons
}
