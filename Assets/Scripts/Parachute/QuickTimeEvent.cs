using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;
using Mono.Cecil;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting.Antlr3.Runtime;

public class QuickTimeEvent : MonoBehaviour
{
    public GameObject forceGameUI;

    public RectTransform pointer;
    public RectTransform center;
    public RectTransform pointA;
    public RectTransform pointB;
    public Image bar;

    public int width = 300;

    public Gradient gradient;
    [Range(0f, 500f)]
    public float speed;


    [Range(0, 1)]
    public float forcePercentage;
    public InputActionReference lift;

    public PlayerInput inputManager;

    
    private LaunchBall launchBall;
    private void OnEnable()
    {
        lift.action.Enable();
    }
    private void OnDisable()
    {
        lift.action.Disable();
    }

    public void Start()
    {
        launchBall = GameObject.Find("Ball").GetComponent<LaunchBall>();
        inputManager.SwitchCurrentActionMap("UI");

    }
   IEnumerator GetUIComponents(GameObject player)
    {
        GameObject canvas = player.transform.Find("Canvas").gameObject;

        forceGameUI = canvas.transform.Find("Bar").gameObject;
        pointA = forceGameUI.transform.Find("A").GetComponent<RectTransform>();
        pointB = forceGameUI.transform.Find("B").GetComponent<RectTransform>();
        center = forceGameUI.transform.Find("center").GetComponent<RectTransform>();
        pointer = forceGameUI.transform.Find("Pointer").GetComponent<RectTransform>();
        yield return null;
    }

    public IEnumerator MovePointer(GameObject player)
    {
        Debug.Log("good Day");
        yield return StartCoroutine(GetUIComponents(player));
        GradientUI(player);
        forceGameUI.SetActive(true);

        Vector2 startPosition = pointA.position;
        Vector2 endPosition = pointB.position;
        float distance = Vector2.Distance(startPosition, endPosition);

        pointer.position = startPosition;

        while ((Vector2)pointer.position != endPosition)
        {
            pointer.position = Vector2.MoveTowards(pointer.position, endPosition, speed * Time.unscaledDeltaTime);

            float targetDistance = Vector2.Distance(pointer.position, center.position);
            float range = Mathf.Clamp01(targetDistance / (width / 2));
            forcePercentage = 1.0f - range;

            if (lift.action.WasPerformedThisFrame())
            {
                double roundedValue = Math.Round(forcePercentage, 2);
                launchBall.StoreForceValue(roundedValue, player);

                forceGameUI.SetActive(false); 
                yield break;        
            }

            yield return null;
        }
        forceGameUI.SetActive(false);
        launchBall.StoreForceValue(0, player);
        Debug.Log("missed");
    }

    void GradientUI(GameObject Player)
    {
        gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.red, 0.0f),
                new GradientColorKey(Color.yellow, 0.25f),
                new GradientColorKey(Color.green, 0.5f),
                new GradientColorKey(Color.yellow, 0.75f),
                new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f)
                });

        Texture2D texture = new Texture2D(width, 1);
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int x = 0; x < width; x++)
        {
            float t = (float)x / (width - 1);
            Color color = gradient.Evaluate(t);
            texture.SetPixel(x, 0, color);
        }
        texture.Apply();
        Rect rect = new Rect(0, 0, texture.width, texture.height);

        bar = forceGameUI.GetComponent<Image>();
        bar.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
}
