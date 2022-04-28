using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    [SerializeField] private TipType tipType = TipType.Card;
    [SerializeField] private List<CardInfo> cardsInfo;
    [SerializeField] private string message;

    private void Awake()
    {
        if (tipType == TipType.Message)
        {
            if (message.Length == 0)
                throw new System.ArgumentException("Tip message cannot be empty");
        }
        else if (tipType == TipType.Card)
        {
            if (cardsInfo == null || cardsInfo.Count == 0)
                throw new System.ArgumentException("Tip card info cannot be empty");
        }
    }

    public void Implement()
    {
        switch (tipType)
        {
            case TipType.Card:
                EventManager.Instance.ShowTipCardsInfo.Invoke(cardsInfo);
                break;
            case TipType.Message:
                EventManager.Instance.ShowTipMessage.Invoke(message);
                break;
            default:
                Debug.LogWarning("This type of tip is not described");
                break;
        }
        Destroy(gameObject);
    }
}
