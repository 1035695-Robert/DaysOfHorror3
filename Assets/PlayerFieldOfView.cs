using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public GameObject dialogueManager;
    DialogueDisplay dialogueDisplay;

    [Header("Player Inputs")]
    PlayerInput playerInput;
    public InputActionReference interact;

    private void Start()
    {
        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();
        dialogueDisplay = dialogueManager.GetComponent<DialogueDisplay>();

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
                    StartCoroutine(InteractionUI());
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
    IEnumerator InteractionUI()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        string keyName = interact.action.GetBindingDisplayString();
        interactText.text = "press " + keyName + " to talk.";
        uiPopUp.SetActive(true);
       
        while
            (canSeeObject == true)
        {
            if (interact.action.IsPressed())
            {
                playerInput.SwitchCurrentActionMap("UI");
                dialogueDisplay.StartDialogue(conversationName);
            }
            yield return null;

        }
        uiPopUp.SetActive(false);
    }
}
