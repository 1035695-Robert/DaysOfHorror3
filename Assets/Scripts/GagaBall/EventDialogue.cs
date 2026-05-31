using UnityEngine;

public class EventDialogue : MonoBehaviour
{
    public string conversationName;
    DialogueDisplay dialogueDisplay;
    void Start()
    {
        dialogueDisplay = GameObject.Find("DialogueManager").GetComponent<DialogueDisplay>();
    }
    public void OnInteract()
    {
        Time.timeScale = 0f;
        dialogueDisplay.StartDialogue(conversationName);
    }
}
