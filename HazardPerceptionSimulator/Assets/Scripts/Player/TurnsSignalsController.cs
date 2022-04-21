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
    }

    public void DisableTurnSignals()
    {
        rightTurnSignal.interactable = false;
        leftTurnSignal.interactable = false;
    }

    private void TurnSignalClassifier(LaneSide laneSide, bool state)
    {
        switch (laneSide)
        {
            case LaneSide.Left:
                ChangeLeftTurnSignalState(state);
                break;
            case LaneSide.Right:
                ChangeRightTurnSignalState(state);
                break;
        }
    }

    private void ChangeRightTurnSignalState(bool state)
    {
        rightTurnSignal.interactable = state;
    }

    private void ChangeLeftTurnSignalState(bool state)
    {
        leftTurnSignal.interactable = state;
    }
}
