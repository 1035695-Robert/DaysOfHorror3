
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

    private Animator animator;

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
        targetObject = GameObject.Find("ball").transform;
        agent = GetComponent<NavMeshAgent>();
        defaultSpeed = agent.speed;
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (!targetObject)
            Debug.LogError("cant find target object");

        if (!hasBall && !isFleeing)
        {
            animator.SetBool("isMoving", true);

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
            animator.SetBool("isMoving", true);

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

        //animator.SetBool("isMoving", false);
        yield break;
    }

    public void HasBall()
    {
        Debug.Log("HAHAHA i have the ball!!");
        animator.SetBool("hasBall", true);
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
        animator.SetBool("isMoving", false);
        agent.speed = 0f;
        hasBall = true;
        agent.ResetPath();
    }
    public void StartMoving()
    {
        animator.SetBool("isMoving", true);
        agent.speed = defaultSpeed;
        hasBall = false;
    }
}