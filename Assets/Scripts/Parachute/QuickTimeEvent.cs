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

    public GameObject player;
    private BallDirectionForce ballDirection;
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
        GradientUI();
        forceGameUI.SetActive(false);
        ballDirection = player.GetComponent<BallDirectionForce>();
        inputManager.SwitchCurrentActionMap("UI");
        
    }

    public void Pointer()
    {
        Debug.Log("start force game");
        StartCoroutine(MovePointer());
    }
    public IEnumerator MovePointer()
    {
        Debug.Log("i am working");
        forceGameUI.SetActive(true);

        Vector2 startPosition = pointA.position;

        Vector2 endPosition = pointB.position;
        float distance = Vector2.Distance(startPosition, endPosition);

        pointer.position = startPosition;

        
        //Time.timeScale = 0.2f;

        while ((Vector2)pointer.position != endPosition)
        {
            
            pointer.position = Vector2.MoveTowards(pointer.position, endPosition, speed * Time.unscaledDeltaTime);

            float targetDistance = Vector2.Distance(pointer.position, center.position);
            float range = Mathf.Clamp01(targetDistance / (width / 2));
            forcePercentage = 1.0f - range;

            if (lift.action.WasPerformedThisFrame())
            {
               // Time.timeScale = 1;
                double roundedValue = Math.Round(forcePercentage, 2);
                ballDirection.ConfirmForceAmount(roundedValue);
                
            }
            yield return null;
        }
        //Time.timeScale = 1;
        yield return new WaitForSeconds(0.05f);
        forceGameUI.SetActive(false);
    }

    void GradientUI()
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
        bar.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
}
