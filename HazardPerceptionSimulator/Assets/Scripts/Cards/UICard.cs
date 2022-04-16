using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private TextMeshProUGUI tittle;
    [SerializeField] private TextMeshProUGUI description;

    private CardInfo info;

    public CardInfo Info {
        set
        {
            info = value;
            Refresh();
        }
    }
    

    public void Refresh()
    {
        if(info==null)
            return;

        imageIcon.sprite = info.SignIcon;
        tittle.text = info.Title;
        description.text = info.Description;
    }

    public void CleanUp()
    {
        
    }
}
