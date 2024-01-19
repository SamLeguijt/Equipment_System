using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    /* ------------------------------------------  VARIABLES ------------------------------------------- */


    [Tooltip("Reference to the ScriptableObject holding this bullet's data")]
    [SerializeField] private BulletObject bulletData;

    // Reference to the rigidbody for applying forces
    private Rigidbody rb;

    // Bool to track if the bullet already collided
    private bool hasCollided;


    /* ------------------------------------------  PROPERTIES ------------------------------------------- */



    /// <summary>
    /// This behaviours Bullet ScriptableObject to get its values from
    /// </summary>
    public BulletObject BulletData { get { return bulletData; } }


    /* ------------------------------------------  METHODS ------------------------------------------- */



    private void Start()
    {
        // Get the rigidbody on this object
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Destroy the whole object after the total delay
        Destroy(gameObject, bulletData.MaxObjectLifetime);
    }

    private void Update()
    {
        // Wait untill the bullet has collided
        if (hasCollided)
        {
            // Call to destroy object with the delay 
            Destroy(gameObject, bulletData.AfterCollisionDestroyDelay);

            // Let the rigidbody use gravity
            rb.useGravity = true;
        }
    }

    /// <summary>
    /// Method gets called upon collision with the bullet 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // Only do the forces once, on first collision

        // Check if collision happened on one of the collision layers
        if ((bulletData.CollisionLayers & (1 << collision.gameObject.layer)) != 0)
        {
            if (!hasCollided)
            {
                // Make two random variables using our random value to apply forces and rotation
                float randomForceHorizontal = Random.Range(-bulletData.MaxRandomNumberValue, bulletData.MaxRandomNumberValue); // Random var for horizontal force (Ranging from min to plus for left and right)
                float randomForceVertical = Random.Range(0, bulletData.MaxRandomNumberValue); // Random var for vertical force ( !< 0 to not apply force towards the ground

                // Random variables for our torque, random for all axis of the vector.
                float randomTorgueValueX = Random.Range(-bulletData.MaxRandomNumberValue, bulletData.MaxRandomNumberValue);
                float randomTorgueValueY = Random.Range(-bulletData.MaxRandomNumberValue, bulletData.MaxRandomNumberValue);
                float randomTorgueValueZ = Random.Range(-bulletData.MaxRandomNumberValue, bulletData.MaxRandomNumberValue);

                // Reduce velocity by percentage on collision
                rb.velocity *= (100 - bulletData.VelocityDecreasePercentageOnCollision) / 100f; // Gets multiplied by percentage ranging from 0-1 to decrease speed

                // Add force in the horizontal direction of our rb, using the horizontal random force value
                rb.AddForce(Vector3.right * randomForceHorizontal, ForceMode.Impulse);

                // Add force in the vertical direction of our rb (up), using the vertical random force value
                rb.AddForce(Vector3.up * (randomForceVertical * 2), ForceMode.Impulse);

                // Apply torque for random rotation
                rb.AddTorque(new Vector3(randomTorgueValueX, randomTorgueValueY, randomTorgueValueZ));

            }

            // Flip bool to not apply forces another time
            hasCollided = true;
        }
    }
}
