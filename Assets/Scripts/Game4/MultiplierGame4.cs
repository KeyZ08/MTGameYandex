using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MultiplierGame4 : MultiplierBase
{
    public Transform Mchoices;
    public TextMeshProUGUI resultText;
    public GameObject figurePrefab;
    public ManagerGame4 manager;
    public Transform[] coordsPosition;
    public float choiceSpawnSeconds = 1f;

    public AudioSource dropletSound;

    private float _timer = 5;
    private List<FigureBase> _unactiveChoices;
    private RectTransform _rectChoicesTransform;
    private MultiplierAnimatorGame4 _animator;
    private Queue<int> selectedChoices;
    private Queue<int> numsBuffer;//нужен для того чтобы выровнять рандомизацию Choices

    protected override void Awake()
    {
        base.Awake();
        numsBuffer = new Queue<int>();
        selectedChoices = new Queue<int>();
        if (numsBuffer.Count == 0)
        {
            //заполняем числами от 1 до 10 два раза
            //for (int k = 0; k < 2; k++)
            for (int i = 1; i <= 10; i++)
                numsBuffer.Enqueue(i);
        }
        _animator = GetComponent<MultiplierAnimatorGame4>();
        _rectChoicesTransform = choices.GetComponent<RectTransform>();
    }

    private void Start()
    {
        CreateChoices(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        resultText.text = "";
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= choiceSpawnSeconds) 
        {
            if (selectedChoices.Count > 0)
            {
                ChoiceSpawn(selectedChoices.Dequeue());
            }
            else ChoiceSpawn();
            dropletSound.Play();
            _timer = 0;
        }
    }

    private void FixedUpdate()
    {
        for (var i=0; i < _figuresChoice.Count; i++)
        {
            var obj = _figuresChoice[i];
            if (obj.transform.position.y < -7f)
            {
                obj.gameObject.SetActive(false);
                _unactiveChoices.Add(obj);
                _figuresChoice.Remove(obj);
            }
        }
    }

    internal override List<FigureBase> CreateChoices(List<int> choices)
    {
        _figuresChoice = new List<FigureBase>();
        _unactiveChoices = new List<FigureBase>();
        for (var d = 0; d < 2; d++)//создаем вдвое больше объектов
        {
            for (int i = 0; i < choices.Count; i++)
            {
                var f = Instantiate(figurePrefab, this.choices).GetComponent<FigureGame4>();
                f.Number = choices[i];
                f.isMovable = true;
                f.gameObject.SetActive(false);
                _unactiveChoices.Add(f);
            }
        }

        return _unactiveChoices;
    }

    private FigureBase ChoiceSpawn()
    {        
        var num = numsBuffer.Dequeue();
        numsBuffer.Enqueue(num);

        return ChoiceSpawn(num);
    }

    private FigureBase ChoiceSpawn(int num)
    {
        if (_unactiveChoices.Count == 0) { return null; }
        var width = _rectChoicesTransform.sizeDelta.x - 1f;
        var spawnPos = new Vector2(Random.Range(transform.position.x - width / 2, transform.position.x + width / 2), choices.position.y - 0.5f);
        var figure = _unactiveChoices[_unactiveChoices.Count - 1] as FigureGame4;
        figure.SetPosition(spawnPos);
        figure.Number = num;
        figure.gameObject.SetActive(true);
        _figuresChoice.Add(figure);
        _unactiveChoices.RemoveAt(_unactiveChoices.Count - 1);
        return figure;
    }

    public override void AddFigure(FigureBase f)
    {
        var figure = f as FigureGame4;
        if (_figure1 == null)
        {
            _figure1 = figure;
            selectedChoices.Enqueue(figure.Number);
            _animator.MoveFigure(figure, coordsPosition[0], Mchoices);
        }
        else if (_figure2 == null)
        {
            _figure2 = figure;
            resultText.text = Multiply().ToString();
            selectedChoices.Enqueue(figure.Number);
            _animator.MoveFigure(figure, coordsPosition[1], Mchoices);
            Decided();
        }
    }

    public override void RemoveFigure(FigureBase figure)
    {
        var f = figure as FigureGame4;
        f.gameObject.SetActive(false);
        _unactiveChoices.Add(f);
        _figuresChoice.Remove(f);
        f.isMovable = true;
        f.transform.SetParent(choices);

        if (f == _figure1)
            _figure1 = null;
        else if (f == _figure2)
            _figure2 = null;
    }


    public void FigureMove(FigureBase f)
    {
        if (!InMultiplier(f))
            AddFigure(f);
        else
        {
            var place = 0;
            if (_figure2 == f) place = 1;
            _animator.ResetFigure(f as FigureGame4, place);
        }
    }

    public override void Decided()
    {
        manager.CorrectExample(_figure1.Number, _figure2.Number);
        _animator.ResetMultiplier(_figure1 as FigureGame4, _figure2 as FigureGame4);
    }

    public void ResetMultiplierAccess()
    {
        resultText.text = "";
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

    public override void Create(int a, int b, int result, int choicesCount)
    {
        throw new System.NotImplementedException();
    }

    public override void Mistaken()
    {
        throw new System.NotImplementedException();
    }
}