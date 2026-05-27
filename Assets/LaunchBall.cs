using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class LaunchBall : MonoBehaviour
{
    private Rigidbody rb;
    private PeakDetector peak;

    public float launchForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        peak = GetComponent<PeakDetector>();
    }

   void Launch()
    {
        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        peak.StartCheck();
    }

    private void OnCollisionEnter(Collision collision)
    {
     if (collision.transform.name == "ground")
        {
            Debug.Log("launch");
            Launch();
        }
    }
}
