using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardInfo", menuName = "Items/Create new CardInfo")]
public class CardInfo : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Sprite signIcon;
    public string Title => title;
    public string Description => description;
    public Sprite SignIcon => signIcon;
}
