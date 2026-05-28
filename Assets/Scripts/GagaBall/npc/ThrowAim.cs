using System.Collections;
using System.Linq;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static Interfaces;

public class ThrowAim : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    public float maxThrowingDistance;

    [Range(0f, 180f)]
    public float fieldOfView;

    public GameObject ball;

    public IInteractable interactable;

    public float throwRange;
    public LayerMask obstructionMask;
    public LayerMask taskMask;
    public GagaBallManager manager;

    HoldObject holdObject;

    MovementAgent movement;

    [Range(0f, 1f)]
    public float viewThreshold;

    public float throwDelay = 1;

   

    private void Start()
    {
        interactable = ball.GetComponent<IInteractable>();
        holdObject = GetComponent<HoldObject>();
        movement = GetComponent<MovementAgent>();
        manager = ball.GetComponent<GagaBallManager>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((taskMask.value & (1 << collision.gameObject.layer)) != 0 && !manager.isThrowActive)
        {
            interactable.OnInteract(gameObject);
        }
    }
   


    public IEnumerator NPCDropCountDown(float timeLength, GameObject target)
    {
        float dropTime = timeLength;
        movement.StopMoving();

        while (dropTime > 0)
        {
            if (target != null)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;

                Quaternion lookRotation = Quaternion.LookRotation(direction);
                lookRotation.x = 0;
                lookRotation.z = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

                if (dropTime < throwDelay && IsLookingAtTarget(target))
                {
                    holdObject.Throw();
                    manager.isThrowActive = true;
                    movement.LoseBall();
                    movement.StartMoving();
                    yield break;
                }
            }
            dropTime -= Time.deltaTime;
            yield return null;
        }

        holdObject.Drop();
        movement.LoseBall();
        movement.StartMoving();
        yield return null;
    }

    bool IsLookingAtTarget(GameObject target)
    {
        Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

        float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

        if (angleToTarget < fieldOfView / 2f && Vector3.Distance(transform.position, target.transform.position) < maxThrowingDistance)
        {
            if (!Physics.Linecast(transform.position, target.transform.position, obstructionMask))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

}

