using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : EquipmentActivation
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firepoint;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = Vector3.zero;
    }

    public override void Activate()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.Euler(90f,0,0));

        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from the main camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Get the point at the maximum shooting range or where the ray hits
        Vector3 targetPoint = GetTargetPoint(ray);

        // Calculate the shooting direction
        Vector3 shootingDirection = targetPoint - firepoint.position;

        // Normalize the direction to ensure consistent shooting force
        shootingDirection.Normalize();

        // Apply force to the bullet in the calculated direction
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * 50, ForceMode.Impulse);
    }


    Vector3 GetTargetPoint(Ray ray)
    {
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, 50))
        {
            // If the ray hits an object within the shooting range, return the hit point
            return hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, return a point at the maximum shooting range
            return ray.GetPoint(50);
        }
    }
}
