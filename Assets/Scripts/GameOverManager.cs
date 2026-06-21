using GJAM5.SoundEffects;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using static Interfaces;


public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverMenuScreen;
    [SerializeField] private TextMeshProUGUI _headerText;

    [SerializeField] private SoundPlayer soundPlayer;

    public GameOverManager instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // This has been the cause of an issue with the
        // GameOverManager being duplicated when reloading scene.
        // If any issues pop up down the line this may need to be
        // added back.

        //DontDestroyOnLoad(this);
    }

    public void Start()
    {
        _gameOverMenuScreen = GameObject.Find("GameOverMenu");
        _headerText = _gameOverMenuScreen.transform.GetComponentInChildren<TextMeshProUGUI>();
        soundPlayer = _gameOverMenuScreen.transform.GetComponent<GameSoundPlayer>();
        _gameOverMenuScreen.SetActive(false);

        Time.timeScale = 1f; // Reset time to normal after reloading again up restarting
    }

    public void GameOverMenu(string victoryCondition)
    {
        Time.timeScale = 0f;
        Debug.Log("GameOver");

        // Added code by Banjo below

        switch (victoryCondition)
        {
            case "Win":
                _headerText.text = "You have proven yourself.";
                soundPlayer.PlaySFXClipAt("HighPitchDrone", transform.position, 0.1f, false);
                break;
            case "Lose":
                _headerText.text = "You have failed.";
                soundPlayer.PlaySFXClipAt("LowPitchDrone", transform.position, 0.1f, false);
                break;
        }

        _gameOverMenuScreen.SetActive(true);
    }
}