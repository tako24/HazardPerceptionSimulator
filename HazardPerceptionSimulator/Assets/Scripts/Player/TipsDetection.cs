using UnityEngine;

public class TipsDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tip"))
        {
            other.GetComponent<Tip>().Implement();
        }
    }
}
