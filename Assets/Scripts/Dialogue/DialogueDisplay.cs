using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] GameObject dialogueUI;
    [SerializeField] TextMeshProUGUI speakerComponent;
    [SerializeField] TextMeshProUGUI dialogueComponent;


    public InputActionReference DialogueAction;
    
    PlayerInput playerInput;
    DialogueFiles files;
    public List<DialogueData> scriptData;

    public int index;
    private bool isTalking = false;


    private void Start()
    {
        files = GetComponent<DialogueFiles>();
        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();

        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        DialogueAction.action.performed += TextUpdate;
    }

    private void OnDisable()
    {
        DialogueAction.action.performed -= TextUpdate;
    }
    private void TextUpdate(InputAction.CallbackContext context)
    {
        if (isTalking == true)
        {
            if (dialogueComponent.text == scriptData[index].dialogueLine)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueComponent.text = scriptData[index].dialogueLine;
            }
        }
    }
    public void StartDialogue(string fileName)
    {
        playerInput.SwitchCurrentActionMap("UI");

        if (isTalking == true)
        { return; }

        index = 0;
        dialogueUI.SetActive(true);
        scriptData = files.LoadFiles(fileName);
        isTalking = true;

        speakerComponent.text = string.Empty;
        dialogueComponent.text = string.Empty;

        StartCoroutine(DialogueLines());
    }

    IEnumerator DialogueLines()
    {
        //to be added: display Charater Icon based on ScriptData[index].speakerName;

        speakerComponent.text = scriptData[index].speakerName;

        foreach (char c in scriptData[index].dialogueLine)
        {
            //dialogueComponent.fontSize = scriptData[index].textSize;
            dialogueComponent.text += c;
            if(char.IsPunctuation(c))
            {
                yield return new WaitForSecondsRealtime(scriptData[index].textSpeed * 2);
            }
            yield return new WaitForSecondsRealtime(scriptData[index].textSpeed);
        }
    }

    void NextLine()
    {
        if (index < scriptData.Count - 1)
        {
            index++;
            dialogueComponent.text = string.Empty;
            StartCoroutine(DialogueLines());
        }
        else
        {
            isTalking = false;
            playerInput.SwitchCurrentActionMap("Player");
             dialogueUI.SetActive(false);
            Time.timeScale = 1;
        }
    }
}


