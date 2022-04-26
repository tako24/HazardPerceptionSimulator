using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightLane : Lane
{
    [SerializeField] private TrafficLane continuingTrafficLane;

    public TrafficLane GetContinuingTrafficLane() => continuingTrafficLane;
}
