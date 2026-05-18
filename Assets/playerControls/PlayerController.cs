using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private Vector2 directionInput;

    public float walkSpeed;
    public float sprintSpeed;
    public float rotationSpeed;


    public InputActionReference move;
    public InputActionReference sprintHold;
    public InputActionReference sprintToggle;


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
        sprintHold.action.started += SprintPressed;
        sprintHold.action.canceled += SprintReleased;

        sprintToggle.action.performed += OnTogglePerformed;
    }
    private void OnDisable()
    {
        move.action.Disable();

        sprintHold.action.started -= SprintPressed;
        sprintHold.action.canceled -= SprintReleased;

        sprintToggle.action.performed -= OnTogglePerformed;
    }

    private void FixedUpdate()
    {
        directionInput = move.action.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(directionInput.x, 0, directionInput.y);
        moveDirection.Normalize();
        controller.Move(moveDirection * walkSpeed * Time.fixedDeltaTime);
        if (isToggleActive == true || isSprintHolding == true)
        {
            controller.Move(moveDirection * sprintSpeed * Time.fixedDeltaTime);
        }
        else
        {
            controller.Move(moveDirection * walkSpeed * Time.fixedDeltaTime);
        }
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime );
        }
    }

    //thi
    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        isToggleActive = !isToggleActive;
    }

    #region Sprint: Press and Hold
    private void SprintPressed(InputAction.CallbackContext context)
    {
        isSprintHolding = true;
    }
    private void SprintReleased(InputAction.CallbackContext context)
    {
        isSprintHolding = false;
    }
    #endregion
}
