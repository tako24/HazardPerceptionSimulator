using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float switchTrafficLaneTime = 2f;
    [SerializeField] private bool isPlayer = false;

    public Lane AheadLane;
    public Lane RightLane;
    public Lane LeftLane;

    public bool IsPassingTrafficLight { set { isPassingTrafficLight = value; } }

    protected Transform targetPathPoint = null;
    protected TrafficLaneController trafficLaneController;
    protected TrafficLightController trafficLightController;
    protected bool isChangingTrafficLane = false;
    protected bool isPassingTrafficLight = false;
    protected int currentPathPointIndex = 0;

    private Vector3 endSwitchTrafficLanePosition;
    private float endSwitchTrafficLanePath;

    protected virtual void Awake()
    {
        trafficLaneController = GetComponentInChildren<TrafficLaneController>();
        trafficLightController = GetComponentInChildren<TrafficLightController>();
    }

    private void FixedUpdate()
    {
        if (isChangingTrafficLane)
        {
            transform.position = Vector3.MoveTowards(transform.position, endSwitchTrafficLanePosition, speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(endSwitchTrafficLanePosition - transform.position), speed * 13.3f * Time.fixedDeltaTime);

            if ((transform.position - endSwitchTrafficLanePosition).sqrMagnitude < 0.1f)
            {
                targetPathPoint = AheadLane.GetPathPoint(endSwitchTrafficLanePath);
                ChangeTrafficLaneColliderState(true); // включаем обнаружение Traffic Lane
                isChangingTrafficLane = false;
            }
            return;
        }

        if ((transform.position - targetPathPoint.position).sqrMagnitude < 0.1f)
        {
            targetPathPoint = AheadLane.GetPathPoint(++currentPathPointIndex);
        }

        if (targetPathPoint == null)
        {
            if (isPassingTrafficLight)
                trafficLightController.TrafficLightEndPassing();
            else
                enabled = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPathPoint.position, speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.LookRotation(targetPathPoint.position - transform.position), speed * 13.3f * Time.fixedDeltaTime);
    }

    public void SwitchTrafficLane(LaneSide laneSide)
    {
        if (isPassingTrafficLight)
        {
            AheadLane = laneSide == LaneSide.Left ? LeftLane : RightLane;
            if (isPlayer)
                EventManager.Instance.ChangeTurnsSignalsStates.Invoke(false, false, false);
            return;
        }

        isChangingTrafficLane = true;
        ChangeTrafficLaneColliderState(false); // выключаем обнаружение Traffic Lane
        float switchTrafficLanePath = speed * switchTrafficLaneTime * 0.8f;
        AheadLane = laneSide == LaneSide.Left ? LeftLane : RightLane;
        ClearAdjacentLanes();

        Collider trafficLaneClosestCollider = trafficLaneController.GetTrafficLaneCollider(AheadLane);
        var direction = laneSide == LaneSide.Left ? -transform.right : transform.right;
        Ray ray = new Ray(transform.position + Vector3.up, direction);
        if (trafficLaneClosestCollider.Raycast(ray, out RaycastHit hit, 5f))
        {
            endSwitchTrafficLanePath = AheadLane.GetPathByPointOnCollider(trafficLaneClosestCollider, hit.point + direction * ((TrafficLane)AheadLane).ColliderWidth / 2);
            endSwitchTrafficLanePath += switchTrafficLanePath;
            if (endSwitchTrafficLanePath > AheadLane.LaneLength)
                endSwitchTrafficLanePath = AheadLane.LaneLength;
            endSwitchTrafficLanePosition = AheadLane.GetEndSwitchTrafficLanePosition(endSwitchTrafficLanePath);
        }
    }

    public void EnableIsPassingTrafficLightState()
    {
        isChangingTrafficLane = false;
        targetPathPoint = AheadLane.GetPathPoint(endSwitchTrafficLanePath);
        isPassingTrafficLight = true;
    }

    public void ChangeTrafficLightPassingState(bool state)
    {
        ChangeTrafficLaneColliderState(!state); // изменяем обнаружение Traffic Lane
        if (state == false)
            ClearAdjacentLanes();
        currentPathPointIndex = 0;
        targetPathPoint = AheadLane.GetPathPoint(currentPathPointIndex);
    }

    private void ChangeTrafficLaneColliderState(bool state)
    {
        trafficLaneController.ChangeColliderState(state);
    }

    private void ClearAdjacentLanes()
    {
        LeftLane = null;
        RightLane = null;
    }
}
