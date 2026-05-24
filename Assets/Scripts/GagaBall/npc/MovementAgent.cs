using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class MovementAgent : MonoBehaviour
{

    [SerializeField] private Transform targetObject;
    private NavMeshAgent agent;
    public bool hasBall;
    [SerializeField] private bool isFleeing;

    private float fleeDistance = 10f;

    private void OnEnable()
    {
        EventManager.flee += StartFlee;
        EventManager.ballCheck += BallUpdate;
    }

    private void OnDisable()
    {
        EventManager.flee -= StartFlee;
        EventManager.ballCheck -= BallUpdate;
    }



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (!targetObject)
            Debug.LogError("cant find target object");

        if (!hasBall && !isFleeing)
        {
            agent.destination = targetObject.position;
        }
    }
    private void StartFlee()
    {
        StartCoroutine(Avoidance());
    }
    IEnumerator Avoidance()
    {
        isFleeing = true;
        Debug.Log("i will run Away");
        while (isFleeing)
        {
            Vector3 awayDirection = transform.position - targetObject.position;
            Vector3 fleeDesitination = transform.position + (awayDirection.normalized * fleeDistance);

            agent.SetDestination(fleeDesitination);

            yield return null;
        }
        yield break;
    }

    public void HasBall()
    {
        Debug.Log("HAHAHA i have the ball!!");
        EventManager.flee -= StartFlee;
    }
    public void LoseBall()
    {
        EventManager.flee += StartFlee;

        EventManager.ballCheck.Invoke();
    }
    private void BallUpdate()
    {
        hasBall = false;
        isFleeing = false;
    }

}