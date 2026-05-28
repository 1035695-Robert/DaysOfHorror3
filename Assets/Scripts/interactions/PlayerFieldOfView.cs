using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Interfaces;

public class PlayerFieldOfView : MonoBehaviour
{
    [Header("dialogue Scripts")]
    public string conversationName;

    [Header("Detection info")]
    public float radius;
    [Range(0, 360)]
    public float angle;
    public bool canSeeObject;
    [Tooltip("this is only used within editor to draw raycast line towards target object")]
    public GameObject targetRef;
    

    [Header("Layer Types")]
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("NPC Interation")]
    public GameObject uiPopUp;
    public TextMeshPro interactText;
    
    

    [Header("Player Inputs")]
    PlayerInput playerInput;
    public InputActionReference interact;

    private void Start()
    {
        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();
        StartCoroutine(FOVRoutine());
    }
    private void OnEnable()
    {
        interact.action.Enable();
       
    }
    private void OnDisable()
    {
        interact.action.Disable();
    }
    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeeObject = true;        
                    StartCoroutine(InteractionUI(target));
                }
                else
                    canSeeObject = false;
            }
            else
                canSeeObject = false;
        }
        else if (canSeeObject)
            canSeeObject = false;
    }
    IEnumerator InteractionUI(Transform target)
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        //string keyName = interact.action.GetBindingDisplayString();
        //interactText.text = "press " + keyName + " to talk.";
        //uiPopUp.SetActive(true);
        Debug.Log("PRESS KEY TO INTERACT");
        while
            (canSeeObject == true)
        {
            if (interact.action.WasPerformedThisFrame())
            {
                Debug.Log("interaction performed");
                IInteractable interactable = target.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract(gameObject);
                    canSeeObject = false;
                }
                
            }
            yield return null;
        }
        //uiPopUp.SetActive(false);
    }
}
