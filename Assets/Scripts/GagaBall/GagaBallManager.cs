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

    public Transform[] targets;

    void Start()
    {
        //player = GameObject.Find("player");
        //playerControls = player.GetComponent<PlayerController>();

        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();

        //playeyDefaultSpeed = playerControls.walkSpeed;
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

        if (IsPlayer(target))
            StartCoroutine(DropCountDown());
        else
        {
            GameObject bestTarget = GetSmallestRotationTarget(targets);
            GagaAgent gagaAgent = target.GetComponent<GagaAgent>();
            gagaAgent.StartCoroutine(gagaAgent.NPCDropCountDown(dropTimeLength, bestTarget));
        }
    }
    GameObject GetSmallestRotationTarget(Transform[] Collection)
    {
        int randomIndex = Random.Range(0, Collection.Length);
        Debug.Log(randomIndex);
        return Collection[randomIndex].gameObject;
        //return Collection
        //    .OrderBy(t => Vector3.Angle(transform.forward, t.position - transform.position))
        //    .FirstOrDefault();
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
        //playerControls.walkSpeed = 0;
        float dropTime = dropTimeLength;
        while (dropTime > 0)
        {
            dropTime -= Time.deltaTime;
            if (Toss.action.WasPerformedThisFrame())
            {
                holdObject.Throw();
                //playerControls.walkSpeed = playeyDefaultSpeed;
                yield break;
            }
            yield return null;
        }
        holdObject.Drop();
        //playerControls.walkSpeed = playeyDefaultSpeed;
        yield return null;
    }

    
   
}
