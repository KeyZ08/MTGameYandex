using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierGame3 : MultiplierBase
{
    public List<Transform> figuresCoords;
    public GameObject unknownFigurePrefab;
    public GameObject figurePrefab;
    public FigureDial dial;

    protected override void Awake(){}

    /// <summary>
    /// если int = int.MinValue => числа нет
    /// </summary>
    public override void Create(int a, int b, int result, int choicesCount)
    {
        if (choicesCount != 1) 
            throw new Exception("Вариантов выбора должно быть ровно 1. Это уровень с Сундуком.");
        var n = ChoiceGenerator(a, b, result, choicesCount)[0];
        if (n >= 10 || n < 0) throw new Exception("Не решаемый пример!");
        var nullCount = 0;
        foreach (var i in new[] { a, b, result })
            if (i == int.MinValue) nullCount += 1;
        if (nullCount != 1) throw new Exception("Неверно составлен пример. Должно быть ровно одно число равное int.MinValue");


        if (a != int.MinValue)
        {
            _figure1 = Instantiate(figurePrefab, figuresCoords[0].position, transform.rotation, this.transform).GetComponent<FigureBase>();
            _figure1.Number = a;
        }
        else
        {
            Instantiate(unknownFigurePrefab, figuresCoords[0].position, transform.rotation, this.transform);
            _figure1 = dial;
        }

        if (b != int.MinValue)
        {
            _figure2 = Instantiate(figurePrefab, figuresCoords[1].position, transform.rotation, this.transform).GetComponent<FigureBase>();
            _figure2.Number = b;
        }
        else
        {
            Instantiate(unknownFigurePrefab, figuresCoords[1].position, transform.rotation, this.transform);
            _figure2 = dial;
        }

        if (result != int.MinValue)
        {
            _result = Instantiate(figurePrefab, figuresCoords[2].position, transform.rotation, this.transform).GetComponent<FigureBase>();
            _result.Number = result;
        }
        else
        {
            Instantiate(unknownFigurePrefab, figuresCoords[2].position, transform.rotation, this.transform);
            _result = dial;
        }
    }

    public void Stop()
    {
        dial.Stop();
    }

    public override void Decided()
    {
        throw new NotImplementedException();
    }

    internal override List<FigureBase> CreateChoices(List<int> choices)
    {
        throw new NotImplementedException();
    }

    public override void AddFigure(FigureBase f)
    {
        throw new NotImplementedException();
    }

    public override void RemoveFigure(FigureBase f)
    {
        throw new NotImplementedException();
    }

    public override void Mistaken()
    {
        throw new NotImplementedException();
    }
}

