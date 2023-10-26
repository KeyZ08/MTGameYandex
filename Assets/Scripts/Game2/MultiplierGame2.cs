using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplierGame2 : MultiplierBase
{
    private MultiplierAnimatorGame2 _multiplierAnimator;
    public FigureGame2 figure1;
    public FigureGame2 result;
    public ManagerGame2 manager;

    protected override void Awake()
    {
        _multiplierAnimator = GetComponent<MultiplierAnimatorGame2>();
    }

    /// <summary>
    /// если int = int.MinValue => числа нет
    /// </summary>
    public override void Create(int a, int b, int result, int choicesCount)
    {
        _figuresChoice = choices.GetComponentsInChildren<FigureBase>().ToList();

        CreateChoices(ChoiceGenerator(a, b, result, choicesCount));
        if (choicesCount != _figuresChoice.Count)
            throw new ArgumentException("Количество Шариков не совпадает с количеством вариантов выбора.");
        if (a == int.MinValue || b != int.MinValue || result == int.MinValue)
            throw new ArgumentException($"Пример не соответствует сцене! \nБыло: {a} * {b} = {result}\n Должно выглядеть: число * int.MinValue = число");

        _figure1 = figure1;
        _figure1.Number = a;
        _figure2 = null;
        _result = this.result;
        _result.Number = result;
    }

    internal override List<FigureBase> CreateChoices(List<int> choices)
    {
        for (int i = 0; i < choices.Count; i++)
        {
            _figuresChoice[i].Number = choices[i];
        }

        return _figuresChoice;
    }

    public override void AddFigure(FigureBase f)
    {
        if (_figure1 == null) { _figure1 = f; }
        else if (_figure2 == null) { _figure2 = f; }
        else _result = f;
        _figuresChoice.Remove(f);
        (f as FigureGame2).rb.gravityScale = 0;
        if (CheckNotNull())
        {
            if (IsValid())
            {
                _multiplierAnimator.Victory();
            }
            else
            {
                _multiplierAnimator.Lose(f);
            }
        }  
    }

    public override void RemoveFigure(FigureBase f)
    {
        if (f == _figure1)
            _figure1 = null;
        else if (f == _figure2)
            _figure2 = null;
        else _result = null;
        _figuresChoice.Add(f);
    }

    public override void Decided()
    {
        manager.CorrectExample();
    }

    public void FigureMouseUp(FigureGame2 figure)
    {
        var hits = Physics2D.RaycastAll(figure.center.position, Vector2.zero)
            .Where(hit => hit.transform.TryGetComponent<FigureTarget>(out var target))
            .Select(hit => hit.transform.GetComponent<FigureTarget>())
            .ToArray();
        if (hits.Length > 1) Debug.Log("Несколько FigureTarget, возможно неправильное поведение!");

        if (hits.Length == 1)
        {
            if (!CheckNotNull() && !InMultiplier(figure))
            {
                figure.SetTarget(hits[0].center);
                AddFigure(figure);
            }
            return;
        }
        else
        {
            if (InMultiplier(figure))
            {
                RemoveFigure(figure);
            }
            figure.ResetTarget();
        }
    }

    public override void Mistaken()
    {
        throw new NotImplementedException();
    }
}

