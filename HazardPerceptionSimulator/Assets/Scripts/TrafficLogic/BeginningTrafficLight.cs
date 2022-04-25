using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningTrafficLight : MonoBehaviour
{
    [SerializeField] private TrafficLightLane leftTrafficLightLane;
    [SerializeField] private TrafficLightLane aheadTrafficLightLane;
    [SerializeField] private TrafficLightLane rightTrafficLightLane;

    private (bool, bool, bool) trafficLightLanes;

    private void Awake()
    {
        trafficLightLanes = (leftTrafficLightLane != null, aheadTrafficLightLane != null, rightTrafficLightLane != null);
    }

    public (bool, bool, bool) CheckTrafficLightLanes() => trafficLightLanes;

    public TrafficLightLane GetLeftTrafficLightLane() => leftTrafficLightLane;
    public TrafficLightLane GetAheadTrafficLightLane() => aheadTrafficLightLane;
    public TrafficLightLane GetRightTrafficLightLane() => rightTrafficLightLane;
}
