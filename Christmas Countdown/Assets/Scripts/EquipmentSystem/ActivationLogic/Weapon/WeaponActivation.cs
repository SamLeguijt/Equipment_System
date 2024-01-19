using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : MonoBehaviour, IEquipmentActivation
{
    [Tooltip("Transform point where bullets should fire from. Place at correct position relative to weapon barrel")]
    [SerializeField] private Transform firepoint;

    WeaponEquipmentObject weaponData;

    GameObject bulletToFire;

    Vector3 bulletRotation;

    float bulletSpeed;

    private int currentAmmoCapacity;

    private int maxAmmoCapacity;

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

        bulletToFire = weaponData.BulletToFire;

        bulletRotation = weaponData.BulletStartRotation;

        bulletSpeed = weaponData.BulletSpeed;

        maxAmmoCapacity = weaponData.MaxClipCapacity;
        currentAmmoCapacity = maxAmmoCapacity;
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
        GameObject bullet = Instantiate(bulletToFire, (firepoint.position), Quaternion.Euler(bulletRotation));

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
            rb.AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
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

    public void Reload(AmmunitionEquipmentObject ammoClip)
    {
        bulletToFire = ammoClip.bulletPrefab;

        bulletRotation = ammoClip.bulletInfo.bulletRotation;

        bulletSpeed = ammoClip.bulletInfo.bulletSpeed;

        currentAmmoCapacity = ammoClip.bulletsAmount;

        if (currentAmmoCapacity > maxAmmoCapacity)
        {
            currentAmmoCapacity = maxAmmoCapacity;
        }

        /*  Bullets: 
         *  Bullet has damage, max distance, speed etc
         *  Ammunition object has a reference to the bullet prefab as object.
         *  The bullet script has a script object reference for its values (distance, speed, damage)
         *  
         * 
         */


        /* AK47 can shoot 30 bullets per clip <- MAxClipSize = 30 
         * It can not reload without picking up an ammo object <- Can't fire if currentAmmo <= 0, cant call Reload();
         * By activating the ammo object, ak47 reloads <- AmmoActivation calls Reload(); 
         * reload: fill currentBullets till maxClipsize <- currentAmmo = AmmoClip.BulletsAmount : if (currentAmmo > maxClipSize) currentammo = maxclipsize
         * 
         * So:
         * WeaponObject has MaxClipSize variable 
         * WeaponActivation has currentAmmo, that sets to max On Initialize()
         * FireBullet does -1 on currentAmmo
         *  In Activate: if (currentAmmo <= 0) return;  else{ FireBullet}
         *  
         *  Reload method gets called in AmmoActivation on activation if other hand is holding a weapon. 
         *  Reload () receives an AmmoObject to get its data and does:
         *  currentAmmo set to the bullets in the data
         *  bullet to fire to bullet prefab in the data
         *  Can make multiple bullet prefabs to make different ammo types, referencing the prefabs in ammo object
         *  
         *  AmmoActivation: Reload() and Destroy after reloading (ammo clip should be consumed when reloading)
         */
    }
}
