using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatisticsController : MonoBehaviour
{
    [SerializeField] private GameObject mistakeTPMPrefab;

    private List<TextMeshProUGUI> mistakes;
    void Start()
    {
        
        mistakes = new List<TextMeshProUGUI>();
        mistakes = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        RefreshMistakesUI();
        
    }

    private void RefreshMistakesUI()
    {
        try
        {
            mistakes = new List<TextMeshProUGUI>();
            var savedMistakes = JsonLoader.GetInstance().Load();

            foreach (var mistake in savedMistakes)
            {
                AddNote(mistake);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void AddNote(Mistake mistake)
    {
        var mistakeTMP =  Instantiate(mistakeTPMPrefab, this.transform).GetComponentInChildren<TextMeshProUGUI>();
        if(mistakeTMP == null) Debug.Log("TMP is null");
        mistakeTMP.text = mistake.Description;
        mistakes.Add( mistakeTMP);
    }

}
