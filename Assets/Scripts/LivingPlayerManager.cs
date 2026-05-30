using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[SerializeField]
public class PlayerList
{
    public string playerName;
    public GameObject playerModel;
}
public class LivingPlayerManager : MonoBehaviour
{
    public LivingPlayerManager instance;
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
    public List<PlayerList> playerLists = new List<PlayerList>();

}
