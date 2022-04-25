using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
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

    protected virtual void Awake()
    {
        trafficLaneController = GetComponentInChildren<TrafficLaneController>();
        trafficLightController = GetComponentInChildren<TrafficLightController>();
    }

    private void FixedUpdate()
    {
        if (isChangingTrafficLane)
            return;

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
        transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.LookRotation(targetPathPoint.position - transform.position), rotationSpeed * Time.fixedDeltaTime);
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
        ChangeTrafficLaneColliderState(false); // ��������� ����������� Traffic Lane
        AheadLane = laneSide == LaneSide.Left ? LeftLane : RightLane;

        Collider trafficLaneClosestCollider = trafficLaneController.GetTrafficLaneCollider(AheadLane);
        currentPathPointIndex = System.Convert.ToInt32(trafficLaneClosestCollider.name.Split('-')[1]);
        targetPathPoint = AheadLane.GetPathPoint(currentPathPointIndex);

        // ����� ������ ����� ������ ����� ������� ������������!!!
        // ������ ��� �������, ����� ������ ������ ����� � ��������� �����
        ChangeTrafficLaneColliderState(true); // �������� ����������� Traffic Lane
        isChangingTrafficLane = false;
    }

    public void ChangeIsPassingTrafficLightState(bool state)
    {
        isPassingTrafficLight = !isPassingTrafficLight;
    }

    public void ChangeTrafficLightPassingState(bool state)
    {
        ChangeTrafficLaneColliderState(!state); // �������� ����������� Traffic Lane
        currentPathPointIndex = 0;
        targetPathPoint = AheadLane.GetPathPoint(currentPathPointIndex);
    }

    private void ChangeTrafficLaneColliderState(bool state)
    {
        trafficLaneController.ChangeColliderState(state);

        if (state == false) // ������� ������� Traffic Lane
        {
            LeftLane = null;
            RightLane = null;
        }
    }
}
