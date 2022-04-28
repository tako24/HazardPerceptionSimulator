using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class TutorialLevel : Level
{
    [Header("Tutorial level params")]
    [SerializeField] private GameObject infoCanvas;
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject step1Panel;
    [SerializeField] private GameObject step2Panel;
    [SerializeField] private GameObject step3Panel;
    [SerializeField] private EventTrigger increaseSpeedButton;
    [SerializeField] private EventTrigger decreaseSpeedButton;
    [SerializeField] private EventTrigger leftTurnButton;
    [SerializeField] private EventTrigger rightTurnButton;

    private UnityAction<BaseEventData> buttonListener;

    protected override void Awake()
    {
        //gameCanvas.SetActive(false);
        //cardsCanvas.SetActive(false);
        //infoCanvas.SetActive(true);

        increaseSpeedButton.gameObject.SetActive(false);
        decreaseSpeedButton.gameObject.SetActive(false);
        leftTurnButton.gameObject.SetActive(false);
        rightTurnButton.gameObject.SetActive(false);
        gameCanvas.SetActive(true);
        base.Awake();
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
            default:
                break;
        }
    }

    private void EnableStep1()
    {
        Time.timeScale = 0f;
        buttonListener = (e) => DisableStep1();
        increaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.AddListener(buttonListener);
        tutorialCanvas.SetActive(true);
        step1Panel.SetActive(true);
        increaseSpeedButton.gameObject.SetActive(true);
    }

    private void DisableStep1()
    {
        increaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.RemoveListener(buttonListener);
        tutorialCanvas.SetActive(false);
        step1Panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void EnableStep2()
    {
        Time.timeScale = 0f;
        buttonListener = (e) => DisableStep2();
        decreaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.AddListener(buttonListener);
        tutorialCanvas.SetActive(true);
        step2Panel.SetActive(true);
        decreaseSpeedButton.gameObject.SetActive(true);
    }

    private void DisableStep2()
    {
        decreaseSpeedButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.RemoveListener(buttonListener);
        tutorialCanvas.SetActive(false);
        step2Panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void EnableStep3()
    {
        Time.timeScale = 0f;
        buttonListener = (e) => DisableStep3();
        rightTurnButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.AddListener(buttonListener);
        tutorialCanvas.SetActive(true);
        step3Panel.SetActive(true);
        leftTurnButton.gameObject.SetActive(true);
        rightTurnButton.gameObject.SetActive(true);
    }

    private void DisableStep3()
    {
        rightTurnButton.triggers.Where(trigger => trigger.eventID == EventTriggerType.PointerDown).First().callback.RemoveListener(buttonListener);
        tutorialCanvas.SetActive(false);
        step3Panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
