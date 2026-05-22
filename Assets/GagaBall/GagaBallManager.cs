using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GagaBallManager : MonoBehaviour, IInteractable
{
    GameObject player;
    PlayerController playerControls;
    HoldObject holdObject;
   
    float playeyDefaultSpeed;

    public float dropTimeLenght = 3.0f;
   

    public InputActionReference Toss;
    PlayerInput playerInput;
    
    void Start()
    {
        player = GameObject.Find("player");
        playerControls = player.GetComponent<PlayerController>();
        
        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();
        
        playeyDefaultSpeed = playerControls.walkSpeed;

        holdObject = player.GetComponent<HoldObject>();
    }
    private void OnEnable()
    {
        Toss.action.Enable();
    }
    public void OnDisable()
    {
        Toss.action.Disable();
    }

    public void OnInteract()
    {
        //pick ball up 
        holdObject.Hold(transform);
        StartCoroutine(DropCountDown());
    }

    public IEnumerator DropCountDown()
    {
        playerControls.walkSpeed = 0;
        float dropTime = dropTimeLenght;
        while (dropTime > 0)
        {
            dropTime -= Time.deltaTime;
            if(Toss.action.WasPerformedThisFrame())
            {
                holdObject.Throw();
                playerControls.walkSpeed = playeyDefaultSpeed;
                yield break;
            }
            yield return null;
        }
        holdObject.Drop();
        playerControls.walkSpeed = playeyDefaultSpeed;
        yield return null;
    }


}
