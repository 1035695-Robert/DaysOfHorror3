using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    public string conversationName;
    DialogueDisplay dialogueDisplay;

    void Start()
    {
        dialogueDisplay = GameObject.Find("DialogueManager").GetComponent<DialogueDisplay>();
    }
    public void OnInteract(GameObject target)
    {
        dialogueDisplay.StartDialogue(conversationName);
    }
}
