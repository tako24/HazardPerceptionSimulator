using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLaneController : MonoBehaviour
{
    [SerializeField] private string laneStartCollisionTag = "LaneStart";
    [SerializeField] private string laneEndCollisionTag = "LaneEnd";

    private List<Transform> availableTrafficLanes = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(laneStartCollisionTag))
        {
            Transform parentTransform = other.transform.parent.parent;
            if (availableTrafficLanes.Contains(parentTransform) == false)
            {
                //Debug.Log("Collision with the new lane - " + parentTransform.name);
                availableTrafficLanes.Add(parentTransform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(laneEndCollisionTag))
        {
            Transform parentTransform = other.transform.parent.parent;
            if (availableTrafficLanes.Contains(parentTransform) == true)
            {
                //Debug.Log("Collision with the end lane - " + parentTransform.name);
                availableTrafficLanes.Remove(parentTransform);
            }
        }
    }
}
