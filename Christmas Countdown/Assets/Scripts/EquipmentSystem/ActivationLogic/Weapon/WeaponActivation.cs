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
        if (currentAmmoCapacity > 0)
        {
            for (int i = 0; i < weaponData.BaseAmmoClip.BulletsPerFire; i++)
            {
                FireBullet();
            }
        }
    }

    private void FireBullet()
    {
        Vector3 fireDirection = GetFireDirectionToMouse(firepoint.position);
  
        Quaternion startRotation = GetBulletStartRotationTowards(fireDirection);

        // Instantiate new bullet at the firepoint position, with an pos offset and  
       // GameObject bullet = Instantiate(currentBullet, firepoint.position, startRotation);

        currentAmmoCapacity--;

        Debug.Log(currentAmmoCapacity);

        // Get the base fire direction towards the mouse
        Vector3 baseFireDirection = GetFireDirectionToMouse(firepoint.position);

        // Apply bullet spread
        Vector3 spreadDirection = ApplyBulletSpread(baseFireDirection);


        GameObject bullet = Instantiate(currentBullet, firepoint.position, Quaternion.LookRotation(spreadDirection));

        // Find the rigidbody on the bullet prefab
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Check if its not null 
        if (rb != null)
        {
            // Add force using our bulletForce, and forcemode impulse
            rb.AddForce(spreadDirection * currentBulletSpeed, ForceMode.Impulse);
        }
        else // Throw error
        {
            throw new System.Exception("Rigidbody on bullet prefab is missing, assign please, then try to fire bullets again");
        }
    }

    private Vector3 ApplyBulletSpread(Vector3 baseDirection)
    {
        // Calculate random spread angle within the specified range
        float spreadAngle = Random.Range(-weaponData.BaseAmmoClip.BulletSpread / 2f, weaponData.BaseAmmoClip.BulletSpread / 2f);

        // Rotate the base direction with the spread angle
        Vector3 spreadDirection = Quaternion.Euler(spreadAngle, spreadAngle, spreadAngle) * baseDirection;

        return spreadDirection.normalized;
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
        bulletStartRotation = ammoClip.BulletData.BulletStartRotation;

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
    /// Returns a Quaternion rotated towards targetDirection multiplied with current bullet's startrotation
    /// </summary>
    /// <param name="_targetDirection"></param>
    /// <returns></returns>
    private Quaternion GetBulletStartRotationTowards(Vector3 _targetDirection)
    {
        // Get initial rotation towards param point
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);

        // Get the start rotation from the current bullet prefab
        Vector3 bulletFireRotation = currentBullet.GetComponent<BulletBehaviour>().BulletData.BulletStartRotation;

        // Multiply the target direction with the start rotation of the bullets data
        targetRotation *= Quaternion.Euler(bulletFireRotation);

        // Return the multiplied vector
        return targetRotation;
    }

    /// <summary>
    /// Returns a normalized vector from param to the mouse position
    /// </summary>
    /// <param name="_fromVector"></param>
    /// <returns></returns>
    private Vector3 GetFireDirectionToMouse(Vector3 _fromVector)
    {
        // Get the mouse position 
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from the main camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Call method to get the targetPoint, depending on the max range of our ray
        Vector3 targetPoint = GetTargetPoint(ray);

        // Calculate direction for the bullet by subtracting firepoint pos from the mouse pos
        Vector3 shootingDirection = targetPoint - _fromVector;

        return shootingDirection.normalized;
    }
}
