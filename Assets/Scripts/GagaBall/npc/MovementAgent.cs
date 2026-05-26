
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class MovementAgent : MonoBehaviour
{

    [SerializeField] private Transform targetObject;
    public NavMeshAgent agent;
    public bool hasBall;
    [SerializeField] private bool isFleeing;

    private float defaultSpeed;
    private float fleeRadius = 10f;

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
        defaultSpeed = agent.speed;
    }

    private void FixedUpdate()
    {
        if (!targetObject)
            Debug.LogError("cant find target object");

        if (!hasBall && !isFleeing)
        {
            agent.SetDestination(targetObject.position);
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
            float distance = Vector3.Distance(transform.position, targetObject.position);

            if (distance < fleeRadius)
            {
                Vector3 directionAway = transform.position - targetObject.position;
                Vector3 runDireciton = directionAway.normalized * fleeRadius;

                //NavMeshHit hit;
                //if (NavMesh.SamplePosition(runDireciton, out hit, fleeRadius, NavMesh.AllAreas))
                agent.SetDestination(runDireciton);
            }

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
    }
    private void BallUpdate()
    {
        hasBall = false;
        isFleeing = false;
    }

    public void StopMoving()
    {
        agent.speed = 0f;
        hasBall = true;
        agent.ResetPath();
    }
    public void StartMoving()
    {
        agent.speed = defaultSpeed;
        hasBall = false;
    }
}