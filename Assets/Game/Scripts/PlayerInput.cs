﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region InControl

    Controls controls;
    string saveData;

    void OnEnable()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    void OnDisable()
    {
        controls.Destroy();
    }

    void SaveBindings()
    {
        saveData = controls.Save();
        PlayerPrefs.SetString("Bindings", saveData);
    }

    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveData = PlayerPrefs.GetString("Bindings");
            controls.Load(saveData);
        }
    }
    #endregion

    [Header("Abilities")]
    public bool _jetpack;
    public bool _dash;

    public static float horizontal;
    public static float horizontal2;

    public static float vertical;
    public static float vertical2;

    public static bool isJumping;
    public static bool isDashing;
    public static bool isMoving;
    public static bool isDead;

    public static bool isFiring;
    public static bool isAiming;

    public static bool jetpack;
    public static bool dash;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetAbilities();
    }

    void SetAbilities()
    {
        jetpack = _jetpack;
        dash = _dash;
    }

    private void Update()
    {
        ApplyInput();
    }

    void ApplyInput()
    {
        horizontal = controls.Move.Value.x;
        horizontal2 = controls.Look.Value.x;

        vertical = controls.Move.Value.y;
        vertical2 = controls.Look.Value.y;

        isMoving = controls.Move;
        isJumping = controls.Jump;
        isDashing = controls.Sprint.WasPressed;

        isFiring = controls.Fire;
        isAiming = controls.Aim;
    }

    public static void IsDead(bool state)
    {
        isDead = state;
    }

}
