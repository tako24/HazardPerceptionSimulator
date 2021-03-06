using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningTrafficLight : MonoBehaviour
{
    [SerializeField] private TrafficLightLane leftTrafficLightLane;
    [SerializeField] private TrafficLightLane aheadTrafficLightLane;
    [SerializeField] private TrafficLightLane rightTrafficLightLane;

    public TrafficLightLane GetLeftTrafficLightLane() => leftTrafficLightLane;
    public TrafficLightLane GetAheadTrafficLightLane() => aheadTrafficLightLane;
    public TrafficLightLane GetRightTrafficLightLane() => rightTrafficLightLane;
}
