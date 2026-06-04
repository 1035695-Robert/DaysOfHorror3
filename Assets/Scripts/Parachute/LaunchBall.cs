using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

[Serializable]
public class PlayerForceStats
{
    public GameObject player;
    public float forceValue;
}

public class LaunchBall : MonoBehaviour
{
    private Rigidbody rb;

    public QuickTimeEvent quickTimeEvent;
    public float launchForce;
    public bool isPaused = false;

    private float lowestValue;
    private float highestValue;

    public LivingPlayerManager playerManagerList;
    public List<PlayerForceStats> playerForce = new List<PlayerForceStats>();
    public List<GameObject> weakestPlayer = new List<GameObject>();

    public BallDirectionForce directions;

    public Vector3 rollBack = new Vector3(0, 0.5f, 0);
    public int roundIndex;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        playerManagerList = FindAnyObjectByType<LivingPlayerManager>().instance;
        for (int p = playerManagerList.playerLists.Count - 1; p >= 0; p--)
        {
            Debug.Log(playerManagerList.playerLists.Count);
            playerForce[p].player = playerManagerList.playerLists[p].playerPrefab;
        }

        quickTimeEvent = FindAnyObjectByType<QuickTimeEvent>();


        rb.useGravity = true;
        roundIndex = 0;
    }

    IEnumerator LaunchCheck()
    {
        if (playerForce == null || playerForce.Count == 0)
        {
            Debug.LogError("no players Found");
        }

        EventManager.tossRound.Invoke(roundIndex);
        isPaused = true;
        yield return null;
        while (isPaused)
        {
            yield return null;
        }

        Debug.Log("Ready to Launch");
        Launch();
        roundIndex++;
    }

    public void Launch()
    {
        Debug.Log("YEET");
        weakestPlayer.Clear();
        for (int p = 0; p < playerForce.Count; p++)
        {
            if (p == 0)
            {
                weakestPlayer.Add(playerForce[p].player);
                lowestValue = playerForce[p].forceValue;
                highestValue = playerForce[p].forceValue;
            }
            else
             if (playerForce[p].forceValue <= lowestValue)
            {
                if (playerForce[p].forceValue == lowestValue)
                {
                    weakestPlayer.Add(playerForce[p].player);
                }
                else
                {
                    weakestPlayer.Clear();
                    weakestPlayer.Add(playerForce[p].player);
                }
            }
            if (playerForce[p].forceValue >= highestValue)
            {
                highestValue = playerForce[p].forceValue;
            }
        }

        foreach (GameObject weak in weakestPlayer)
        {
            directions = weak.transform.Find("parachute").GetComponent<BallDirectionForce>();
            if (directions != null)
            {
                directions.ConfirmForceAmount(highestValue);
            }
            else
                Debug.LogError("error");
        }
        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "ground")
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            Vector3 rollDirection = (new Vector3(0, transform.position.y, 0) - transform.position).normalized;
            rb.AddForce(rollDirection * 1f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "ParachuteCenter")
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            StartCoroutine(LaunchCheck());
        }
    }
}



