using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed;
    public float rotationSpeed;

    [Space, Header("Jump Variables")]
    public float jumpForce;
    public float distToGround = 1.1f;
    public float gravity;
    public LayerMask ground;

    float horizontal;
    float vertical;
    
    float horizontal2;

    float xRotationValue;

    Vector3 direction;
    Quaternion rotation;

    bool isJumping;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update ()
    {
        RecieveInput();
    }

    private void LateUpdate()
    {
        Movement();
        Rotate();
        Jump();

        if (!Grounded())
            Fall();
    }

    void RecieveInput()
    {
        horizontal = PlayerInput.horizontal;
        horizontal2 = PlayerInput.horizontal2;

        vertical = PlayerInput.vertical;

        isJumping = PlayerInput.isJumping;
    }

    void Movement()
    {
        direction = new Vector3(horizontal * speed, 0, vertical * speed);
        print(horizontal + " " +  vertical);

        direction *= Time.deltaTime;
        direction = transform.TransformDirection(direction);
        rb.velocity = new Vector3(direction.x, rb.velocity.y, direction.z);
    }
    
    void Rotate()
    {
        xRotationValue -= -horizontal2 * rotationSpeed * Time.deltaTime;
        rotation = Quaternion.Euler(0, xRotationValue, 0);
        transform.rotation = rotation;
    }

    void Jump()
    {
        if(isJumping && Grounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    void Fall()
    {
        rb.velocity += Physics.gravity * gravity * Time.fixedDeltaTime;
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround, ground);
    }
}
