using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    public TrafficLane AheadTrafficLane;
    public TrafficLane RightTrafficLane;
    public TrafficLane LeftTrafficLane;

    protected Transform targetPathPoint = null;
    protected TrafficLaneController trafficLaneController;
    protected bool isChangingTrafficLane = false;
    protected int currentPathPointIndex = 0;

    protected virtual void Awake()
    {
        trafficLaneController = GetComponentInChildren<TrafficLaneController>();
    }

    private void FixedUpdate()
    {
        if (isChangingTrafficLane)
            return;

        if ((transform.position - targetPathPoint.position).sqrMagnitude < 0.1f)
            targetPathPoint = AheadTrafficLane.GetPathPoint(++currentPathPointIndex);

        if (targetPathPoint == null)
        {
            enabled = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPathPoint.position, speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.LookRotation(targetPathPoint.position - transform.position), rotationSpeed * Time.fixedDeltaTime);
    }

    protected virtual void SwitchTrafficLane(LaneSide laneSide)
    {
        isChangingTrafficLane = true;
        currentPathPointIndex = 0;
        trafficLaneController.ChangeColliderState(); // выключаем обнаружение Traffic Lane

        switch (laneSide)
        {
            case LaneSide.Left:
                // включение логики перестроения к LeftTrafficLane
                AheadTrafficLane = LeftTrafficLane;
                Debug.Log("left");
                break;
            case LaneSide.Right:
                // включение логики перестроения к RightTrafficLane
                AheadTrafficLane = RightTrafficLane;
                Debug.Log("right");
                break;
        }

        Collider trafficLaneClosestCollider = trafficLaneController.GetTrafficLaneCollider(AheadTrafficLane);

        targetPathPoint = AheadTrafficLane.GetPathPoint(System.Convert.ToInt32(trafficLaneClosestCollider.name.Split('-')[1]));

        // очищаем боковые Traffic Lane
        LeftTrafficLane = null;
        RightTrafficLane = null;

        // нужно менять стейт только после полного перестроения!!!
        trafficLaneController.ChangeColliderState(); // включаем обнаружение Traffic Lane
        isChangingTrafficLane = false;
    }
}
