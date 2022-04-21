using System;

public class EventManager : Singleton<EventManager>
{
    public Action<LaneSide> OnTrafficLaneStart;
    public Action<LaneSide> OnTrafficLaneEnd;
}
