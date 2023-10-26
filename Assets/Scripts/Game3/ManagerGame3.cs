using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGame3 : MonoBehaviour
{
    [Header("Примеры")]
    public MultiplierGame3 multiplier1;
    public MultiplierGame3 multiplier2;
    public MultiplierGame3 multiplier3;

    [Header("Предметы в сундуке")]
    public Image Image;
    public Sprite[] Sprites;

    private AnimatorGame3 _anim;
    private float timer = 0;
    private bool _isStopped;
    private (int f1, int f2, int result)[] Examples;

    public void SetParams((int f1, int f2, int result)[] examples, Ingredients obj)
    {
        if (examples == null || examples.Length != 3)
            throw new ArgumentException("Examples");
        Examples = examples;
        var sprite = Sprites[(int)obj];
        Image.sprite = sprite;
        var rectTr = Image.GetComponent<RectTransform>();
        rectTr.sizeDelta = sprite.rect.size;
    }

    public void Start()
    {
        _anim = GetComponent<AnimatorGame3>();

        multiplier1.Create(Examples[0].f1, Examples[0].f2, Examples[0].result, 1);
        multiplier2.Create(Examples[1].f1, Examples[1].f2, Examples[1].result, 1);
        multiplier3.Create(Examples[2].f1, Examples[2].f2, Examples[2].result, 1);
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0.5f && !_isStopped)
        {
            timer = 0;
            if (multiplier1.IsValid() 
                && multiplier2.IsValid()
                && multiplier3.IsValid())
            {
                CorrectExample();
                multiplier1.Stop();
                multiplier2.Stop();
                multiplier3.Stop();
                _isStopped = true;
            }
        }
    }

    public void CorrectExample()
    {
        _anim.Victory();
    }
}


public enum Ingredients
{
    Berries = 0,
    Branch = 1,
    Textbook = 2,
    Claw = 3, //коготь
}