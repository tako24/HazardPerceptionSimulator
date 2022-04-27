using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaneSide { Left, Ahead, Right }

public class TrafficLaneDetection : MonoBehaviour
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
        if (state == true)
            trafficLanesCollisions.Clear();

        trafficLanesDetectionCollider.enabled = state;
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

            if (carController.AheadLane == trafficLane || carController.LeftLane == trafficLane || carController.RightLane == trafficLane)
                return;

            if (trafficLanesCollisions.ContainsKey(trafficLane) == false)
                trafficLanesCollisions.Add(trafficLane, other);
            else
                trafficLanesCollisions[trafficLane] = other;

            Ray ray = new Ray(transform.position + transform.forward/2, transform.right);
            RaycastHit hit;
            LaneSide laneSide;

            // попал справа
            laneSide = other.Raycast(ray, out hit, 5f) ? LaneSide.Right : LaneSide.Left;

            if (!carController.AheadLane.isFarRightLane && other.Raycast(ray, out hit, 10f)) // попал справа
            {
                laneSide = LaneSide.Right;
                carController.RightLane = trafficLane;
            }
            else
            {
                ray.direction = -transform.right;
                if (!carController.AheadLane.isFarLeftLane && other.Raycast(ray, out hit, 10f)) // попал слева
                {
                    laneSide = LaneSide.Left;
                    carController.LeftLane = trafficLane;
                }
                else
                    return;
            }

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

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + transform.forward / 2, transform.right * 5f, Color.blue); 
        Debug.DrawRay(transform.position + transform.forward / 2, -transform.right * 5f, Color.grey);
    }
}
