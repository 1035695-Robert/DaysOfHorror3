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
    public static DialogueDisplay instance;

    [SerializeField] GameObject dialogueUI;
    [SerializeField] TextMeshProUGUI speakerComponent;
    [SerializeField] TextMeshProUGUI dialogueComponent;


    public InputActionReference DialogueAction;

    PlayerInput playerInput;
    DialogueFiles files;
    public List<DialogueData> scriptData;

    public int index;
    private bool isTalking = false;

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
    private void Start()
    {
        files = GetComponent<DialogueFiles>();
        playerInput = GameObject.Find("Input Manager").GetComponent<PlayerInput>();
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
            if (dialogueComponent.maxVisibleCharacters >= dialogueComponent.textInfo.characterCount)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueComponent.maxVisibleCharacters = dialogueComponent.textInfo.characterCount;
            }
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
    public void StartDialogue(string fileName)
    {
        playerInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0f;
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

        dialogueComponent.text = scriptData[index].dialogueLine;
        dialogueComponent.maxVisibleCharacters = 0;


        dialogueComponent.ForceMeshUpdate();

        int totalCharacters = dialogueComponent.textInfo.characterCount;

        while (dialogueComponent.maxVisibleCharacters < totalCharacters)
        {
            dialogueComponent.maxVisibleCharacters++;
            int lastCharIndex = dialogueComponent.maxVisibleCharacters - 1;
            char lastChar = dialogueComponent.textInfo.characterInfo[lastCharIndex].character;
            if (IsPunctuation(lastChar))
            {
                yield return new WaitForSecondsRealtime(scriptData[index].textSpeed * 2);
            }

            yield return new WaitForSecondsRealtime(scriptData[index].textSpeed);
        }
    }

    private bool IsPunctuation(char ch)
    {
        return ch == '.' || ch == ',' ||ch == '!' || ch == '?' || ch == ';' || ch == ':';
    }
}


