using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    public Lane AheadLane;
    public Lane RightLane;
    public Lane LeftLane;

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

    public virtual void SwitchTrafficLane(LaneSide laneSide)
    {
        isChangingTrafficLane = true;
        ChangeTrafficLaneColliderState(false); // выключаем обнаружение Traffic Lane
        AheadLane = laneSide == LaneSide.Left ? LeftLane : RightLane;

        Collider trafficLaneClosestCollider = trafficLaneController.GetTrafficLaneCollider(AheadLane);
        currentPathPointIndex = System.Convert.ToInt32(trafficLaneClosestCollider.name.Split('-')[1]);
        targetPathPoint = AheadLane.GetPathPoint(currentPathPointIndex);

        // нужно менять стейт только после полного перестроения!!!
        // Сейчас это костыль, чтобы машина просто ехала к следующей точке
        ChangeTrafficLaneColliderState(true); // включаем обнаружение Traffic Lane
        isChangingTrafficLane = false;
    }

    public void ChangeTrafficLightState(bool state)
    {
        isPassingTrafficLight = !isPassingTrafficLight;
        ChangeTrafficLaneColliderState(!state); // изменяем обнаружение Traffic Lane

        if (state == true)
            currentPathPointIndex = 0;

        targetPathPoint = AheadLane.GetPathPoint(currentPathPointIndex);
    }

    private void ChangeTrafficLaneColliderState(bool state)
    {
        trafficLaneController.ChangeColliderState(state);

        if (state == false) // очищаем боковые Traffic Lane
        {
            LeftLane = null;
            RightLane = null;
        }
    }
}
