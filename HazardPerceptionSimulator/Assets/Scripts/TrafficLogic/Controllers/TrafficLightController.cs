using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
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
        carController.AheadLane = trafficLightLane.GetAdjacentTrafficLane();
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

            // включить поворотники в соответствии с доступными путями.
            // выбор пути из доступных по правилам поворотников.

            //carController.AheadLane = trafficLightLane;
            //carController.ChangeTrafficLightState(true);
            (bool, bool, bool) checkLanes = beginningTrafficLight.CheckTrafficLightLanes(); // left, ahead, right

            if (checkLanes.Item1)
                carController.LeftLane = beginningTrafficLight.GetLeftTrafficLightLane();
            if (checkLanes.Item2)
                carController.AheadLane = beginningTrafficLight.GetAheadTrafficLightLane();
            if (checkLanes.Item3)
                carController.RightLane = beginningTrafficLight.GetRightTrafficLightLane();

            carController.ChangeIsPassingTrafficLightState(true);

            if (isPlayer)
                EventManager.Instance.ChangeTurnsSignalsStates.Invoke(checkLanes.Item1, checkLanes.Item2, checkLanes.Item3);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(beginningTrafficLightColliderTag))
        {
            // к этому моменту уже должен быть выбран маршрут, т.к. сейчас он придет в действие

            if (isPlayer)
                EventManager.Instance.ChangeTurnsSignalsStates.Invoke(false, false, false);

            trafficLightLane = (TrafficLightLane)carController.AheadLane;

            carController.ChangeTrafficLightPassingState(true);
            //EventManager.Instance.DisableTurnsSignals.Invoke();
        }
    }
}
