
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Rendering;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] private TextMeshPro healthUI;
    private GagaBallManager manager;


    private void Start()
    {
        healthUI = gameObject.GetComponentInChildren<TextMeshPro>();
        health = maxHealth;
        healthUI.text =maxHealth.ToString();
        manager = GameObject.Find("ball").GetComponent<GagaBallManager>();
    }
    private void OnEnable()
    {
        EventManager.finalShowdown += FinalShowdown;
    }
    private void OnDisable()
    {
        EventManager.finalShowdown -= FinalShowdown;
    }
    public void BeenHit()
    {
        health--;
        healthUI.text = health.ToString();
       

        if (health <= 0)
        {
            manager.PlayerIsOut(gameObject);
            gameObject.SetActive(false);
            
            if (gameObject.name == "player")
            {
                GameOverManager gameOver = GameObject.Find("GameOver").GetComponent<GameOverManager>();
                gameOver.GameOverMenu("Lose");
            }
        }
    }

     public void FinalShowdown()
    {
        health = 1;
        healthUI.gameObject.SetActive(false);
    }
}
