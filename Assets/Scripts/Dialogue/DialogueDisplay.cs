using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] GameObject dialogueUI;
    [SerializeField] TextMeshProUGUI speakerComponent;
    [SerializeField] TextMeshProUGUI dialogueComponent;



    
    DialogueFiles files;
    public List<DialogueData> scriptData;

    public int index;
    private bool isTalking = false;


    private void Start()
    {
        files = GetComponent<DialogueFiles>();

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space) && isTalking == true)
        {
            if(dialogueComponent.text == scriptData[index].dialogueLine)
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
                yield return new WaitForSeconds(scriptData[index].textSpeed * 2);
            }
            yield return new WaitForSeconds(scriptData[index].textSpeed);
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
             dialogueUI.SetActive(false);
        }
    }
}


