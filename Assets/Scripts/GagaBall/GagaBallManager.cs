using JetBrains.Annotations;
using Mono.Cecil;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.WSA;
using static Interfaces;

public class GagaBallManager : MonoBehaviour, IInteractable
{
    GameObject PlayableCharacter;
    PlayerController playerControls;
    HoldObject holdObject;

    private Rigidbody thrownObject;

    float playeyDefaultSpeed;

    public float dropTimeLength = 3.0f;

    public float stopThreshold = 0.01f;

    public InputActionReference Toss;
    PlayerInput playerInput;

    public LivingPlayerManager playerManager;
    public List<PlayerList> gagaPlayerList = new List<PlayerList>();
    public List<PlayerList> targetable = new List<PlayerList>();

    public List<PlayerList> teamMembers;
    string playerString = "Clancy";
    private MovementAgent movement;

    public bool isThrowActive;
    public bool isBombActive;
    public bool isFinalShowdown = false;
    Collider ballCollision;
    void Start()
    {
        playerManager = FindAnyObjectByType<LivingPlayerManager>();
        foreach (var p in playerManager.instance.playerLists)
        {
            gagaPlayerList.Add(p);
        }

        thrownObject = gameObject.GetComponent<Rigidbody>();
        PlayableCharacter = GameObject.Find(playerString);
        playerControls = PlayableCharacter.GetComponent<PlayerController>();

        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();

        playeyDefaultSpeed = playerControls.walkSpeed;
        foreach (var p in gagaPlayerList)
        {
            if (p.playerName == "Clancy")
            {
                teamMembers.Add(p);
            }
            if (p.playerName == "Matilda")
            {
                teamMembers.Add(p);
                return;
            }
            if (p.playerName == "Ryan")
            {
                teamMembers.Add(p);
                return;
            }
        }
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
        StopAllCoroutines();

        transform.GetComponent<Renderer>().material.color = Color.white;

        holdObject = target.GetComponent<HoldObject>();
        //pick ball up 

        GameObject bestTarget = FindRandomPlayer(target);

        holdObject.Hold(transform);

        if (IsPlayer(target))
            StartCoroutine(DropCountDown());

        else
        {
            ThrowAim aimThrow = target.GetComponent<ThrowAim>();

            movement = target.GetComponent<MovementAgent>();
            movement.HasBall();

            Debug.Log(bestTarget);
            aimThrow.StartCoroutine(aimThrow.NPCDropCountDown(dropTimeLength, bestTarget));
        }
        EventManager.flee?.Invoke();
    }
    GameObject FindRandomPlayer(GameObject target)
    {
        if (targetable != null)
        { targetable.Clear(); }

        foreach (var p in gagaPlayerList)
        {

            if (p.playerPrefab != target)
            { targetable.Add(p); }

        }

        if (targetable.Count <= 1f && !isFinalShowdown)
        {
            ActivateBombBall();
        }
        else
        {
            for (int t = 0; t < targetable.Count; t++)
            {
                if (targetable[t].playerPrefab == target)
                {
                    Debug.Log("teamMembers are here");
                    foreach (var tm in teamMembers)
                    {
                        if (tm.playerPrefab != target)
                        {
                            targetable.RemoveAll(T => T.playerName == tm.playerName);
                            break;
                        }
                    }
                }
            }
        }
        int randomIndexValue = Random.Range(0, targetable.Count);
        return targetable[randomIndexValue].playerPrefab;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "ground" && isThrowActive)
        {
            StartCoroutine(WaitTillSlowedDown());
        }
        foreach (var p in targetable)
        {
            if (collision.gameObject == p.playerPrefab && isThrowActive)
            {
                HitTarget(p.playerPrefab);
                if (isBombActive)
                {
                    Explosion(p.playerPrefab);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var p in targetable)
        {
            if (other.gameObject == p.playerPrefab && isThrowActive)
            {
                HitTarget(p.playerPrefab);
                if (isBombActive)
                {
                    Explosion(p.playerPrefab);
                }
            }
        }
    }

    bool IsPlayer(GameObject target)
    {
        if (target.name == playerString)
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
                gameObject.layer = LayerMask.NameToLayer("PickUp");
                playerControls.walkSpeed = playeyDefaultSpeed;
                isThrowActive = true;
                yield break;
            }
            yield return null;
        }
        holdObject.Drop();
        EventManager.ballCheck?.Invoke();
        playerControls.walkSpeed = playeyDefaultSpeed;
        yield return null;
    }

    public IEnumerator WaitTillSlowedDown()
    {
        yield return new WaitForSeconds(0.5f);

        transform.GetComponent<Renderer>().material.color = Color.red;
        gameObject.layer = LayerMask.NameToLayer("Interactables");
        EventManager.ballCheck?.Invoke();
        isThrowActive = false;

    }

    void HitTarget(GameObject target)
    {
        Health health = target.GetComponent<Health>();
        health.BeenHit();
        isThrowActive = false;
        StartCoroutine(WaitTillSlowedDown());



    }
    public void PlayerIsOut(GameObject outPlayer)
    {
        gagaPlayerList.RemoveAll(knockOut => knockOut.playerPrefab == outPlayer);
    }

    void ActivateBombBall()
    {
        isFinalShowdown = true;
        isBombActive = true;
        EventDialogue dialogue = gameObject.GetComponent<EventDialogue>();
        EventManager.finalShowdown.Invoke();
        dialogue.OnInteract();
        //time.timescale = 0f;
    }
    void Explosion(GameObject target)
    {

        Debug.Log("KABOOM");
        if (target.name == playerString)
        {
            GameOverManager gameOver = GameObject.Find("GameOver").GetComponent<GameOverManager>();
            gameOver.instance.GameOverMenu();
        }
        else
        {
            Debug.Log("playerWins");
            playerManager.instance.playerLists.RemoveAll(player => player.playerPrefab == target);
        }

    }
}
