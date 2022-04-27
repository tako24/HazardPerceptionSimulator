using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrafficLight : MonoBehaviour
{
    // здесь нужно переключать свет у светофора и вызывать событие, что загорелся зеленый у всех, кто подписан с нужной полосы
    [SerializeField] private List<Light> greenSignals;
    [SerializeField] private List<Light> yellowSignals;
    [SerializeField] private List<Light> redSignals;
    [SerializeField] private List<MeshRenderer> trafficLights;
    [field: SerializeField] public TrafficLightState CurrentTrafficLightState { get; private set; } = TrafficLightState.Red;

    public Action GreenSignalIsOn;
    public enum TrafficLightState { Green, Yellow, Red }

    public void EnableTrafficLightSignal(TrafficLightState trafficLightState)
    {
        foreach (MeshRenderer trafficLight in trafficLights)
        {
            Material meshMaterial = trafficLight.material;
            switch (trafficLightState)
            {
                case TrafficLightState.Green:
                    ChangeYellowSignalsState(false);
                    ChangeRedSignalsState(false);
                    ChangeGreenSignalsState(true);
                    meshMaterial.mainTextureOffset = new Vector2(0.667f, 0f);
                    if (GreenSignalIsOn != null)
                        GreenSignalIsOn.Invoke();
                    break;
                case TrafficLightState.Yellow:
                    ChangeRedSignalsState(false);
                    ChangeGreenSignalsState(false);
                    ChangeYellowSignalsState(true);
                    meshMaterial.mainTextureOffset = new Vector2(0.334f, 0f);
                    break;
                case TrafficLightState.Red:
                    ChangeYellowSignalsState(false);
                    ChangeGreenSignalsState(false);
                    ChangeRedSignalsState(true);
                    meshMaterial.mainTextureOffset = new Vector2(0f, 0f);
                    break;
            }
            trafficLight.material = meshMaterial;
        }
        CurrentTrafficLightState = trafficLightState;
    }

    private void ChangeGreenSignalsState(bool state)
    {
        greenSignals.ForEach(signal => signal.enabled = state);
    }

    private void ChangeYellowSignalsState(bool state)
    {
        yellowSignals.ForEach(signal => signal.enabled = state);
    }

    private void ChangeRedSignalsState(bool state)
    {
        redSignals.ForEach(signal => signal.enabled = state);
    }
}
