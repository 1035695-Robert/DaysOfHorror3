using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EndOfGameDialogue : MonoBehaviour
{
    public string killedPlayer;
    public string deathGame;
    
    DialogueDisplay display;
    
    private void Start()
    {
        display = FindAnyObjectByType<DialogueDisplay>();

    }
    public void EndGameSaveData(string deadPlayer, string game)
    {
       
        killedPlayer = deadPlayer;
        deathGame = game;
        
        StartCoroutine(AfterGameDialogue());
    }
    IEnumerator AfterGameDialogue()
    {
        yield return new WaitForSeconds(3f);
        string filePath = "AfterGame/" + deathGame + "/" + killedPlayer;
        display.StartDialogue(filePath);
       
        while (Time.timeScale == 0)
        {
            yield return null;
        }
        LoadLobby();
    }
    public async void LoadLobby()
    {
       SceneLoaderManager loader = new SceneLoaderManager();
        await loader.OnAsyncLoadScene("Lobby");
    }

}
