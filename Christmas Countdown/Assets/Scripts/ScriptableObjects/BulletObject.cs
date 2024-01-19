using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Bullet Object", menuName = "Bullet")]
public class BulletObject : ScriptableObject
{
    [Tooltip("Time before destroying the bullet after colliding")]
    [SerializeField] private float afterCollisionDestroyDelay;

    [Tooltip("Total time this object is active in the scene")]
    [SerializeField] private float totalDestroyDelay;

    [Tooltip("Max value of random calculation, used for adding a rotation and force after colliding")]
    [SerializeField] private float maxRandomNumberValue;

    [Tooltip("How much the velocity decreases after colliding (in percentages)")]
    [Range(0, 100f)]
    [SerializeField] private float velocityDecreasePercentageOnCollision;

    [Tooltip("Layers that the bullet can collide with")]
    [SerializeField] private LayerMask collisionLayers;

    [SerializeField] private Vector3 bulletRotation;

    [SerializeField] private float bulletSpeed;


    public float AfterCollisionDestroyDelay {  get { return afterCollisionDestroyDelay; } }
    public float TotalDestroyDelay { get { return totalDestroyDelay; } }
    public float MaxRandomNumberValue { get {  return maxRandomNumberValue; } }

    public float VelocityDecreasePercentageOnCollision { get { return velocityDecreasePercentageOnCollision;} }

    public LayerMask CollisionLayers { get { return collisionLayers; } }

    public Vector3 BulletRotation { get { return bulletRotation; } }
    public  float BulletSpeed { get { return bulletSpeed; } }

}
