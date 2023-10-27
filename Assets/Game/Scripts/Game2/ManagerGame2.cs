using System;
using UnityEngine;

public class ManagerGame2 : MonoBehaviour
{
    public MultiplierGame2 multiplier;

    private (int f1, int f2, int result) example;

    public void SetParams((int f1, int f2, int result) example)
    {
        if (example.f2 == int.MinValue && example.f1 != int.MinValue && example.result != int.MinValue)
            this.example = example;
        else
        {
            throw new ArgumentException();
        }
    }

    public void Start()
    {
        multiplier.Create(example.f1, example.f2, example.result, 6);
    }

    public void CorrectExample()
    {
        FindAnyObjectByType<MainManager>().Win();
    }
}
