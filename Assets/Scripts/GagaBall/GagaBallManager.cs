using JetBrains.Annotations;
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

public class GagaBallManager : MonoBehaviour, IInteractable
{
    GameObject player;
    PlayerController playerControls;
    HoldObject holdObject;

    private Rigidbody thrownObject;

    float playeyDefaultSpeed;

    public float dropTimeLength = 3.0f;

    public float stopThreshold = 0.01f;

    public InputActionReference Toss;
    PlayerInput playerInput;

    public GameObject[] players;
    public GameObject[] otherPlayers;
    private MovementAgent movement;

    public bool isThrowActive;
    public bool isBombActive;
    public bool isFinalShowdown = false;
    Collider ballCollision;
    void Start()
    {
        thrownObject = gameObject.GetComponent<Rigidbody>();
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
        StopAllCoroutines();

        transform.GetComponent<Renderer>().material.color = Color.white;

        holdObject = target.GetComponent<HoldObject>();
        //pick ball up 

        GameObject bestTarget = FindRandomPlayer(players, target);

        holdObject.Hold(transform);
        Debug.Log(transform.parent.name);
        if (IsPlayer(target))
            StartCoroutine(DropCountDown());
        else
        {
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
        otherPlayers = Collection.Where(player => player != target).ToArray();
        if (otherPlayers.Length <= 1f && !isFinalShowdown)
        {
            ActivateBombBall();
        }
        int randomIndexValue = Random.Range(0, otherPlayers.Length);
        Debug.Log(otherPlayers[randomIndexValue].name);
        return otherPlayers[randomIndexValue];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "ground" && isThrowActive)
        {
            StartCoroutine(WaitTillSlowedDown());
        }
        foreach (var player in otherPlayers)
        {
            if (collision.gameObject.name == player.name && isThrowActive)
            {
                HitTarget(player);
                if (isBombActive)
                {
                    Explosion(player);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var player in otherPlayers)
        {
            if (other.gameObject.name == player.name && isThrowActive)
            {
                HitTarget(player);
                if (isBombActive)
                {
                    Explosion(player);
                }
            }
        }
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
                isThrowActive = true;
                yield break;
            }
            yield return null;
        }
        holdObject.Drop();
        EventManager.ballCheck.Invoke();
        playerControls.walkSpeed = playeyDefaultSpeed;
        yield return null;
    }

    public IEnumerator WaitTillSlowedDown()
    {
        yield return new WaitForSeconds(0.5f);

        transform.GetComponent<Renderer>().material.color = Color.red;
        EventManager.ballCheck?.Invoke();
        isThrowActive = false;
    }

    void HitTarget(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        health.BeenHit();
    }
    public void PlayerIsOut(GameObject outPlayer)
    {
        players = players.Where(knockOut => knockOut != outPlayer).ToArray();
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
    void Explosion(GameObject player)
    {

        Debug.Log("KABOOM");
        if (player.name == "player")
        {
            GameOverManager gameOver = GameObject.Find("GameOver").GetComponent<GameOverManager>();
            gameOver.GameOverMenu();
        }
        else
        {
            Debug.Log("playerWins");
        }

    }
}
