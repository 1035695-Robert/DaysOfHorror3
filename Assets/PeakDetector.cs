using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PeakDetector : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasReachedPeak = false;
    private float PreviousYVelocity;

    public QuickTimeEvent qte;
    public GameObject QTE;
    public GameObject UI;
    private void Start()
    {
       rb = GetComponent<Rigidbody>();
        qte = QTE.GetComponent<QuickTimeEvent>();   
    }

    public void StartCheck()
    {
        StartCoroutine(CheckPeakArc());
    }
     public IEnumerator CheckPeakArc()
    {
        hasReachedPeak = false;
        Debug.Log("check for Peak");
        while (!hasReachedPeak)
        {
            if (PreviousYVelocity > 0 && rb.linearVelocity.y <= 0)
            {
                Debug.Log("reached peak");
                qte.Pointer();
                hasReachedPeak = true;
                
            }
            PreviousYVelocity = rb.linearVelocity.y;
            yield return null;
        }
        yield break;

    }
}
