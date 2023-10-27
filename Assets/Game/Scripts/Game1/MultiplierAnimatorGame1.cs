using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierAnimatorGame1 : MonoBehaviour
{
    public GameObject[] icons;
    private TextMeshProUGUI[] textIcons; 

    public Material IncorrectMaterial;
    public Material CorrectMaterial;
    public Material DefaultMaterial;

    public AudioSource ExampleTrue;
    public AudioSource ExampleFail;

    private MultiplierGame1 _multiplier;
    private Animator _anim;


    private void Awake()
    {
        _multiplier = GetComponent<MultiplierGame1>();
        _anim = GetComponent<Animator>();
        textIcons = new TextMeshProUGUI[icons.Length];
        for (var i = 0; i < textIcons.Length; i++)
            textIcons[i] = icons[i].GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetFiguresInteractable(bool interactable)
    {
        foreach (var figure in _multiplier.GetChoiceFigures())
            (figure as FigureGame1).SetInteractable(interactable);
    }

    public void SetDefaultView()
    {
        SetMaterialForFigures(_multiplier.GetExampleFigures(), DefaultMaterial);
        for (var i = 0; i < textIcons.Length; i++)
            SetMaterial(textIcons[i], DefaultMaterial);
        SetMaterialForFigures(_multiplier.GetChoiceFigures(), DefaultMaterial);
    }

    public void ShowMistake()
    {
        var figures = _multiplier.GetExampleFigures();
        for (var i = 0; i < figures.Length; i++)
        {
            if ((figures[i] as FigureGame1).isMovable)
            {
                SetMaterial(figures[i]._text, IncorrectMaterial);
            }
        }
    }

    public void SetCorrectView()
    {
        SetMaterialForFigures(_multiplier.GetExampleFigures(), CorrectMaterial);
        for (var i = 0; i < textIcons.Length; i++)
            SetMaterial(textIcons[i], CorrectMaterial);
    }
    
    public void SetIncorrectView()
    {
        SetMaterialForFigures(_multiplier.GetExampleFigures(), IncorrectMaterial);
        for (var i = 0; i < textIcons.Length; i++)
            SetMaterial(textIcons[i], IncorrectMaterial);
        _anim.SetTrigger("Incorrect");
    }

    public void ExampleUpdate()
    {
        _anim.SetTrigger("ExampleUpdate");
    }

    private void SetMaterial(TextMeshProUGUI text, Material material)
    {
        text.fontMaterial = material;
    }

    private void SetMaterialForFigures(IEnumerable<FigureBase> figures, Material material)
    {
        foreach (var f in figures)
        {
            if (f != null)
                SetMaterial(f._text, material);
        }
    }

    public void MoveFigure(FigureGame1 f, Transform endPos, Transform choices)
    {
        StartCoroutine(MoveCoroutine(f, endPos, choices));
    }

    public void TPFigure(FigureGame1 f, Transform endPos, Transform choices)
    {
        var tr = f.transform;
        tr.position = endPos.position;
        if (endPos != f.startPosition)
            f.transform.SetParent(transform);
        else
            f.transform.SetParent(choices);
    }

    public void MoveAndShowMistakesOrRight(FigureGame1 f, Transform choices)
    {
        StartCoroutine(MoveAndShowMistakesOrRightCoroutine(f, choices));
    }

    public IEnumerator MoveCoroutine(FigureGame1 f, Transform endPos, Transform choices)
    {
        choices.GetComponent<HorizontalLayoutGroup>().enabled = false;
        var tr = f.transform;
        SetFiguresInteractable(false);
        while ((Vector2)tr.position != (Vector2)endPos.position)
        {
            tr.position = Vector2.MoveTowards((Vector2)tr.position, (Vector2)endPos.position, Time.deltaTime * 20);
            yield return null;
        }

        if (endPos != f.startPosition) 
            f.transform.SetParent(transform);
        else 
            f.transform.SetParent(choices);

        SetFiguresInteractable(true);
    }

    private IEnumerator MoveAndShowMistakesOrRightCoroutine(FigureGame1 f, Transform choices)
    {
        yield return MoveCoroutine(f, _multiplier.GetPostition(f), choices);
        if (_multiplier.CheckNotNull())
        {
            SetFiguresInteractable(false);
            if (_multiplier.IsValid())
            {
                SetCorrectView();
                //Debug.Log("Решено правильно!");

                ExampleTrue.Play();
                ExampleUpdate();
                _multiplier.Decided();
                yield return new WaitForSeconds(1f);
            }
            else
            {

                ExampleFail.Play();
                SetIncorrectView();
                yield return new WaitForSeconds(0.5f);
                SetDefaultView();
                ShowMistake();
                _multiplier.Mistaken();
                _multiplier.RemoveFigure(f);
                //Debug.Log("Неверно!");
            }
            SetFiguresInteractable(true);
        }
    }
}
