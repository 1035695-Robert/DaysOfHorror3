using Unity.Cinemachine;
using Unity.Hierarchy;
using UnityEngine;
public class CharacterLoader : MonoBehaviour
{
    public LivingPlayerManager playerManager;
    public string deathSceneFile;
    public GameObject[] SpawnLocations;
    CharacterController controller;
    private float radius = 5;

    void Awake()
    {
        playerManager = FindAnyObjectByType<LivingPlayerManager>();
        if (playerManager != null)
        {

            for (int p = 0; p < playerManager.playerLists.Count; p++)
            {

                float theta = (p * 2 * Mathf.PI / playerManager.playerLists.Count) +Mathf.PI;

                float x = Mathf.Cos(theta) * radius;
                float z = Mathf.Sin(theta) * radius;

                Vector3 spawnPositions =  new Vector3(z, 1, x);

                if (playerManager.playerLists[p].playerName == "Clancy")
                {
                    GameObject player = GameObject.Find("Clancy");
                    controller = player.GetComponent<CharacterController>();
                    controller.enabled = false;
                    player.transform.position = spawnPositions;
                    controller.enabled = true;
                    playerManager.playerLists[p].playerPrefab = player;
                    player.transform.LookAt(new Vector3(0, 1, 0));
                }
                else
                {
                    playerManager.playerLists[p].playerPrefab = Resources.Load<GameObject>(deathSceneFile + "/" + playerManager.playerLists[p].playerName);

                    if (playerManager.playerLists[p].playerPrefab != null)
                    {
                        GameObject player = Instantiate(playerManager.playerLists[p].playerPrefab, spawnPositions, Quaternion.identity);
                        player.name = playerManager.playerLists[p].playerName;
                        player.transform.LookAt(new Vector3(0, 1, 0));
                        player.name.Replace("(Clone)", "").Trim();
                        playerManager.playerLists[p].playerPrefab = player;
                    }
                }
                Vector3 center = new Vector3(0, 1, 0);
               
            }
        }
        else
            Debug.LogError("missing living players list. Are all the players Dead?");
    }
     

}
