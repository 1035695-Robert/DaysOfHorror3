using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability : MonoBehaviour
{

    [Header("\tAbility \n Action Inputs")]
    public InputActionReference abilityInput;
    PlayerInput playerInput;
    protected virtual void Start()
    {
        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();
      
    }
    private void OnEnable()
    {
        abilityInput.action.performed += Input;
    }
    private void OnDisable()
    {
        abilityInput.action.performed -= Input;
    }

    public abstract void Input(InputAction.CallbackContext context);
    
}

