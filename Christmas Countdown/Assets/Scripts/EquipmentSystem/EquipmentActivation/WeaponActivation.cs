using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : MonoBehaviour, IEquipmentActivation
{
    [Tooltip("Prefab of the bullet object to clone upon firing")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Transform point where bullets should fire from, without offset")]
     private Transform firepoint;

    [Tooltip("Offset in position for the firepoint where bullets should fire from")]
    [SerializeField] private Vector3 firepointOffset;

    [Tooltip("Start rotation of the bullet prefab, set when instantiating a new one")]
    [SerializeField] private Vector3 bulletRotation;

    [Tooltip("Force of what the bullet should fire with, applied to the rigidbody")]
    [SerializeField] private float bulletForce = 25f;

    [Tooltip("Max hit range of the ray used for getting the point to fire to")]
    [SerializeField] private float maxHitRange = 50f;


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
    public void Initialize(Transform _targetFirepoint)
    {
        // First get our equipmentBehaviour attached to same object
        EquipmentBehaviour behaviour = GetComponent<EquipmentBehaviour>();

        // Get the data of our object, casted as weapon (weapon activation so is a weapon)
        WeaponEquipmentObject data = (WeaponEquipmentObject)behaviour.EquipmentData;

        // Set this firepoint to the param
        firepoint = _targetFirepoint;

        SetWeaponValues(data);
    }

    private void SetWeaponValues(WeaponEquipmentObject _weaponData)
    {
        // Set the bullet for this activation to the reference on the data
        bulletPrefab = _weaponData.bulletToFire;

        bulletForce = _weaponData.bulletSpeed;

        maxHitRange = _weaponData.maxHitRange;
    }

    /// <summary>
    /// Overridden method that runs when player input is received <br/>
    /// Shoots a bullet toward the mouse cursor
    /// </summary>
    public void Activate()
    {
        // Call method to fire a bullet upon activation
        FireBullet();
    }

    private void FireBullet()
    {
        // Instantiate new bullet at the firepoint position, with an pos offset and  
        GameObject bullet = Instantiate(bulletPrefab, (firepoint.position + firepointOffset), Quaternion.Euler(bulletRotation));

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
            rb.AddForce(shootingDirection * bulletForce, ForceMode.Impulse);
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
        if (Physics.Raycast(ray, out hit, maxHitRange))
        {
            // If the ray hits an object within the shooting range, return the hit point
            return hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, return a point at the maximum shooting range
            return ray.GetPoint(maxHitRange);
        }
    }
}
