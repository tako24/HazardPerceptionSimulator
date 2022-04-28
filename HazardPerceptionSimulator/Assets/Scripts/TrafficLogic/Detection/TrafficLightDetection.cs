using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightDetection : MonoBehaviour
{
    [SerializeField] private string beginningTrafficLightColliderTag = "BeginningTrafficLight";
    [SerializeField] private bool isPlayer = false;

    private TrafficLightLane trafficLightLane = null;
    private BeginningTrafficLight beginningTrafficLight = null;
    private CarController carController;

    private void Start()
    {
        carController = GetComponentInParent<CarController>();
    }

    public void TrafficLightEndPassing()
    {
        carController.AheadLane = trafficLightLane.GetContinuingTrafficLane();
        carController.IsPassingTrafficLight = false;
        carController.ChangeTrafficLightPassingState(false);
        trafficLightLane = null;
        beginningTrafficLight = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(beginningTrafficLightColliderTag))
        {
            beginningTrafficLight = other.GetComponent<BeginningTrafficLight>();
            carController.EnableIsPassingTrafficLightState();

            carController.LeftLane = beginningTrafficLight.GetLeftTrafficLightLane();
            carController.AheadLane = beginningTrafficLight.GetAheadTrafficLightLane();
            carController.RightLane = beginningTrafficLight.GetRightTrafficLightLane();

            if (isPlayer)
                EventManager.Instance.ChangeTurnsSignalsStates.Invoke(carController.LeftLane, carController.AheadLane, carController.RightLane);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(beginningTrafficLightColliderTag))
        {
            if (isPlayer)
            {
                EventManager.Instance.ChangeTurnsSignalsStates.Invoke(false, false, false);
                TrafficLight trafficLight = other.GetComponentInParent<TrafficLight>();
                if (trafficLight && trafficLight.CurrentTrafficLightState != TrafficLight.TrafficLightState.Green)
                    EventManager.Instance.OnMistake(new Mistake("Выезд на запрещающий сигнал светофора"));
            }

            trafficLightLane = (TrafficLightLane)carController.AheadLane;
            carController.ChangeTrafficLightPassingState(true);
        }
    }
}
