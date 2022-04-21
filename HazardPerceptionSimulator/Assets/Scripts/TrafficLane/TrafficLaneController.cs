using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaneSide { Left, Ahead, Right }

public class TrafficLaneController : MonoBehaviour
{
    [SerializeField] private string trafficLaneCollidersTag = "TrafficLane";

    private CarController carController;
    private Dictionary<TrafficLane, Collider> trafficLanesCollisions = new ();
    private Collider trafficLanesDetectionCollider;

    private void Start()
    {
        carController = GetComponentInParent<CarController>();
        trafficLanesDetectionCollider = GetComponent<Collider>();
    }

    public void ChangeColliderState()
    {
        trafficLanesDetectionCollider.enabled = !trafficLanesDetectionCollider.enabled;
    }

    public Collider GetTrafficLaneCollider(TrafficLane trafficLane)
    {
        if (trafficLanesCollisions.ContainsKey(trafficLane))
            return trafficLanesCollisions[trafficLane];
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(trafficLaneCollidersTag))
        {
            TrafficLane trafficLane = other.GetComponentInParent<TrafficLane>();

            if (carController.AheadTrafficLane == trafficLane)
                return;

            if (trafficLanesCollisions.ContainsKey(trafficLane) == false)
                trafficLanesCollisions.Add(trafficLane, other);
            else
                trafficLanesCollisions[trafficLane] = other;

            Vector3 cross = Vector3.Cross((trafficLane.transform.position - transform.position).normalized, transform.forward);
            LaneSide laneSide;

            if (cross.y > 0.01f && carController.LeftTrafficLane != trafficLane)
            {
                carController.LeftTrafficLane = trafficLane;
                laneSide = LaneSide.Left;
            }
            else if (cross.y < -0.01f && carController.RightTrafficLane != trafficLane)
            {
                carController.RightTrafficLane = trafficLane;
                laneSide = LaneSide.Right;
            }
            else
                return;
            EventManager.Instance.OnTrafficLaneStart.Invoke(laneSide);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(trafficLaneCollidersTag))
        {
            TrafficLane trafficLane = other.GetComponentInParent<TrafficLane>();

            if (carController.AheadTrafficLane == trafficLane)
                return;

            if (trafficLanesCollisions.ContainsKey(trafficLane) == true)
            {
                if (trafficLanesCollisions[trafficLane] == other)
                {
                    trafficLanesCollisions.Remove(trafficLane);
                    LaneSide laneSide;
                    if (carController.LeftTrafficLane == trafficLane)
                    {
                        laneSide = LaneSide.Left;
                        carController.LeftTrafficLane = null;
                    }
                    else if (carController.RightTrafficLane == trafficLane)
                    {
                        laneSide = LaneSide.Right;
                        carController.RightTrafficLane = null;
                    }
                    else
                        return;

                    EventManager.Instance.OnTrafficLaneEnd.Invoke(laneSide);
                }
            }
        }
    }

    private class TrafficLaneTriggeredColliders
    {
        public List<Collider> trafficLaneTriggeredColliders = new List<Collider>();
    }
}
