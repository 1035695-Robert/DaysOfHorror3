using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using UnityEditor;
using UnityEngine;


[Serializable]
public class DialogueData
{
    public string speakerName;
    public string dialogueLine;

    public float textSpeed;
}
[Serializable]
public class DialogueList
{
    public List<DialogueData> data;
}

public class DialogueFiles : MonoBehaviour
{
    public List<DialogueData> LoadFiles(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogue/" + fileName);

        if (jsonFile != null)
        {
            DialogueList dataList = JsonUtility.FromJson<DialogueList>(jsonFile.text);

            return dataList.data;
        }
        else
        {
            Debug.Log("Error " + fileName);
            return new List<DialogueData>();
        }
    }


}
