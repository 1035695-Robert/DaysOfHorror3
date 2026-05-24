using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GagaBallManager : MonoBehaviour, IInteractable
{
    GameObject player;
    PlayerController playerControls;
    HoldObject holdObject;

    float playeyDefaultSpeed;

    public float dropTimeLength = 3.0f;


    public InputActionReference Toss;
    PlayerInput playerInput;

    public GameObject[] players;
    private MovementAgent movement;
    void Start()
    {
        player = GameObject.Find("player");
        playerControls = player.GetComponent<PlayerController>();

        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();
       
        playeyDefaultSpeed = playerControls.walkSpeed;
    }
    private void OnEnable()
    {
        Toss.action.Enable();
    }
    public void OnDisable()
    {
        Toss.action.Disable();
    }

    public void OnInteract(GameObject target)
    {
        holdObject = target.GetComponent<HoldObject>();
        //pick ball up 
        
        holdObject.Hold(transform);
        Debug.Log(transform.parent.name);
        if (IsPlayer(target))
            StartCoroutine(DropCountDown());
        else
        {
            GameObject bestTarget = FindRandomPlayer(players, target);
            ThrowAim aimThrow = target.GetComponent<ThrowAim>();
            
            movement = target.GetComponent<MovementAgent>();
            movement.HasBall();

            aimThrow.StartCoroutine(aimThrow.NPCDropCountDown(dropTimeLength, bestTarget));
        }
        EventManager.flee?.Invoke();
    }
    GameObject FindRandomPlayer(GameObject[] Collection, GameObject target)
    {
        //creates temporary array of players exluding itself
        GameObject[] otherPlayer = Collection.Where(player => player != target).ToArray();

        int randomIndexValue = Random.Range(0, otherPlayer.Length);
        Debug.Log(otherPlayer[randomIndexValue].name);
        return otherPlayer[randomIndexValue];

        
       
    }

    bool IsPlayer(GameObject target)
    {
        if (target.name == "player")
            return true;
        else
            return false;
    }
    public IEnumerator DropCountDown()
    {
        playerControls.walkSpeed = 0;
        float dropTime = dropTimeLength;
        while (dropTime > 0)
        {
            dropTime -= Time.deltaTime;
            if (Toss.action.WasPerformedThisFrame())
            {
                holdObject.Throw();
                playerControls.walkSpeed = playeyDefaultSpeed;
                EventManager.ballCheck.Invoke();
                yield break;
            }
            yield return null;
        }
        holdObject.Drop();
        EventManager.ballCheck.Invoke();
        playerControls.walkSpeed = playeyDefaultSpeed;
        yield return null;
    }

    
   
}
