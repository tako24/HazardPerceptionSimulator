using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelInfo", menuName = "Items/Create new LevelInfo")]
public class LevelInfo : ScriptableObject
{
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private string description;
    [SerializeField] private Sprite mapImage;
    public GameObject LevelPrefab => levelPrefab;
    public string Description => description;
    public Sprite MapImage => mapImage;
}
