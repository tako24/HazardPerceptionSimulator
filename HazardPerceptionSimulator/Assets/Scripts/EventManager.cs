using System;

public class EventManager : Singleton<EventManager>
{
    public Action<LaneSide> OnTrafficLaneStart;
    public Action<LaneSide> OnTrafficLaneEnd;
    public Action<bool, bool, bool> ChangeTurnsSignalsStates;
}
