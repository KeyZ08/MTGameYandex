using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGame5 : MonoBehaviour
{
    public Button ResetBtn;

    private List<int> choices;
    private (int bag1, int bag2) Bags;
    private MultiplierGame5 multiplier;

    private void Awake()
    {
        multiplier = FindAnyObjectByType<MultiplierGame5>();
        ResetBtn.onClick.AddListener(() => multiplier.ResetExample());
    }
    public void SetParams(List<int> choices, (int bag1, int bag2) bags)
    {
        this.choices = choices;
        Bags = bags;
    }

    public void Start()
    {
        multiplier.Create(choices, Bags);
    }

    public void CorrectExample()
    {
        FindAnyObjectByType<LevelUIs>().Win();
    }
}
