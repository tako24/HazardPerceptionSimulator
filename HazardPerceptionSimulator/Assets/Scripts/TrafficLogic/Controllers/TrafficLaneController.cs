using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaneSide { Left, Ahead, Right }

public class TrafficLaneController : MonoBehaviour
{
    [SerializeField] private string trafficLaneCollidersTag = "TrafficLane";
    [SerializeField] private bool isPlayer = false;
    private CarController carController;
    private Dictionary<Lane, Collider> trafficLanesCollisions = new ();
    private Collider trafficLanesDetectionCollider;

    private void Start()
    {
        carController = GetComponentInParent<CarController>();
        trafficLanesDetectionCollider = GetComponent<Collider>();
    }

    public void ChangeColliderState(bool state)
    {
        trafficLanesDetectionCollider.enabled = !trafficLanesDetectionCollider.enabled;

        if (state == false)
            trafficLanesCollisions.Clear();
    }

    public Collider GetTrafficLaneCollider(Lane trafficLane)
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

            if (carController.AheadLane == trafficLane)
                return;

            if (trafficLanesCollisions.ContainsKey(trafficLane) == false)
                trafficLanesCollisions.Add(trafficLane, other);
            else
                trafficLanesCollisions[trafficLane] = other;

            // cross нужна проверка более гибкая, а не на начало traffic lane
            Vector3 cross = Vector3.Cross((trafficLane.transform.position - transform.position).normalized, transform.forward);
            LaneSide laneSide;

            if (cross.y > 0.01f && carController.LeftLane != trafficLane)
            {
                carController.LeftLane = trafficLane;
                laneSide = LaneSide.Left;
            }
            else if (cross.y < -0.01f && carController.RightLane != trafficLane)
            {
                carController.RightLane = trafficLane;
                laneSide = LaneSide.Right;
            }
            else
                return;

            if (isPlayer)
                EventManager.Instance.OnTrafficLaneStart.Invoke(laneSide);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(trafficLaneCollidersTag))
        {
            TrafficLane trafficLane = other.GetComponentInParent<TrafficLane>();

            if (carController.AheadLane == trafficLane || !trafficLanesDetectionCollider.enabled)
                return;

            if (trafficLanesCollisions.ContainsKey(trafficLane) == true)
            {
                if (trafficLanesCollisions[trafficLane] == other)
                {
                    trafficLanesCollisions.Remove(trafficLane);
                    LaneSide laneSide;
                    if (carController.LeftLane == trafficLane)
                    {
                        laneSide = LaneSide.Left;
                        carController.LeftLane = null;
                    }
                    else if (carController.RightLane == trafficLane)
                    {
                        laneSide = LaneSide.Right;
                        carController.RightLane = null;
                    }
                    else
                        return;

                    if (isPlayer)
                        EventManager.Instance.OnTrafficLaneEnd.Invoke(laneSide);
                }
            }
        }
    }
}
