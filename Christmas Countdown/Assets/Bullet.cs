using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float afterCollisionLifetime;
    [SerializeField] private float totalMaxLifetime;
    [SerializeField] private float maxRandomNumberValue;

    private Rigidbody rb;
    private bool hasCollided;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, totalMaxLifetime);
    }

    private void Update()
    {
        if (hasCollided)
        {
            Destroy(gameObject, afterCollisionLifetime);
            rb.useGravity = true;
            rb.velocity *= 0.99f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float randomUp = Random.Range(0, maxRandomNumberValue);
        float randomSides = Random.Range(-maxRandomNumberValue, maxRandomNumberValue);

        if (!hasCollided)
        {
            rb.AddForce(Vector3.right * randomSides, ForceMode.Impulse);
            rb.AddForce(Vector3.up * randomUp, ForceMode.Impulse);
        }

        hasCollided = true;
    }
}
