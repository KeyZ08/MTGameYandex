using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class MultiplierGame1 : MultiplierBase
{
    public GameObject figurePrefab;
    public ManagerGame1 manager;
    public Transform[] coordsPosition;

    private FigureBase ChoiceInExample;//отмечаем что мы поместили в пример в качестве ответа
    private FigureGame1[] ExampleFigures;//фигуры используемые в самом примере
    private MultiplierAnimatorGame1 _exampleAnimator;

    public override void AddFigure(FigureBase f)
    {
        var figure = f as FigureGame1;
        if (figure.startPosition == null)
        {
            var obj = new GameObject("default_pos");
            obj.transform.position = f.transform.position;
            figure.startPosition = obj.transform;
        }

        if (_figure1 == null) { _figure1 = figure; ChoiceInExample = figure; }
        else if (_figure2 == null) { _figure2 = figure; ChoiceInExample = figure; }
        else { _result = figure; ChoiceInExample = figure; };
    }

    protected override void Awake()
    {
        base.Awake();
        _exampleAnimator = GetComponent<MultiplierAnimatorGame1>();

        _figuresChoice = new List<FigureBase>();
        for (int i = 0; i < 4; i++)
        {
            var f = Instantiate(figurePrefab, this.choices).GetComponent<FigureGame1>();
            _figuresChoice.Add(f);
        }

        ExampleFigures = new FigureGame1[3];
        ExampleFigures[0] = Instantiate(figurePrefab, coordsPosition[0].position, Quaternion.identity, this.transform).GetComponent<FigureGame1>();
        ExampleFigures[1] = Instantiate(figurePrefab, coordsPosition[1].position, Quaternion.identity, this.transform).GetComponent<FigureGame1>();
        ExampleFigures[2] = Instantiate(figurePrefab, coordsPosition[2].position, Quaternion.identity, this.transform).GetComponent<FigureGame1>();
    }

    /// <summary>
    /// если int = int.MinValue => числа нет
    /// </summary>
    public override void Create(int a, int b, int result, int choicesCount)
    {
        // иногда меняем местами искомые числа (для бесконечной игры когда выбрали 1-2 числа чтобы не только эти числа выбирать в качестве решений)
        if ((a == int.MinValue || b == int.MinValue) && UnityEngine.Random.Range(0, 2) == 0)
        {
            if (a == int.MinValue)
            {
                a = result / b;
                b = int.MinValue;
            }
            else
            {
                b = result / a;
                a = int.MinValue;
            }
        }
        CreateChoices(ChoiceGenerator(a, b, result, choicesCount));

        if (ChoiceInExample != null)
        {
            var figure = ChoiceInExample as FigureGame1;
            if (figure == _figure1)
                _figure1 = null;
            else if (figure == _figure2)
                _figure2 = null;
            else _result = null;
            _exampleAnimator.TPFigure(figure, figure.startPosition, choices);
            ChoiceInExample = null;
        }

        _figure1 = ExampleFigures[0];
        _figure2 = ExampleFigures[1];
        _result = ExampleFigures[2];
        CreateExample(a, b, result);

        _exampleAnimator.SetDefaultView();
    }

    private void CreateExample(int a, int b, int result)
    {
        var example = GetExampleFigures();
        var exampleInt = new[] {a, b, result};
        for (var i = 0; i < exampleInt.Length; i++)
        {
            if (exampleInt[i] != int.MinValue)
            {
                example[i].Number = exampleInt[i];
                example[i].gameObject.SetActive(true);
            }
            else
            {
                example[i].gameObject.SetActive(false);

                var figure = example[i] as FigureGame1;
                if (figure == _figure1)
                    _figure1 = null;
                else if (figure == _figure2)
                    _figure2 = null;
                else _result = null;
            }
        }
    }

    internal override List<FigureBase> CreateChoices(List<int> choices)
    {
        if (_figuresChoice.Count != choices.Count) throw new ArgumentOutOfRangeException("choices");

        for (int i = 0; i < choices.Count; i++)
        {
            var f = _figuresChoice[i] as FigureGame1;
            f.Number = choices[i];
            f.isMovable = true;
        }

        return _figuresChoice;
    }

    public override void RemoveFigure(FigureBase f)
    {
        var figure = f as FigureGame1;
        if (figure == _figure1)
            _figure1 = null;
        else if (figure == _figure2)
            _figure2 = null;
        else _result = null;
        _exampleAnimator.MoveFigure(figure, figure.startPosition, choices);
    }

    public void FigureMove(FigureBase f)
    {
        if (InMultiplier(f))
        {
            _exampleAnimator.SetDefaultView();
            RemoveFigure(f);
        }
        else if (CheckNotNull())
        {
           // Debug.Log("Нет свободных мест");
        }
        else
        {
            AddFigure(f);
            _exampleAnimator.MoveAndShowMistakesOrRight(f as FigureGame1, choices);
        }
    }

    //триггер для анимации
    public void NextMultiplier()
    {
        manager.ResetMultiplier();
    }

    public override void Decided()
    {
        int[] nums = GetExampleFigures().Select(x => x._number).Take(2).ToArray();
        manager.CorrectExample(nums);
    }

    public override void Mistaken()
    {
        int[] nums = GetExampleFigures().Select(x => x._number).Take(2).ToArray();
        manager.MistakeExample(nums);
    }

    /// <summary>
    /// Transform места где Figure должно находиться в примере
    /// </summary>
    public Transform GetPostition(FigureBase f)
    {
        if (f == _figure1) return coordsPosition[0];
        else if (f == _figure2) return coordsPosition[1];
        else return coordsPosition[2];
    }

}
