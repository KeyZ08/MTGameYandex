using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class MultiplierBase : MonoBehaviour
{
    public Transform choices;

    internal FigureBase _figure1;
    internal FigureBase _figure2;
    internal FigureBase _result;
    internal List<FigureBase> _figuresChoice;

    protected virtual void Awake()
    {
        _figuresChoice = new List<FigureBase>();
    }

    public bool IsValid()
        => Multiply() == _result.Number;

    public int Multiply()
    {
        if (!(_figure1 != null && _figure2 != null))
            throw new ArgumentNullException();
        return _figure1.Number * _figure2.Number;
    }

    public bool InMultiplier(FigureBase f)
        => f == _figure1 || f == _figure2 || f == _result;

    public bool CheckNotNull()
        => _figure1 != null && _figure2 != null && _result != null;

    public FigureBase[] GetExampleFigures()
    {
        return new[] { _figure1, _figure2, _result };
    }

    public List<FigureBase> GetChoiceFigures()
    {
        return _figuresChoice;
    }

    public abstract void Decided();

    public abstract void Mistaken();

    /// <summary>
    /// если int = int.MinValue => числа нет
    /// </summary>
    public abstract void Create(int a, int b, int result, int choicesCount);

    /// <summary>
    /// Генерирует набор чисел на основе имеющегося примера.
    /// Хотябы одно число из примера должно быть int.MinValue (считается как null)
    /// Рассчитано только для одного пустого числа
    /// </summary>
    internal List<int> ChoiceGenerator(int a, int b, int r, int choicesCount)
    {
        if (a != int.MinValue && b != int.MinValue && r != int.MinValue)
            throw new ArgumentException("Хотябы одно число из примера должно быть int.MinValue");

        var result = new List<int>();
        int n = int.MinValue;//пропущенное число
        bool NullInResult = false;
        if (r != int.MinValue)
        {
            int z = (a == int.MinValue ? b : a);
            n = z == 0 ? Random.Range(0, 11) : r / z;
            result.Add(n);
            if (a == int.MinValue) a = n;
            else b = n;
        }
        else
        {
            NullInResult = true;
            n = a * b;
            result.Add(n);
            r = n;
        }

        if (a * b != r)
            throw new ArgumentException("Что-то не сходится, возможно в предполагаемом примере есть не целые числа");

        while (result.Count != choicesCount)
        {
            if (NullInResult)
            {
                var first = r - 15 < 0 ? 0 : r - 15;
                var last = r + 10 > 100 ? 100 : r + 10;
                var rand = Random.Range(first, last + 1);
                if (!result.Contains(rand))
                    result.Add(rand);
            }
            else
            {
                var rand = Random.Range(0, 11);
                if (!result.Contains(rand))
                    result.Add(rand);
            }
        }

        //перемешиваем варианты ответа (достаточно переместить правильный ответ)
        var random = Random.Range(0, result.Count);
        (result[0], result[random]) = (result[random], result[0]);

        return result;
    }

    internal abstract List<FigureBase> CreateChoices(List<int> choices);

    public abstract void AddFigure(FigureBase f);

    public abstract void RemoveFigure(FigureBase f);
}
