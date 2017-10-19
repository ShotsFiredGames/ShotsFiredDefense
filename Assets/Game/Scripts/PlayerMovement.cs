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

    [Space, Header("Dash Variables")]
    public float dashForce;
    public GameObject dashStreaks;
    
    bool dashing;

    #region Jetpack Variables
    [Space, Header("JetPack Variables")]
    public float fuel = 100;
    public float fuelConsumtionRate;
    public float fuelRegenerationRate;

    bool jetpack;
    float currentFuel;
    bool consumingFuel;
    bool regainingFuel;
    bool canUseJetPack;
    bool jetPackOnCooldown;
#endregion

    float horizontal;
    float vertical;
    
    float horizontal2;

    float xRotationValue;

    Vector3 direction;
    Quaternion rotation;

    bool isJumping;

    Rigidbody rb;

    public static bool inAir;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (jetpack)
        {
            currentFuel = fuel;
            canUseJetPack = true;
        }

        SetAbilities();
    }

    void SetAbilities()
    {
        jetpack = PlayerInput.jetpack;
    }

    private void Update ()
    {
        RecieveInput();

        print("Test");
        if (PlayerInput.dash)
        {
            if (PlayerInput.isDashing && !dashing)
            {
                print("Can Dash");
                dashing = true;
                StartCoroutine(Dash());
            }
        }
    }

    private void LateUpdate()
    {
        Movement();
        Rotate();


        if (!jetpack)
        {
            Jump();
        }
        else
        {
            #region JetPackLogic
                        if (Grounded())
                            jetPackOnCooldown = false;

                        if (canUseJetPack)
                            JetPack();
                        else
                        {
                            if(!jetPackOnCooldown)
                            RegenerateFuel();

                            Jump();
                        }
            #endregion
        }

        if (!Grounded())
        {
            inAir = true;
            Fall();
        }
        else
            inAir = false;
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
        direction = new Vector3((horizontal * speed), 0, (vertical * speed));

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

    IEnumerator Dash()
    {
        Camera.main.fieldOfView = 55;
        rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);
        dashStreaks.SetActive(true);
        yield return new WaitForSeconds(.1f);
        Camera.main.fieldOfView = 60;
        dashStreaks.SetActive(false);
        dashing = false;
    }

    #region JetPack
        void JetPack()
        {
            if (isJumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

                if(!consumingFuel)
                {
                    consumingFuel = true;
                    StartCoroutine(ConsumeFuel());

                    if (currentFuel <= 0)
                    {
                        jetPackOnCooldown = true;
                        canUseJetPack = false;
                    }
                }
            }
            else
            {
                RegenerateFuel();
            }
        }

        void RegenerateFuel()
        {
            if (currentFuel > 0)
                canUseJetPack = true;

            if (currentFuel < fuel && !regainingFuel)
            {
                regainingFuel = true;
                StartCoroutine(RegainFuel());
            }
        }

        IEnumerator ConsumeFuel()
        {
            currentFuel -= 10;
            yield return new WaitForSeconds(fuelConsumtionRate);
            consumingFuel = false;
        }

        IEnumerator RegainFuel()
        {
            currentFuel += 10;
            yield return new WaitForSeconds(fuelRegenerationRate);
            regainingFuel = false;
        }
    #endregion

    void Fall()
    {
        rb.velocity += Physics.gravity * gravity * Time.fixedDeltaTime;
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround, ground);
    }
}
