using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelController : MonoBehaviour
{
    [SerializeField] private UILevelInfo uiCard;
    [SerializeField] private List<LevelInfo> levelInfos;

    private int currentLevel;

    public void SetCurrentLevel(int index)
    {
        if (levelInfos.Count < index || index < 0)
        {
            Debug.LogError($"index = {index} \n LevelInfos count = {levelInfos.Count}");
            return;
        }

        currentLevel = index;
        RefreshContent();
    }

    private void Start()
    {
        uiCard.LevelInfo = levelInfos[0];
    }

    public void StartLevel()
    {
        // запуск уровня
    }
    public void RefreshContent()
    {
        uiCard.LevelInfo = levelInfos[currentLevel];
    }
}
