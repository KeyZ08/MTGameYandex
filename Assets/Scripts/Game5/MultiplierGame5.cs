using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MultiplierGame5 : MultiplierBase
{
    public BagGame5 bag1;
    public BagGame5 bag2;

    private ManagerGame5 manager;
    private MultiplierAnimatorGame5 _animator;
    private List<FigureGame5> _selectedFigures;
    private bool _isDecided;

    protected override void Awake()
    {
        manager = FindAnyObjectByType<ManagerGame5>();
        _animator = GetComponent<MultiplierAnimatorGame5>();
        _selectedFigures = new List<FigureGame5>();
    }

    public void FigureEndDrag(FigureGame5 figure)
    {
        var hit = Physics2D.RaycastAll(figure.transform.position, Vector2.zero)
            .Where(x => x.transform.TryGetComponent<BagGame5>(out var bag));
        if (hit.Count() == 0) return;
        var bag = hit.First().transform.GetComponent<BagGame5>();
        AddFigure(figure, bag);
    }

    public void AddFigure(FigureGame5 f, BagGame5 target)
    {
        target.Add(f);
        f.gameObject.SetActive(false);
        _selectedFigures.Add(f);
        _figuresChoice.Remove(f);
        if (bag1.Result == bag2.Result && _figuresChoice.Count == 0)//использованы все монеты и сумма в мешках одинакова
            _animator.Victory();
        else if(_figuresChoice.Count == 0 && bag1.Result != bag2.Result)
        {
            Mistaken();
        }
    }

    public void Create(List<int> choices, (int bag1, int bag2) bags)
    {
        _figuresChoice = FindObjectsByType<FigureBase>(FindObjectsSortMode.None).ToList();
        if (_figuresChoice.Count != choices.Count) throw new Exception("Ќесоответствие количества монет и значений");
        var bufferRandom = choices.Select(x=>x).ToList();
        for(var i = 0; i < _figuresChoice.Count; i++)
        {
            var num = Random.Range(0, bufferRandom.Count);
            _figuresChoice[i].Number = bufferRandom[num];
            bufferRandom.RemoveAt(num);
        }
        bag1.SetRatio(bags.bag1);
        bag2.SetRatio(bags.bag2);
    }

    public void ResetExample()
    {
        if (_isDecided) return;
        bag1.Drop();
        bag2.Drop();
        for(var i = _selectedFigures.Count - 1; i >= 0; i--)
        {
            _selectedFigures[i].gameObject.SetActive(true);
            _figuresChoice.Add(_selectedFigures[i]);
            _selectedFigures.RemoveAt(i);
        }
    }

    public override void Decided()
    {
        _isDecided = true;
        StartCoroutine(Complete());
    }

    IEnumerator Complete()
    {
        yield return new WaitForSeconds(3f);
        manager.CorrectExample();
    }

    internal override List<FigureBase> CreateChoices(List<int> choices)
    {
        throw new NotImplementedException();
    }

    public override void RemoveFigure(FigureBase f)
    {
        throw new NotImplementedException();
    }

    public override void Create(int a, int b, int result, int choicesCount)
    {
        throw new NotImplementedException();
    }

    public override void AddFigure(FigureBase f)
    {
        throw new NotImplementedException();
    }

    public override void Mistaken()
    {
        OnMistaken.Invoke();
    }

    public Action OnMistaken = () => { };
}

