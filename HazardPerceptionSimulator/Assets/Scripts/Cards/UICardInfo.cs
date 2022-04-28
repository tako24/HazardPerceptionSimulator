using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICardInfo : MonoBehaviour
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private TextMeshProUGUI tittle;
    [SerializeField] private TextMeshProUGUI description;

    public void UpdateCardInfo(CardInfo cardInfo)
    {
        if(cardInfo == null)
            return;

        imageIcon.sprite = cardInfo.SignIcon;
        tittle.text = cardInfo.Title;
        description.text = cardInfo.Description;
    }
}
