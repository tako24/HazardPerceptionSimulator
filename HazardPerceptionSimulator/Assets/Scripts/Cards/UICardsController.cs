using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardsController : MonoBehaviour
{
    [SerializeField] private UICardInfo uiCard;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button previousButton;

    private List<CardInfo> cardsInfo;
    private int cardIndex = 0;
    private Action callback;

    public void Initialize(List<CardInfo> cardsInfo, Action callback)
    {
        cardIndex = 0;
        this.cardsInfo = cardsInfo;
        this.callback = callback;
        uiCard.UpdateCardInfo(cardsInfo[cardIndex]);

        bool isHigher = cardsInfo.Count > 1;

        previousButton.gameObject.SetActive(isHigher);
        nextButton.gameObject.SetActive(isHigher);
        previousButton.interactable = false;
        nextButton.interactable = isHigher;
        closeButton.interactable = !isHigher;
    }

    public void NextCard()
    {        
        cardIndex++;

        if (cardIndex == cardsInfo.Count - 1)
        {
            nextButton.interactable = false;
            closeButton.interactable = true;
        }

        if (previousButton.interactable == false)
            previousButton.interactable = true;

        uiCard.UpdateCardInfo(cardsInfo[cardIndex]);
    }

    public void PrevCard()
    {
        cardIndex--;

        if (cardIndex == 0)
            previousButton.interactable = false;

        if (nextButton.interactable == false)
            nextButton.interactable = true;

        uiCard.UpdateCardInfo(cardsInfo[cardIndex]);
    }

    public void CloseCardsPanel()
    {
        transform.parent.gameObject.SetActive(false);
        callback.Invoke();
        callback = null;
    }
}
