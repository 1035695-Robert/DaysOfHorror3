using UnityEngine;
public class CharacterLoader : MonoBehaviour
{
    public LivingPlayerManager PlayerManager;
    public string deathSceneFile;
    public GameObject[] SpawnLocations;


    void Awake()
    {
        PlayerManager = FindAnyObjectByType<LivingPlayerManager>();
        if (PlayerManager != null)
        {
            int index = 0;
            foreach (var p in PlayerManager.instance.playerLists)
            {
                if (p.playerName == "Clancy")
                {
                    p.playerPrefab = GameObject.Find("Clancy");

                    if (p.playerPrefab == null)
                    {
                        Debug.LogError("where is Clancy?");
                    }
                }
                else
                {
                    p.playerPrefab = Resources.Load<GameObject>(deathSceneFile + "/" + p.playerName);

                    if (p.playerPrefab != null)
                    {
                       p.playerPrefab = Instantiate(p.playerPrefab, SpawnLocations[index].transform.position, Quaternion.identity);
                        index++;
                    }
                }
            }

        }
        else
            Debug.LogError("wtf");
    }
    void OnSceneLoaded()
    {

    }




}
