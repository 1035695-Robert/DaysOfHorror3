using UnityEngine;

public class EventDialogue : MonoBehaviour
{
    public static EventDialogue instance;
    public string conversationName;
    DialogueDisplay dialogueDisplay;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        dialogueDisplay = GetComponent<DialogueDisplay>();
    }
    public void OnInteract(string conversationName)
    {
        Time.timeScale = 0f;
        dialogueDisplay.StartDialogue(conversationName);
    }
}
