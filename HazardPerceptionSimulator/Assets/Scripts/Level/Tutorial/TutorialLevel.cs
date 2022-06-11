using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using System;

public class TutorialLevel : Level
{
    [Header("Tutorial level params")]
    [SerializeField] private InfoCanvas infoCanvas;
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject step1Panel;
    [SerializeField] private GameObject step2Panel;
    [SerializeField] private GameObject step3Panel;
    [SerializeField] private EventTrigger increaseSpeedButton;
    [SerializeField] private EventTrigger decreaseSpeedButton;
    [SerializeField] private EventTrigger leftTurnButton;
    [SerializeField] private EventTrigger rightTurnButton;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private TrafficLightController trafficLightControllerStep2;
    [SerializeField] private TrafficLightController trafficLightControllerStep3;

    private UnityAction<BaseEventData> buttonListener;

    protected override void OnEnable()
    {
        base.OnEnable();
        EnableStep1();
    }

    public void EnableStepById(int stepId)
    {
        switch (stepId)
        {
            case 2:
                EnableStep2();
                break;
            case 3:
                EnableStep3();
                break;
            case 4:
                EnableStep4();
                break;
            default:
                break;
        }
    }

    private void EnableStep1()
    {
        increaseSpeedButton.gameObject.SetActive(false);
        decreaseSpeedButton.gameObject.SetActive(false);
        leftTurnButton.gameObject.SetActive(false);
        rightTurnButton.gameObject.SetActive(false);
        gameCanvas.SetActive(true);
        cardsCanvas.SetActive(false);

        Time.timeScale = 0f;
        buttonListener = (e) => DisableStep1();
        increaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.AddListener(buttonListener);
        step1Panel.SetActive(true);
        tutorialCanvas.SetActive(true);
        infoCanvas.gameObject.SetActive(true);
        increaseSpeedButton.gameObject.SetActive(true);
    }

    private void DisableStep1()
    {
        increaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.RemoveListener(buttonListener);
        tutorialCanvas.SetActive(false);
        infoCanvas.gameObject.SetActive(false);
        step1Panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void EnableStep2()
    {
        Time.timeScale = 0f;
        buttonListener = (e) => DisableStep2();
        decreaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.AddListener(buttonListener);
        step2Panel.SetActive(true);
        tutorialCanvas.SetActive(true);
        infoCanvas.UpdateInfoCanvas("Снижение скорости", "Впереди перекресток с красным сигналом светофора.\nДля снижения скорости зажмите кнопку - снизить скорость");
        infoCanvas.gameObject.SetActive(true);
        decreaseSpeedButton.gameObject.SetActive(true);
    }

    private void DisableStep2()
    {
        decreaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.RemoveListener(buttonListener);
        tutorialCanvas.SetActive(false);
        infoCanvas.gameObject.SetActive(false);
        step2Panel.SetActive(false);
        Time.timeScale = 1f;
        IDisposable playerStop = null;
        playerStop = Observable.EveryUpdate()
            .SkipWhile(w => playerController.Speed > 0.5f)
            .Finally(() =>
            {
                Observable.TimerFrame(5).Subscribe(s => trafficLightControllerStep2.enabled = false);
            })
            .Subscribe(s =>
            {
                trafficLightControllerStep2.enabled = true;
                playerStop?.Dispose();
            });
    }

    private void EnableStep3()
    {
        Time.timeScale = 0f;
        buttonListener = (e) => DisableStep3();
        rightTurnButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.AddListener(buttonListener);
        tutorialCanvas.SetActive(true);
        step3Panel.SetActive(true);
        infoCanvas.UpdateInfoCanvas("Поворот", "Для поворота на перекрестке нажмите на стрелочку");
        infoCanvas.gameObject.SetActive(true);
        leftTurnButton.gameObject.SetActive(true);
        rightTurnButton.gameObject.SetActive(true);
    }

    private void DisableStep3()
    {
        rightTurnButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.RemoveListener(buttonListener);
        tutorialCanvas.SetActive(false);
        infoCanvas.gameObject.SetActive(false);
        step3Panel.SetActive(false);
        Time.timeScale = 1f;
        IDisposable playerStop = null;
        playerStop = Observable.EveryUpdate()
            .SkipWhile(w => playerController.Speed > 0.5f)
            .Finally(() =>
            {
                Observable.TimerFrame(5).Subscribe(s => trafficLightControllerStep3.enabled = false);
            })
            .Subscribe(s =>
            {
                trafficLightControllerStep3.enabled = true;
                playerStop?.Dispose();
            });
    }

    private void EnableStep4()
    {
        Time.timeScale = 0f;
        gameCanvas.SetActive(false);
        infoCanvas.UpdateInfoCanvas("Конец обучения", "Поздравляю! Ты успешно прошёл обучение");
        infoCanvas.gameObject.SetActive(true);
    }
}
