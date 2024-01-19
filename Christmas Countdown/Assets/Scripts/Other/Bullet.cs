using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Time before destroying the bullet after colliding")]
    [SerializeField] private float afterCollisionDestroyDelay;

    [Tooltip("Total time this object is active in the scene")]
    [SerializeField] private float totalDestroyDelay;

    [Tooltip("Max value of random calculation, used for adding a rotation and force after colliding")]
    [SerializeField] private float maxRandomNumberValue;

    [Tooltip("How much the velocity decreases after colliding (in percentages) ")]
    [Range(0, 100f)]
    [SerializeField] private float velocityDecreaseOnCollision;

    [Tooltip("Layers that the bullet can collide with")]
    [SerializeField] private LayerMask collisionLayers;

    public Vector3 bulletRotation;

    public float bulletSpeed;

    // Reference to the rigidbody for applying forces
    private Rigidbody rb;

    // Bool to track if the bullet already collided
    private bool hasCollided;

    private void Start()
    {
        // Get the rigidbody on this object
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        // Destroy the whole object after the total delay
        Destroy(gameObject, totalDestroyDelay);
    }

    private void Update()
    {
        // Wait untill the bullet has collided
        if (hasCollided)
        {
            // Call to destroy object with the delay 
            Destroy(gameObject, afterCollisionDestroyDelay);
           
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
        if ((collisionLayers & (1 << collision.gameObject.layer)) != 0)
        {
            if (!hasCollided)
            {
                // Make two random variables using our random value to apply forces and rotation
                float randomForceHorizontal = Random.Range(-maxRandomNumberValue, maxRandomNumberValue); // Random var for horizontal force (Ranging from min to plus for left and right)
                float randomForceVertical = Random.Range(0, maxRandomNumberValue); // Random var for vertical force ( !< 0 to not apply force towards the ground

                // Random variables for our torque, random for all axis of the vector.
                float randomTorgueValueX = Random.Range(-maxRandomNumberValue, maxRandomNumberValue);
                float randomTorgueValueY = Random.Range(-maxRandomNumberValue, maxRandomNumberValue);
                float randomTorgueValueZ = Random.Range(-maxRandomNumberValue, maxRandomNumberValue);

                // Reduce velocity by percentage on collision
                rb.velocity *= (100 - velocityDecreaseOnCollision) / 100f; // Gets multiplied by percentage ranging from 0-1 to decrease speed

                // Add force in the horizontal direction of our rb, using the horizontal random force value
                rb.AddForce(Vector3.right * randomForceHorizontal, ForceMode.Impulse);

                // Add force in the vertical direction of our rb (up), using the vertical random force value
                rb.AddForce(Vector3.up * (randomForceVertical *2 ) , ForceMode.Impulse);

                // Apply torque for random rotation
                rb.AddTorque(new Vector3(randomTorgueValueX, randomTorgueValueY, randomTorgueValueZ));

            }

            // Flip bool to not apply forces another time
            hasCollided = true;
        }
    }
}
