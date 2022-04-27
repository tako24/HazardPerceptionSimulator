using System;

public class EventManager : Singleton<EventManager>
{   // only for player
    public Action<LaneSide> OnTrafficLaneStart;
    public Action<LaneSide> OnTrafficLaneEnd;
    public Action<bool, bool, bool> ChangeTurnsSignalsStates;
    public Action<Mistake> OnMistake;
}
