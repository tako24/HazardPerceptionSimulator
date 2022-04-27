using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMistakes : MonoBehaviour
{
    [SerializeField] private List<Mistake> mistakes = new List<Mistake>();

    private void Start()
    {
        EventManager.Instance.OnMistake += AddMistake;
    }

    private void AddMistake(Mistake mistake)
    {
        mistakes.Add(mistake);
    }

    public List<Mistake> GetMistakes() => mistakes;
}
