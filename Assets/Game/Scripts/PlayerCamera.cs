using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{ 
    public float lookSpeed;
    public float clampValue;
    public float aimSpeed;
    public float baseFieldOfView;
    public float aimFieldOfView;

    public GameObject cameraObject;
    public Camera myCamera;
    Vector3 basePosition;
    Vector3 baseRotation;
    Vector3 aimPosition;
    Vector3 aimRotation;

    float xRotationValue;
    float yRotationValue;
    Quaternion cameraYRotation;
    bool isAiming;
    Coroutine currentAimCo;

    float vertical2;

    private void Update()
    {
        vertical2 = PlayerInput.vertical2;
        isAiming = PlayerInput.isAiming;
        Look();
    }

    public void Look()
    {
        if (!isAiming)
            yRotationValue += -vertical2 * lookSpeed * Time.fixedDeltaTime;
        else
            yRotationValue += -vertical2 * (lookSpeed * .25f) * Time.fixedDeltaTime;

        yRotationValue = ClampAngle(yRotationValue, -clampValue, clampValue);
        cameraYRotation = Quaternion.Euler(yRotationValue, 0, 0);
        cameraObject.transform.rotation = cameraYRotation;
        cameraObject.transform.localRotation = Quaternion.Slerp(myCamera.transform.localRotation, cameraYRotation, 1);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }

    public void Aim()
    {
        if (!isAiming)
            isAiming = true;
        
        SetView(aimPosition, aimRotation, aimFieldOfView);
    }

    public void StopAim()
    {
        if (isAiming)
            isAiming = false;
        
        SetView(basePosition, baseRotation, baseFieldOfView);
    }

    public void SetFieldOfView(int view)
    {
        myCamera.fieldOfView = view;
    }

    void SetView(Vector3 newPos, Vector3 newRot, float newFOV)
    {
        currentAimCo = StartCoroutine(MoveToPos(newPos, newRot, newFOV));
    }

    bool moving; //Variable that should only be used in the MoveToPos coroutine

    IEnumerator MoveToPos(Vector3 newPos, Vector3 newRot, float newFOV)
    {
        if (!moving)
        {
            moving = true;
            yield return new WaitUntil(() => InPosition(newPos, newRot, newFOV) == true);
            moveSpeed = 0;
            atPos = false;
            atRot = false;
            atFOV = false;
            moving = false;
        }
    }

    void StopMyCoroutine(Coroutine aCoroutine)
    {
        if (aCoroutine != null)
        {
            StopCoroutine(aCoroutine);
            moveSpeed = 0;
            atPos = false;
            atRot = false;
            atFOV = false;
            moving = false;
        }
    }

    bool atPos, atRot, atFOV; //Variables that should really only be used in the InPosition function
    float moveSpeed;

    bool InPosition(Vector3 newPos, Vector3 newRot, float newFOV)
    {
            moveSpeed += aimSpeed * Time.fixedDeltaTime;
            if (Vector3.Distance(transform.localPosition, newPos) > 0.01f)
                transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, moveSpeed);
            else
            {
                transform.localPosition = newPos;
                atPos = true;
            }

            if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(newRot)) > 1)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(newRot), moveSpeed);
            else
            {
                transform.localRotation = Quaternion.Euler(newRot);
                atRot = true;
            }

            if (myCamera.fieldOfView != newFOV)
                myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, newFOV, moveSpeed);
            else
            {
                myCamera.fieldOfView = newFOV;
                atFOV = true;
            }

            if (atPos && atRot && atFOV)
                return true;
            else
                return false;
        }
}
