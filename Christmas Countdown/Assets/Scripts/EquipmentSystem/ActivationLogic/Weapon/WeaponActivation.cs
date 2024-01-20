using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : MonoBehaviour, IEquipmentActivation
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    [Tooltip("Transform point where bullets should fire from. Place at correct position relative to weapon barrel")]
    [SerializeField] private Transform firepoint;

    /* Private variables for inside this class only */

    // Reference to the weapon object for data
    private WeaponEquipmentObject weaponData;

    // Store the current bullet being fired
    private GameObject currentBullet;

    // Store the rotation of the bullet when firing
    private Vector3 bulletStartRotation;

    // Store the current bullet's speed 
    private float currentBulletSpeed;

    // Reference the current ammo capacity of this weapon
    private int currentAmmoCapacity;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /// <summary>
    /// Reference to this weapon's data
    /// </summary>
    public WeaponEquipmentObject WeaponData {  get { return weaponData; } }



    /* ------------------------------------------  METHODS ------------------------------------------- */



    // Start is called before the first frame update
    void Start()
    {
        // Reset position to be at Zero relative to parent
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Public method gets called when this script gets added to equipment object <br/>
    /// Sets the firepoint and bullet to fire of this weapon
    /// </summary>
    /// <param name="_targetFirepoint"></param>
    public void Initialize(EquipmentBehaviour _myEquipment, Transform _targetFirepoint)
    {
        // Get the tooldata in correct type by casting the behaviour's data reference
        weaponData = (WeaponEquipmentObject)_myEquipment.EquipmentData;

        _myEquipment.activationLogic = this;

        // Set this firepoint to the param
        firepoint = _targetFirepoint;

        // Reload the weapon with the base ammo clip 
        Reload(weaponData.BaseAmmoClip);
    }


    /// <summary>
    /// Overridden method that runs when player input is received <br/>
    /// Shoots a bullet toward the mouse cursor
    /// </summary>
    public void Activate()
    {
        if (currentAmmoCapacity >0 )
        {
            // Call method to fire a bullet upon activation
            FireBullet();
        }
    }

    private void FireBullet()
    {
        // Instantiate new bullet at the firepoint position, with an pos offset and  
        GameObject bullet = Instantiate(currentBullet, (firepoint.position), Quaternion.Euler(bulletStartRotation));

        currentAmmoCapacity--;

        Debug.Log(currentAmmoCapacity);

        // Get the mouse position 
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from the main camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Call method to get the targetPoint, depending on the max range of our ray
        Vector3 targetPoint = GetTargetPoint(ray);

        // Calculate direction for the bullet by subtracting firepoint pos from the mouse pos
        Vector3 shootingDirection = targetPoint - firepoint.position;

        // Normalize to get the direction vector
        shootingDirection.Normalize();

        // Find the rigidbody on the bullet prefab
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Check if its not null 
        if (rb != null)
        {
            // Add force using our bulletForce, and forcemode impulse
            rb.AddForce(shootingDirection * currentBulletSpeed, ForceMode.Impulse);
        }
        else // Throw error
        {
            throw new System.Exception("Rigidbody on bullet prefab is missing, assign please, then try to fire bullets again");
        }
    }

    /// <summary>
    /// Method to get a hit point on trajectory ray <br/>
    /// Returns a point as Vector3 representing either the end of our range, or the point where the ray collides with something
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    Vector3 GetTargetPoint(Ray ray)
    {
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, weaponData.MaxHitDistance))
        {
            // If the ray hits an object within the shooting range, return the hit point
            return hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, return a point at the maximum shooting range
            return ray.GetPoint(weaponData.MaxHitDistance);
        }
    }

    /// <summary>
    /// Method to reload the weapon object with new ammo object, refilling the current ammo <br/>
    /// Sets the bullet that will be fired to param bullets, as well as other bullet specific values 
    /// </summary>
    /// <param name="ammoClip"></param>
    public void Reload(AmmunitionEquipmentObject ammoClip)
    {
        // Set new bullet to fire
        currentBullet = ammoClip.BulletPrefab;

        // Set the rotation 
        bulletStartRotation = ammoClip.BulletData.BulletFireRotation;

        // Set speed of the bullet
        currentBulletSpeed = ammoClip.BulletData.BulletFireSpeed;

        // Call method to refill ammo
        RefillAmmo(ammoClip.ClipSize);
    }

    /// <summary>
    /// Sets the value of the current ammo to the param value, keeping it within max capacity of weapon
    /// </summary>
    /// <param name="_newAmmo"></param>
    private void RefillAmmo(int _newAmmo)
    {
        // Refill ammo by setting current to new ammo
        currentAmmoCapacity = _newAmmo;

        // Keep current ammo within the max of its weapon
        if (currentAmmoCapacity > weaponData.MaxAmmoCapacity)
        {
            currentAmmoCapacity = weaponData.MaxAmmoCapacity;
        }
    }
}
