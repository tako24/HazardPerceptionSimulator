using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    public void UpdateInfoCanvas(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }
}
