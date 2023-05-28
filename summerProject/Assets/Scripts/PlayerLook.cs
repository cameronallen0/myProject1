using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    PlayerControls controls;

    private Vector2 mouseDelta;
    public float sensitivity = 100f;
    float xRotation = 0f;
    public Transform playerBody;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => mouseDelta = Vector2.zero;
    }
    private void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    private void Update()
    {
        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
