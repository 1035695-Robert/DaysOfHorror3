using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private Vector2 directionInput;

    public float walkSpeed;
    public float sprintSpeed;


    public InputActionReference move;
    public InputActionReference sprintHold;
    public InputActionReference sprintToggle;

    public bool isToggleActive = false;
    public bool isSprintHolding = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {

        sprintHold.action.started += SprintPressed;
        sprintHold.action.canceled += SprintReleased;

        sprintToggle.action.performed += OnTogglePerformed;
    }
    private void OnDisable()
    {
        sprintHold.action.started -= SprintPressed;
        sprintHold.action.canceled -= SprintReleased;

        sprintToggle.action.performed -= OnTogglePerformed;
    }


    private void Update()
    {
        directionInput = move.action.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(directionInput.x, 0, directionInput.y);
        if (isToggleActive == true || isSprintHolding == true)
        {
            rb.MovePosition(rb.position + moveDirection * sprintSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + moveDirection * walkSpeed * Time.fixedDeltaTime);
        }
        //if (moveDirection.sqrMagnitude > 0.01f)
        //{
        //    transform.rotation = Quaternion.LookRotation(moveDirection);
        //}
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
