using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UICardsController : MonoBehaviour
{
    [SerializeField] private UICard uiCard;

    [SerializeField] private List<CardInfo> cardsList;

    private int cardIndex;

    private void Start()
    {
        uiCard.Info = cardsList[0];
    }

    public void NextCard()
    {
        cardIndex++;
        if (cardIndex >= cardsList.Count)
        {
            cardIndex--;
        }
        uiCard.Info = cardsList[cardIndex];
    }

    public void PrevCard()
    {
        cardIndex--;
        if (cardIndex <0)
        {
            cardIndex++;
        }
        uiCard.Info = cardsList[cardIndex];
    }
}
