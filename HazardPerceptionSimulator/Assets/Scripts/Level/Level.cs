using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipType { Message, Card }

public class Level : MonoBehaviour
{
    [Header("Level params")]
    [SerializeField] protected GameObject gameCanvas;
    [SerializeField] protected GameObject cardsCanvas;
    [SerializeField] private List<CardInfo> startCardsInfo;

    private UICardsController cardsController;

    protected virtual void Awake()
    {
        cardsController = cardsCanvas.GetComponentInChildren<UICardsController>();
        if (startCardsInfo.Count != 0)
            ShowCardsInfo(startCardsInfo);

        EventManager.Instance.ShowTipCardsInfo += ShowCardsInfo;
        EventManager.Instance.ShowTipMessage += ShowTipMessage;
    }

    public void ShowCardsInfo(List<CardInfo> cardsInfo)
    {
        cardsController.Initialize(cardsInfo, () => UnfreezeGame());
        ChangeTimeScale(0f);
        gameCanvas.SetActive(false);
        cardsCanvas.SetActive(true);
    }

    public void ShowTipMessage(string message)
    {

    }

    private void UnfreezeGame()
    {
        gameCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

    private void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }
}
