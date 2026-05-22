using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    public string conversationName;
    DialogueDisplay dialogueDisplay;

    void Start()
    {
        dialogueDisplay = GameObject.Find("DialogueManager").GetComponent<DialogueDisplay>();
    }
    public void OnInteract()
    {
        dialogueDisplay.StartDialogue(conversationName);
    }
}
