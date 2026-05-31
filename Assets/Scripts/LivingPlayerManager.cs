using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerList
{
    public string playerName;
    public GameObject playerPrefab;
}
public class LivingPlayerManager : MonoBehaviour
{
    public LivingPlayerManager instance;
    public List<PlayerList> playerLists = new List<PlayerList>();
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
   

}
