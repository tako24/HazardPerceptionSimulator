using UnityEngine;

public class PlayerController : CarController
{
    [SerializeField] [Tooltip("Pause between speed changes")] 
    private float changeSpeedTimer = 0.2f;
    [SerializeField] [Tooltip("Value by which the speed will change")] 
    private float speedChangeValue = 1f;

    private float direction = 0f;
    private float currentTime = 0f;

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

    private void Update()
    {
        if (direction == 0f)
            return;

        currentTime += Time.deltaTime;
        if (currentTime >= changeSpeedTimer)
        {
            currentTime = 0f;
            speed += direction;
            if (speed < 0)
            {
                speed = 0f;
                direction = 0f;
            }
        }
    }
}
