using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField ]private float movementSpeed;

    [SerializeField] private Transform playerOrientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection; 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMoveInput();
        Move(); 
    }

    private void Move()
    {
        moveDirection = ((playerOrientation.forward * verticalInput) + (playerOrientation.right * horizontalInput)).normalized;
        rb.AddForce(moveDirection * movementSpeed, ForceMode.Force);
    }

    private void CheckMoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
