using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private Vector2 directionInput;

    public float walkSpeed;
    public float rotationSpeed;


    public InputActionReference move;



    private CharacterController controller;
    public bool isToggleActive = false;
    public bool isSprintHolding = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        move.action.Enable();
    }
    private void OnDisable()
    {
        move.action.Disable();
    }

    private void FixedUpdate()
    {
        directionInput = move.action.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(directionInput.x, 0, directionInput.y);
        moveDirection.Normalize();
        controller.Move(moveDirection * walkSpeed * Time.fixedDeltaTime);

        controller.Move(moveDirection * walkSpeed * Time.fixedDeltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
