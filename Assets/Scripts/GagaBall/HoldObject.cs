using GJAM5.SoundEffects;
using System.Collections;
//using Unity.Cinemachine.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HoldObject : MonoBehaviour
{
    [Header("PickUp settings")]
    [SerializeField] Transform holdArea;
    [SerializeField] Transform throwArea;
    [SerializeField] public GameObject heldObject;
    [SerializeField] private Rigidbody heldObjectRB;

    [Header("Throwing")]
    [SerializeField] private float forceAmount = 500f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    public void Hold(Transform targetObject)
    {
        if (targetObject.GetComponent<Rigidbody>())
        {
            heldObject = targetObject.gameObject;
            heldObject.layer = LayerMask.NameToLayer("PickUp");

            heldObjectRB = heldObject.GetComponent<Rigidbody>();
            heldObjectRB.angularVelocity = Vector3.zero;
            heldObjectRB.useGravity = false;
            heldObjectRB.linearDamping = 10;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            //holdArea.transform.localPosition = new Vector3(0, 0, 0f);
            heldObjectRB.transform.parent = holdArea;
            heldObject.transform.localPosition = new Vector3(0, 0, 0f);
            heldObject.transform.localRotation = Quaternion.identity;

            animator.SetBool("hasBall", true);
            animator.SetBool("isMoving", false);
        }
    }

    public void Drop()
    {
        Debug.Log("drop");
        heldObjectRB.useGravity = true;
        heldObjectRB.linearDamping = 1;
        heldObjectRB.constraints = RigidbodyConstraints.None;

        heldObjectRB.transform.parent = null;
        heldObject.layer = LayerMask.NameToLayer("Interactables");
        heldObject = null;
    }

    public void Throw()
    {
        GagaBallSoundPlayer.instance.PlaySFXClipAt("Throw", transform.position, 1, false);

        Debug.Log("Thrown by " + heldObject.transform.parent.name);
        heldObjectRB.useGravity = true;
        heldObjectRB.linearDamping = 1;
        heldObjectRB.constraints = RigidbodyConstraints.None;

        heldObjectRB.transform.parent = throwArea;
        heldObject.transform.localPosition = new Vector3(0, 0, 1f);
        heldObject.transform.localRotation = Quaternion.identity;
        heldObjectRB.transform.parent = null;

        heldObject.layer = LayerMask.NameToLayer("Interactables");
        heldObjectRB.AddForce(transform.forward * forceAmount, ForceMode.Impulse);
        heldObject = null;

        animator.SetBool("hasBall", false);
        animator.SetBool("throwingBall", true);
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

}
//drop cooldown
// out if hold too long