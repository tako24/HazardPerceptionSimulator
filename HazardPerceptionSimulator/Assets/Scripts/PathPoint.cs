using UnityEngine;

public class PathPoint : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        TrafficLane trafficLane = GetComponentInParent<TrafficLane>();
        if (trafficLane)
            trafficLane.AddCreatedPathPoint(transform);
    }
#endif
}
