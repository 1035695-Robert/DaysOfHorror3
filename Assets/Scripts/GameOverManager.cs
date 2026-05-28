using UnityEditor.SearchService;
using UnityEngine;
using static Interfaces;


public class GameOverManager : MonoBehaviour
{
    public GameOverManager instance;
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

    public void GameOverMenu()
    {
        Debug.Log("GameOver");
    }

}
