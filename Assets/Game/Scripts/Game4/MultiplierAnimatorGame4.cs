using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierAnimatorGame4 : MonoBehaviour
{
    public Animator Result;
    public List<FigureGame4> Figure1;
    public List<FigureGame4> Figure2;

    private Queue<(FigureGame4 f, Animator anim)> _figures1 = new Queue<(FigureGame4 f, Animator anim)>();
    private Queue<(FigureGame4 f, Animator anim)> _figures2 = new Queue<(FigureGame4 f, Animator anim)>();
    private MultiplierGame4 _multiplier;

    private void Start()
    {
        _multiplier = GetComponent<MultiplierGame4>();
        for (var i = 0; i < Figure1.Count; i++)
        {
            var obj = Figure1[i];
            obj.gameObject.SetActive(false);
            obj.button.interactable = false;
            _figures1.Enqueue((obj, obj.GetComponent<Animator>()));
        }
        for (var i = 0; i < Figure2.Count; i++)
        {
            var obj = Figure2[i];
            obj.gameObject.SetActive(false);
            obj.button.interactable = false;
            _figures2.Enqueue((obj, obj.GetComponent<Animator>()));
        }
    }

    private (FigureGame4 f, Animator anim) GetFigure(int place)
    {
        if (place > 1 || place < 0) throw new ArgumentException();
        if (place == 0)
        {
            return _figures1.Dequeue();
        }
        else
        {
            return _figures2.Dequeue();
        }
    }

    private void PutFigure((FigureGame4 f, Animator anim) obj, int place)
    {
        if (place > 1 || place < 0) throw new ArgumentException();
        if (place == 0)
        {
            _figures1.Enqueue((obj.f, obj.anim));
        }
        else
        {
            _figures2.Enqueue((obj.f, obj.anim)); ;
        }
    }

    public void MoveFigure(FigureGame4 f, Transform endPos, Transform choices)
    {
        StartCoroutine(MoveCoroutine(f, endPos, choices));
    }

    public IEnumerator MoveCoroutine(FigureGame4 f, Transform endPos, Transform choices)
    {
        var tr = f.transform; 
        f.isMovable = false;
        f.transform.SetParent(choices);
        while ((Vector2)tr.position != (Vector2)endPos.position)
        {
            tr.position = Vector2.MoveTowards((Vector2)tr.position, (Vector2)endPos.position, Time.deltaTime * 20);
            yield return null;
        }
    }

    public void ResetMultiplier(FigureGame4 f1, FigureGame4 f2)
    {
        StartCoroutine(Reset(f1,f2));
    }

    IEnumerator Reset(FigureGame4 f1, FigureGame4 f2)
    {
        (FigureGame4 figure1, Animator anim1) = GetFigure(0);
        (FigureGame4 figure2, Animator anim2) = GetFigure(1);
        figure1.Number = f1.Number;
        figure2.Number = f2.Number;
        f1.button.interactable = false;
        f2.button.interactable = false;

        yield return new WaitForSeconds(1f);

        figure1.gameObject.SetActive(true);
        figure2.gameObject.SetActive(true);

        f1.gameObject.SetActive(false);
        f2.gameObject.SetActive(false);
        f1.button.interactable = true;
        f2.button.interactable = true;

        Result.SetTrigger("drop");
        anim1.SetTrigger("drop");
        anim2.SetTrigger("drop");

        _multiplier.RemoveFigure(f1);
        _multiplier.RemoveFigure(f2);

        yield return new WaitForSeconds(1f);

        figure1.gameObject.SetActive(false);
        figure2.gameObject.SetActive(false);

        PutFigure((figure1, anim1), 0);
        PutFigure((figure2, anim2), 1);

        _multiplier.ResetMultiplierAccess();
    }

    public void ResetFigure(FigureGame4 f, int place)
    {
        StartCoroutine(ResetFigureCoroutine(f, place));
    }

    private IEnumerator ResetFigureCoroutine(FigureGame4 f, int place)
    {
        if (place > 1 || place < 0) throw new ArgumentException();
        (FigureGame4 figure, Animator anim) = GetFigure(place);

        figure.Number = f.Number;
        figure.gameObject.SetActive(true);
        f.gameObject.SetActive(false);

        anim.SetTrigger("drop");
        _multiplier.RemoveFigure(f);

        yield return new WaitForSeconds(1f);

        PutFigure((figure, anim), place);
        figure.gameObject.SetActive(false);
    }
}
