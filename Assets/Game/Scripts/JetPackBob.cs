using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackBob : MonoBehaviour
{
    public float zClampValue;
    public float xClampValue;
    public float zSpeed;
    public float xSpeed;

    float zRotation;
    float xRotation;
    float horizontal;
    float horizontal2;

    float vertical;
    float vertical2;

    Quaternion gunRotation;

    void Update ()
    {
        horizontal = PlayerInput.horizontal;
        horizontal2 = PlayerInput.horizontal2;

        vertical = PlayerInput.vertical;
        vertical2 = PlayerInput.vertical2;

        zRotation += -horizontal * zSpeed * Time.fixedDeltaTime;
        zRotation += -horizontal2 * zSpeed * Time.fixedDeltaTime;
        zRotation = ClampAngle(zRotation, -zClampValue, zClampValue);

        xRotation += vertical * xSpeed * Time.fixedDeltaTime;
        xRotation += vertical2 * xSpeed * Time.fixedDeltaTime;
        xRotation = ClampAngle(xRotation, -xClampValue, xClampValue);

        gunRotation = Quaternion.Euler(xRotation, 0, zRotation);
        //transform.rotation = gunRotation;

        if (horizontal != 0 || horizontal2 != 0 || vertical != 0 || vertical2 != 0)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, gunRotation, 1);
        else if(horizontal == 0 && horizontal2 == 0 || vertical == 0 && vertical2 == 0)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 1);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
