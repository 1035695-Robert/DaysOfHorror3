using UnityEngine;
using static Interfaces;

public class LobbyNPCDialogue : MonoBehaviour, IInteractable
{

    public GameObject dialogueManager;
    DialogueDisplay dialogueDisplay;
    EndOfGameDialogue deathGame;
    string whichGame;
    string whoDied;
    string speaker;
    void Start()
    {
        dialogueManager = GameObject.Find("DialogueManager");
        dialogueDisplay = dialogueManager.GetComponent<DialogueDisplay>();
        deathGame = dialogueManager.GetComponent<EndOfGameDialogue>();

        whichGame = deathGame.deathGame;
        whoDied = deathGame.killedPlayer;
        speaker = transform.name;
    }
    public void OnInteract(GameObject target)
    {
       
        string conversationFile =
             "DeathLobby/"
            + whichGame + "/"
            + whoDied + "/"
            + speaker;

        dialogueDisplay.StartDialogue(conversationFile);
    }
}
