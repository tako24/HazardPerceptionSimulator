using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    [SerializeField] private int stepId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<TutorialLevel>().EnableStepById(stepId);
            Destroy(gameObject);
        }
    }
}
