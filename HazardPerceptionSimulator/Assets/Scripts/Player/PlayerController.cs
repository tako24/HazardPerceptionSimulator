using UnityEngine;
using TMPro;
using System;

public class PlayerController : CarController
{
    [SerializeField] [Tooltip("Pause between speed changes")] 
    private float changeSpeedTimer = 0.2f;
    [SerializeField] [Tooltip("Value by which the speed will change")] 
    private float speedChangeValue = 1f;

    [SerializeField] [Tooltip("The traffic lane the player will be teleported to at the awake")]
    private TrafficLane startTrafficLane;

    [SerializeField] private TMP_Text speedText;

    private float direction = 0f;
    private float currentTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        AheadLane = startTrafficLane;
        targetPathPoint = AheadLane.GetPathPoint(currentPathPointIndex);
        if (targetPathPoint == null)
            enabled = false;

        transform.position = targetPathPoint.position;
        speedText.text = Convert.ToString((int)(speed * 5)) + " ��/�";
    }

    private void Update()
    {
        if (direction == 0f)
            return;

        currentTime += Time.deltaTime;
        if (currentTime >= changeSpeedTimer)
        {
            currentTime = 0f;
            speed += direction;
            if (speed < 0.4f)
            {
                speed = 0f;
                direction = 0f;
            }
            if (speed > maxSpeedInKmH / 5)
            {
                speed = maxSpeedInKmH / 5;
                direction = 0f;
            }
            speedText.text = Convert.ToString((int)(speed * 5)) + " ��/�";
        }
    }

    public void IncreaseSpeedStart()
    {
        direction = speedChangeValue;
    }

    public void DecreaseSpeedStart()
    {
        direction = -speedChangeValue;
    }

    public void StopChangingSpeed()
    {
        direction = 0f;
        currentTime = 0f;
    }

    public void SwitchTrafficLane(string laneSideSignal) // from UI turns signals
    {
        System.Enum.TryParse(laneSideSignal, out LaneSide laneSide);
        EventManager.Instance.ChangeTurnsSignalsStates.Invoke(false, false, false); // ��������� ����������� ��������
        SwitchTrafficLane(laneSide);
    }
}
