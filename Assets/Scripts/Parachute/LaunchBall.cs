using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class PlayerForceStats
{
    public GameObject player;
    public float forceValue;
}

public class LaunchBall : MonoBehaviour
{
    private Rigidbody rb;

    QuickTimeEvent quickTimeEvent;
    public float launchForce;
    private bool isPaused = false;

    private float lowestValue;
    private float highestValue;

    public List<PlayerForceStats> playerForce = new List<PlayerForceStats>();
    public List<GameObject> weakestPlayer = new List<GameObject>();

    public BallDirectionForce directions;

    public Vector3 rollBack = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        quickTimeEvent = GameObject.Find("QTE_Manager").GetComponent<QuickTimeEvent>();

        rb = GetComponent<Rigidbody>();

    }

    IEnumerator LaunchCheck()
    {
        if (playerForce == null || playerForce.Count == 0)
        {
            Debug.LogError("no players Found");
        }
        foreach (var P in playerForce)
        {
            StartCoroutine(quickTimeEvent.MovePointer(P.player));
            isPaused = true;
            yield return null;
            while (isPaused)
            {
                yield return null;
            }
        }
        if (!isPaused)
        {
            Debug.Log("Ready to Launch");
            Launch();
        }
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
    public void StoreForceValue(double Value, GameObject player)
    {

        int index = playerForce.FindIndex(playerInfo => playerInfo.player.name == player.name);

        playerForce[index].forceValue = (float)Value;
        isPaused = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        if (collision.transform.name == "ground")
        {
            Vector3 rollDirection = (new Vector3(0, transform.position.y, 0) - transform.position).normalized;
            rb.AddForce(rollDirection * 1f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.transform.name == "ParachuteCenter")
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            StartCoroutine(LaunchCheck());
        }
    }
}



