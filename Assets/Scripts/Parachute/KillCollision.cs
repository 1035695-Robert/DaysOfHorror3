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
            // Commented out as we no longer need to keep track of dead polayers
            // to update it after game as this is no longer being presented as a
            // narrative story the player is following

            //if (playerManagerList.playerLists[index].playerName != "Clancy")
            //{
            //    string killedPlayer = playerManagerList.playerLists[index].playerName;
            //    Debug.Log(killedPlayer);
            //    endOfGame.EndGameSaveData(killedPlayer, "Parachute");
            //    playerManagerList.playerLists.Remove(playerManagerList.playerLists[index]);

            //}
            //else
            //{
            //    GameOverManager gameOver = GameObject.Find("GameOver").GetComponent<GameOverManager>();
            //    gameOver.GameOverMenu();
            //}

            // Added code
            GameOverManager gameOver = GameObject.Find("GameOver").GetComponent<GameOverManager>();
            gameOver.GameOverMenu("Lose");
        }

    }
    public void NoMoreRounds()
    {
        //endOfGame.EndGameSaveData("NoDeath", "Parachute");

        // Added code
        GameOverManager gameOver = GameObject.Find("GameOver").GetComponent<GameOverManager>();
        gameOver.GameOverMenu("Win");
    }
    

}
