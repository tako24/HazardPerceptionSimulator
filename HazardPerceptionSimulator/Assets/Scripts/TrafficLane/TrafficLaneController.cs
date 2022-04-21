using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaneSide { Left, Ahead, Right }

public class TrafficLaneController : MonoBehaviour
{
    [SerializeField] private string trafficLaneCollidersTag = "TrafficLane";

    private CarController carController;
    private Dictionary<TrafficLane, TrafficLaneTriggeredColliders> trafficLanesCollision = new ();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(trafficLaneCollidersTag))
        {
            TrafficLane trafficLane = other.GetComponentInParent<TrafficLane>();

            if (carController.AheadTrafficLane == trafficLane)
                return;

            if (trafficLanesCollision.ContainsKey(trafficLane) == false)
            {
                trafficLanesCollision.Add(trafficLane, new TrafficLaneTriggeredColliders());
            }
            trafficLanesCollision[trafficLane].trafficLaneTriggeredColliders.Add(other);

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

            if (trafficLanesCollision.ContainsKey(trafficLane) == true)
            {
                trafficLanesCollision[trafficLane].trafficLaneTriggeredColliders.Remove(other);
                if (trafficLanesCollision[trafficLane].trafficLaneTriggeredColliders.Count == 0)
                {
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
                    trafficLanesCollision.Remove(trafficLane);
                }
            }
        }
    }

    private class TrafficLaneTriggeredColliders
    {
        public List<Collider> trafficLaneTriggeredColliders = new List<Collider>();
    }
}
