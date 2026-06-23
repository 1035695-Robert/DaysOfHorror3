//using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class BallDirectionForce : MonoBehaviour
{
    [SerializeField] private GameObject Ball;
    private Rigidbody ballRb;
    [Range(0,20)]
    [SerializeField]private float forceValue;

    public float angle;

    public void Start()
    {
        ballRb = GameObject.Find("Ball").GetComponent<Rigidbody>();
    }
    public void ConfirmForceAmount(double forceOutput)
    {
        float forceStrength = forceValue * (float)forceOutput;
        Debug.Log(forceStrength);
        
        Vector3 pullDirection = (transform.position - ballRb.position).normalized;

        ballRb.AddForce(pullDirection * forceStrength, ForceMode.Impulse);
    }

}
