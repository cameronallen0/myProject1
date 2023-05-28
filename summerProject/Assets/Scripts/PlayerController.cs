using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    CharacterController charController;
    public GameObject playerCamera;

    //move
    private Vector2 moveInput;
    public float moveSpeed = 35f;
    private bool jumpPressed;

    //ground check
    private bool isGrounded;
    public Transform groundCheck;
    private float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpPressed = ctx.ReadValueAsButton();
        controls.Player.Move.performed += ctx => jumpPressed = false;
    }
    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        movement = playerCamera.transform.forward * movement.z + playerCamera.transform.right * movement.x;
        //transform.Translate(movement * moveSpeed * Time.deltaTime);
        charController.Move(movement * moveSpeed * Time.deltaTime);

        if(jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }
        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
    }
}
