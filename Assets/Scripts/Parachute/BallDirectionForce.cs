using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class BallDirectionForce : MonoBehaviour
{
    public GameObject Ball;
    private Rigidbody ballRb;
    public float forceValue;

    public float angle;

    public void Start()
    {
        ballRb = Ball.GetComponent<Rigidbody>();
    }
    public void ConfirmForceAmount(double forceOutput)
    {
        float forceStrength = forceValue * (float)forceOutput;
        Debug.Log(forceStrength);
        Vector3 playerForward = transform.forward;

        float randomAngle = Random.Range(-angle / 2, angle / 2);
        Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);

        Vector3 forceDirection = randomRotation * playerForward;

        ballRb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
    }


}
