using System.Collections;
using Unity.Cinemachine.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HoldObject : MonoBehaviour
{
    [Header("PickUp settings")]
    [SerializeField] Transform holdArea;
    [SerializeField] public GameObject heldObject;
    [SerializeField] private Rigidbody heldObjectRB;

    [Header("Throwing")]
    [SerializeField] private float forceAmount = 500f;

    public void Hold(Transform targetObject)
    {
        if (targetObject.GetComponent<Rigidbody>())
        {
            heldObject = targetObject.gameObject;
            heldObject.layer = LayerMask.NameToLayer("PickUp");

            heldObjectRB = heldObject.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.linearDamping = 10;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRB.transform.parent = holdArea;
            heldObject.transform.localPosition = new Vector3(0, 0, 1f);
            heldObject.transform.localRotation = Quaternion.identity;
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
        Debug.Log("Thrown by " + heldObject.transform.parent.name);
        heldObjectRB.useGravity = true;
        heldObjectRB.linearDamping = 1;
        heldObjectRB.constraints = RigidbodyConstraints.None;
        heldObjectRB.transform.parent = null;

        heldObject.layer = LayerMask.NameToLayer("Interactables");
        heldObjectRB.AddForce(transform.forward * forceAmount, ForceMode.Impulse);
        heldObject = null;
    }

}
//drop cooldown
// out if hold too long