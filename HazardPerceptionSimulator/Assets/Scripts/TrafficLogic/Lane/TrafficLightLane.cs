using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightLane : Lane
{
    [SerializeField] private TrafficLane adjacentTrafficLane;

    public TrafficLane GetAdjacentTrafficLane() => adjacentTrafficLane;
}
