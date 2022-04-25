using UnityEngine;
using UnityEngine.UI;

public class TurnsSignalsController : MonoBehaviour
{
    [SerializeField] private Button rightTurnSignal;
    [SerializeField] private Button leftTurnSignal;

    private void Start()
    {
        EventManager.Instance.OnTrafficLaneStart += (laneSide) => TurnSignalClassifier(laneSide, true);
        EventManager.Instance.OnTrafficLaneEnd += (laneSide) => TurnSignalClassifier(laneSide, false);
        EventManager.Instance.ChangeTurnsSignalsStates += ChangeTurnsSignalsStates;
    }

    private void ChangeTurnsSignalsStates(bool leftLane, bool aheadLane, bool rightLane)
    {
        leftTurnSignal.interactable = leftLane;
        rightTurnSignal.interactable = rightLane;
    }

    private void TurnSignalClassifier(LaneSide laneSide, bool state)
    {
        switch (laneSide)
        {
            case LaneSide.Left:
                leftTurnSignal.interactable = state;
                break;
            case LaneSide.Right:
                rightTurnSignal.interactable = state;
                break;
        }
    }
}
