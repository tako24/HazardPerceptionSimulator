using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private TrafficLane trafficLane;

    private Transform targetPathPoint = null;

    private void Awake()
    {
        targetPathPoint = trafficLane.GetNextPathPoint();
        if (targetPathPoint == null)
            enabled = false;

        transform.position = targetPathPoint.position;
    }

    private void FixedUpdate()
    {
        if ((transform.position - targetPathPoint.position).sqrMagnitude < 0.1f)
            targetPathPoint = trafficLane.GetNextPathPoint();

        if (targetPathPoint == null)
        {
            enabled = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPathPoint.position, speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPathPoint.position - transform.position), rotationSpeed * Time.fixedDeltaTime);
    }

    public void SwitchTrafficLane()
    {

    }
}
