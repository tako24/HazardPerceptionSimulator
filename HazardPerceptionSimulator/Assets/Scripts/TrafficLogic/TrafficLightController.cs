using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    // здесь нужно вызывать переключение света у светофоров
    private enum TrafficLightSystemType { Single, Pair }

    [SerializeField]
    [Tooltip("Traffic lights will turn on, either one after the other, or in pairs (adjacent in the list)")]
    private TrafficLightSystemType trafficLightSystemType = TrafficLightSystemType.Pair;
    [SerializeField] private List<TrafficLight> trafficLights;
    [SerializeField] private int greenLightTime = 15;
    [SerializeField] private int yellowLightTime = 2;

    private int currentActiveTrafficLightIndex;

    private void Awake()
    {
        if (trafficLightSystemType == TrafficLightSystemType.Pair && trafficLights.Count % 2 != 0)
            throw new System.ArgumentException("For the paired inclusion of traffic lights, there must be an even number of them");
        if (trafficLights.Count == 0)
            throw new System.ArgumentException("The traffic lights list cannot be empty");

        StartCoroutine(ChangeActiveTrafficLight());
    }

    private IEnumerator ChangeActiveTrafficLight()
    {
        EnableTrafficLightsSignals(TrafficLight.TrafficLightState.Green);

        while (true)
        {
            yield return new WaitForSecondsRealtime(greenLightTime);
            EnableTrafficLightsSignals(TrafficLight.TrafficLightState.Yellow);

            yield return new WaitForSecondsRealtime(yellowLightTime);
            EnableTrafficLightsSignals(TrafficLight.TrafficLightState.Red);

            currentActiveTrafficLightIndex++;
            if (trafficLightSystemType == TrafficLightSystemType.Pair)
                currentActiveTrafficLightIndex++;
            if (currentActiveTrafficLightIndex >= trafficLights.Count)
                currentActiveTrafficLightIndex = 0;

            EnableTrafficLightsSignals(TrafficLight.TrafficLightState.Yellow);

            yield return new WaitForSecondsRealtime(yellowLightTime);

            EnableTrafficLightsSignals(TrafficLight.TrafficLightState.Green);
        }
    }

    private void EnableTrafficLightsSignals(TrafficLight.TrafficLightState trafficLightState)
    {
        trafficLights[currentActiveTrafficLightIndex].EnableTrafficLightSignal(trafficLightState);
        if (trafficLightSystemType == TrafficLightSystemType.Pair)
            trafficLights[currentActiveTrafficLightIndex + 1].EnableTrafficLightSignal(trafficLightState);
    }
}
