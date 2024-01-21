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

    private EquipmentBehaviour equipmentBehaviour;

    // Store the current bullet being fired
    private GameObject currentBullet;

    // Store the rotation of the bullet when firing
    private Vector3 bulletStartRotation;

    // Store the current bullet's speed 
    private float currentBulletSpeed;

    // Reference the current ammo capacity of this weapon
    private int currentAmmoCapacity;

    private bool isFullAutoCoroutineStarted;

    private bool canAutomaticFire;

    private WeaponFireMode currentFireMode;

    private int currentFireModeIndex;

    /* ------------------------------------------  PROPERTIES ------------------------------------------- */


    /// <summary>
    /// Reference to this weapon's data
    /// </summary>
    public WeaponEquipmentObject WeaponData { get { return weaponData; } }



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

        equipmentBehaviour = _myEquipment;

        // Set this firepoint to the param
        firepoint = _targetFirepoint;

        // Reload the weapon with the base ammo clip 
        Reload(weaponData.BaseAmmoClip);

        currentFireModeIndex = 0;
        SwitchFireMode(currentFireModeIndex);
    }

    private void Update()
    {

        /* TODO:
         * 
         * Make variables and properties for amount of shots per burst, delay between burst shot, delay between full auto shots
         * Optimize updaye method to only check while weapon is in hand
         */




        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentFireModeIndex >= weaponData.fireModes.Length)
            {
                currentFireModeIndex = 0;
            }

            SwitchFireMode(currentFireModeIndex);
        }

        if (isFullAutoCoroutineStarted)
        {
            // Check if the left mouse button is released
            if (Input.GetKeyUp(equipmentBehaviour.CurrentHand.activationKey) || equipmentBehaviour.CurrentHand == null)
            {
                // Stop the firing coroutine
                StopCoroutine(AutomaticFire());

                isFullAutoCoroutineStarted = false;
            }
        }
    }

    private void SwitchFireMode(int _targetIndex)
    {

        currentFireMode = weaponData.fireModes[_targetIndex];
        currentFireModeIndex++;

        Debug.Log(currentFireMode);
    }
    private IEnumerator AutomaticFire()
    {
        isFullAutoCoroutineStarted = true;

        while (isFullAutoCoroutineStarted)
        {
            PerformShot();

            // Wait for the specified interval before firing again
            yield return new WaitForSeconds(.2f);
        }
    }

    /// <summary>
    /// Overridden method that runs when player input is received <br/>
    /// Performs a shoot action depending on the current fire mode of the weapon
    /// </summary>
    public void Activate()
    {
        // Switch statement for switching between the current fire mode
        switch (currentFireMode)
        {
            // Semi automatic only fires one bullet a time, so just call function to shoot once on activation call
            case WeaponFireMode.SemiAutomatic:

                PerformShot();

                break;
            // Full automatic fires bullet with a small delay while the activation key is being held down
            case WeaponFireMode.FullAutomatic:

                // Check if the activation key for the current hand is being held down
                if (Input.GetKey(equipmentBehaviour.CurrentHand.activationKey))
                {
                    // Check if the corotoutine has started already 
                    if (!isFullAutoCoroutineStarted)
                    {
                        // Start it if not started
                        StartCoroutine(AutomaticFire());
                    }
                }
                break;
            // Burst fire mode performs multiple shots in quick succession (Shots, not just bullets)
            case WeaponFireMode.BurstFire:

                // Start coroutine to fire a burstfire shot
                StartCoroutine(BurstFireShot(weaponData.burstShotAmount));
                break;
            default:
                break;
        }
    }


    private void PerformShot(int _ammoThreshold = 1)
    {
        // Only fire if we have > threshold ammo
        if (currentAmmoCapacity >= _ammoThreshold)
        {
            // Fire a bullet for every bullet a single shot should fire (according to weaponData) 
            for (int i = 0; i < weaponData.BaseAmmoClip.BulletsPerShot; i++)
            {
                // Call method to fire bullet
                FireBullet();
            }
        }
    }

    /// <summary>
    /// Coroutine that shoots a burst of shots 
    /// </summary>
    /// <param name="_shotsToFire"> How many shots should the burst fire? </param>
    /// <returns></returns>
    private IEnumerator BurstFireShot(int _shotsToFire)
    {
        // Perform a shot and wait a small delay for param shots
        for (int i = 0; i < _shotsToFire; i++)
        {
            // Call method to fire 'weaponData.BulletsPerShot' bullets
            PerformShot();

            // Wait a small delay between the shots to make it burst
            yield return new WaitForSeconds(.1f);
        }
    }
    
    /// <summary>
    /// Fires a bullet from the firepoint.position towards the direction of the mouse
    /// </summary>
    /// <param name="_applyBulletSpread"> Should there be bullet spread applied to the bullet(s)? Default to true</param>
    /// <exception cref="System.Exception"></exception>
    private void FireBullet(bool _applyBulletSpread = true)
    {
        // Get the direction to fire in
        Vector3 fireDirection = GetFireDirectionToMouse(firepoint.position, _applyBulletSpread);

        // Get Quaterion to get correct starting rotation towards the fire direction
        Quaternion startRotation = GetBulletStartRotationTowards(fireDirection);

        // Instantiate a new bullet from the firepoint with the startRotation, using the currentBulelt as prefab
        GameObject bullet = Instantiate(currentBullet, firepoint.position, startRotation);

        // Take one of the current ammo after instantiating
        currentAmmoCapacity--;

        // Debug.Log(currentAmmoCapacity);

        // Find the rigidbody on the bullet 
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Check if its not null 
        if (rb != null)
        {
            // Add force towards the fire direction, multiplied with the current speed for the bullet
            rb.AddForce(fireDirection * currentBulletSpeed, ForceMode.Impulse);
        }
        else // Throw error
        {
            throw new System.Exception("Rigidbody on bullet prefab is missing, assign please, then try to fire bullets again");
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
    /// Returns a normalized vector from param to the mouse direction, applying bullet spread around it. 
    /// </summary>
    /// <param name="_fromVector"></param>
    /// <param name="_applyBulletSpread"></param>
    /// <returns></returns>
    private Vector3 GetFireDirectionToMouse(Vector3 _fromVector, bool _applyBulletSpread = true)
    {
        // Get the mouse position 
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from the main camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Call method to get the targetPoint, depending on the max range of our ray
        Vector3 targetPoint = GetTargetPoint(ray);

        Vector3 shootingDirection = Vector3.zero;

        if (_applyBulletSpread)
        {
            Vector3 spreadTargetPoint = GetRandomSpreadVector(targetPoint);

            // Calculate direction for the bullet by subtracting firepoint pos from the target pos
            shootingDirection = spreadTargetPoint - _fromVector;
        }
        else // Do not apply bullet spread
        {
            // So get directiom from the targetpoint itself
            shootingDirection = targetPoint - _fromVector;
        }

        // Return normalized vector for direction
        return shootingDirection.normalized;
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
    /// Returns a new vector with random calculated offsets on the param vector, used for bullet spread <br/>
    /// Gets the point, then gets a random point within x value around it, scaling it with the distance from the firepoint towards the param
    /// </summary>
    /// <param name="_targetPoint"></param>
    /// <returns></returns>
    private Vector3 GetRandomSpreadVector(Vector3 _targetPoint)
    {
        // Var for setting the amount of vecotr axis we want to apply spread to
        int vectorAxis = 3;

        // We make a new float array to store random values, with size of above int
        float[] randomSpreadValues = new float[vectorAxis];

        //Calculate distance from firepoint to the target point
        float distanceToPoint = Vector3.Distance(_targetPoint, firepoint.position);

        // We loop through the vectorAxis' and fill the array with a random calculated spread, scaled to distance to point
        for (int i = 0; i < vectorAxis; i++)
        {
            // Calculate random spread using the value from the ammo clip as range
            float randomSpread = Random.Range(-weaponData.BaseAmmoClip.RandomSpreadRange, weaponData.BaseAmmoClip.RandomSpreadRange);

            // We multiply the spread with the distance to scale it with distance (prevents closer aimed shots having more spread than further aimed shots)
            randomSpread *= distanceToPoint;

            // We store the random calculated spread value in the array 
            randomSpreadValues[i] = randomSpread;
        }

        // We create a new vector that takes the params vector and applies a value from the array to each axis.
        Vector3 spreadVector = new Vector3(_targetPoint.x + randomSpreadValues[0], _targetPoint.y + randomSpreadValues[1], _targetPoint.z + randomSpreadValues[2]);

        // We return the new vector as spreadVector;
        return spreadVector;
    }

    /// <summary>
    /// Method to get a hit point on trajectory ray <br/>
    /// Returns a point as Vector3 representing either the end of our range, or the point where the ray collides with something
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    private Vector3 GetTargetPoint(Ray ray)
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

    public enum WeaponFireMode
    {
        SemiAutomatic, // Fires a single shot each activation
        FullAutomatic, // Fires a single shot with delay while holding activation key
        BurstFire // Fires multiple shots in quick succession each activation
    }
}

