using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;
using JetBrains.Annotations;
using DG.Tweening;
using UnityEditor;



public class QuickTimeEvent : MonoBehaviour
{
    public GameObject forceGameUI;

    public RectTransform pointer;
    public RectTransform center;
    public RectTransform pointA;
    public RectTransform pointB;
    public Image bar;

    public int width = 300;
    public RandomSpeedList RSL;
    public Gradient gradient;



    [Range(0, 1)]
    public float forcePercentage;
    public InputActionReference lift;

    public InputManager inputManager;
    private PlayerInput playerInputManager;

    public float randomSpeed;
    private LaunchBall launchBall;

    private Tween qteTween;

    private Transform mainCamera;

    private void OnEnable()
    {
        EventManager.tossRound += ForceUI;
        lift.action.Enable();
    }
    private void OnDisable()
    {
        lift.action.Disable();
    }

    public void Start()
    {
        launchBall = GameObject.Find("Ball").GetComponent<LaunchBall>();
        RSL = FindAnyObjectByType<RandomSpeedList>();
        inputManager = FindAnyObjectByType<InputManager>().instance;
        playerInputManager = inputManager.GetComponent<PlayerInput>();
        if (playerInputManager.currentActionMap != null && playerInputManager.currentActionMap.name != "UI")
        { playerInputManager.SwitchCurrentActionMap("UI"); }
        mainCamera = Camera.main.transform;
        GetUIComponents();

    }
    void GetUIComponents()
    {
        Canvas canvas = GetComponent<Canvas>();

        forceGameUI = canvas.transform.Find("Bar").gameObject;
        pointA = forceGameUI.transform.Find("A").GetComponent<RectTransform>();
        pointB = forceGameUI.transform.Find("B").GetComponent<RectTransform>();
        center = forceGameUI.transform.Find("center").GetComponent<RectTransform>();
        pointer = forceGameUI.transform.Find("Pointer").GetComponent<RectTransform>();
        CameraSetup();
        GradientUI();
    }
    void CameraSetup()
    {
        Vector3 uiPosition = transform.position;
        uiPosition.y = 2f;

        forceGameUI.transform.position = uiPosition;


        Quaternion uiRotation = forceGameUI.transform.rotation;
        uiRotation.z = transform.rotation.y;


    }
    public void ForceUI(int round)
    {
        Debug.Log(round + "/ " + (RSL.rsRange.Count - 1));
        if (round <= RSL.rsRange.Count - 1)
        {
            randomSpeed = UnityEngine.Random.Range(RSL.rsRange[round].speedValues.x, RSL.rsRange[round].speedValues.y);
            Debug.Log("min:" + RSL.rsRange[0].speedValues.x + " Max:" + RSL.rsRange[0].speedValues.y);
        }
        else
        {
            KillCollision killcol = FindAnyObjectByType<KillCollision>();
            killcol.NoMoreRounds();
        }

            Vector3 startPosition = pointA.position;
        Vector3 endPosition = pointB.position;

        pointer.position = startPosition;

        qteTween = pointer.DOMove(endPosition, randomSpeed)
             .SetLoops(-1, LoopType.Yoyo)
             .SetEase(Ease.Linear);


        StartCoroutine(MovePointer(randomSpeed));
    }
    private IEnumerator MovePointer(float AdditionalSpeed)
    {
        Debug.Log("good Day");

        forceGameUI.SetActive(true);

        Vector3 startPosition = pointA.position;
        Vector3 endPosition = pointB.position;
        float distance = Vector2.Distance(startPosition, endPosition);

        pointer.position = startPosition;

        while (true)
        {
            float targetDistance = Vector2.Distance(pointer.position, center.position);
            float range = Mathf.Clamp01(targetDistance / (width / 2));
            forcePercentage = 1.0f - range;

            if (lift.action.WasPerformedThisFrame())
            {
                //double roundedValue = Math.Round(forcePercentage);
                StoreForceValue(forcePercentage);
                qteTween.Kill();
                forceGameUI.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
    public void StoreForceValue(float Value)
    {
        int index = launchBall.playerForce.FindIndex(player => player.player == transform.root.gameObject);
        Debug.Log(index);
        launchBall.playerForce[index].forceValue = Value;
        launchBall.isPaused = false;
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

        bar = forceGameUI.GetComponent<Image>();
        bar.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
}
