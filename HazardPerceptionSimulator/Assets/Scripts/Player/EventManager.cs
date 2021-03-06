using System;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{   // only for player
    public Action<LaneSide> OnTrafficLaneStart;
    public Action<LaneSide> OnTrafficLaneEnd;
    public Action<bool, bool, bool> ChangeTurnsSignalsStates;
    public Action<Mistake> OnMistake;
    public Action<List<CardInfo>> ShowTipCardsInfo;
    public Action<string> ShowTipMessage;
}
