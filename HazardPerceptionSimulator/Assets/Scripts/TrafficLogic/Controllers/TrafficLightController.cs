using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [SerializeField] private string trafficLightCollidersTag = "TrafficLightLane";
    [SerializeField] private bool isPlayer = false;

    private TrafficLightLane trafficLightLane = null;
    private CarController carController;

    private void Start()
    {
        carController = GetComponentInParent<CarController>();
    }

    public void TrafficLightEndPassing()
    {
        carController.AheadLane = trafficLightLane.GetAdjacentTrafficLane();
        carController.ChangeTrafficLightState(true);
        trafficLightLane = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(trafficLightCollidersTag))
        {
            TrafficLightLane trafficLightLane = other.GetComponentInParent<TrafficLightLane>();

            if (this.trafficLightLane == null)
                this.trafficLightLane = trafficLightLane;
            else
                return;

            carController.AheadLane = trafficLightLane;
            carController.ChangeTrafficLightState(true);

            if (isPlayer)
                EventManager.Instance.DisableTurnsSignals.Invoke();
        }
    }
}
