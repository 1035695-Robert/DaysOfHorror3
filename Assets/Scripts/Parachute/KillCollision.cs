using JetBrains.Annotations;
using UnityEngine;

public class KillCollision : MonoBehaviour
{
    LivingPlayerManager playerManagerList;
    EndOfGameDialogue endOfGame;

    private void Start()
    {
        playerManagerList = FindAnyObjectByType<LivingPlayerManager>();
        endOfGame = GameObject.Find("DialogueManager").GetComponent<EndOfGameDialogue>();

    }
    private void OnCollisionEnter(Collision collision)
    {

        GameObject hitPLayer = collision.gameObject;
        int index = playerManagerList.playerLists.FindIndex(p => p.playerPrefab == hitPLayer);
        bool wasPlayer = playerManagerList.playerLists.Exists(p => p.playerPrefab == hitPLayer);

        if (wasPlayer)
        {
            EventManager.Die.Invoke(hitPLayer);
            if (playerManagerList.playerLists[index].playerName != "Clancy")
            {
                string killedPlayer = playerManagerList.playerLists[index].playerName;
                Debug.Log(killedPlayer);
                endOfGame.EndGameSaveData(killedPlayer, "Parachute");
                playerManagerList.playerLists.Remove(playerManagerList.playerLists[index]);
            }
            else
            {
                GameOverManager gameOver = GameObject.Find("GameOver").GetComponent<GameOverManager>();
                gameOver.GameOverMenu();
            }
        }
    }
    public void NoMoreRounds()
    {
        endOfGame.EndGameSaveData("NoDeath", "Parachute");
    }
    

}
