using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Bullet Object", menuName = "Equipment/Ammunition/Bullet")]
public class BulletObject : ScriptableObject
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    [Tooltip("Time before destroying the bullet after colliding")]
    [SerializeField] private float afterCollisionDestroyDelay;

    [Tooltip("Total time this object is active in the scene")]
    [SerializeField] private float maxObjectLifetime;

    [Tooltip("Max value of random calculation, used for adding a rotation and force after colliding")]
    [SerializeField] private float maxRandomNumberValue;

    [Tooltip("How much the velocity decreases after colliding (in percentages)")]
    [Range(0, 100f)]
    [SerializeField] private float velocityDecreasePercentageOnCollision;

    [Tooltip("Layers that the bullet can collide with")]
    [SerializeField] private LayerMask collisionLayers;

    [Tooltip("Rotation of this bullet when firing")]
    [SerializeField] private Vector3 bulletFireRotation;

    [Tooltip("Speed of the bullet when firing")]
    [SerializeField] private float bulletFireSpeed;



    /* ------------------------------------------  PROPERTIES ------------------------------------------- */

    /// <summary>
    /// Delay in seconds to destroy bullet after colliding, read-only
    /// </summary>
    public float AfterCollisionDestroyDelay {  get { return afterCollisionDestroyDelay; } }

    /// <summary>
    /// Maximum lifetime this object is active (seconds), read-only
    /// </summary>
    public float MaxObjectLifetime { get { return maxObjectLifetime; } }

    /// <summary>
    /// Value used for calculating random values, maximum it can be (read-only)
    /// </summary>
    public float MaxRandomNumberValue { get {  return maxRandomNumberValue; } }

    /// <summary>
    /// How much velocity the bullet loses on collision, in percentages (read-only)
    /// </summary>
    public float VelocityDecreasePercentageOnCollision { get { return velocityDecreasePercentageOnCollision;} }

    /// <summary>
    /// Layermask defining the layers the bullet will collide with, read-only
    /// </summary>
    public LayerMask CollisionLayers { get { return collisionLayers; } }

    /// <summary>
    /// The rotation of the bullet when firing, read-only
    /// </summary>
    public Vector3 BulletStartRotation { get { return bulletFireRotation; } }

    /// <summary>
    /// Speed of the bullet when firing, read-only
    /// </summary>
    public  float BulletFireSpeed { get { return bulletFireSpeed; } }


    /* ------------------------------------------  METHODS ------------------------------------------- */


}
