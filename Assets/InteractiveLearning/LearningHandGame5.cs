using System;
using System.Collections;
using UnityEngine;

public class LearningHandGame5 : MonoBehaviour
{
    private Animator animator;

    private Transform start;
    private Transform end;

    private Transform clickTarget;

    private Transform _tr;

    private bool isWait;

    private bool isDown;
    private bool isUp;
    private bool isClick;

    private bool IsDown
    {
        get { return isDown; }
        set 
        { 
            isDown = value;
            if (isDown) animator.SetTrigger("Down");
            animator.SetBool("DownBool", value);
        }
    }

    private bool IsUp
    {
        get { return isUp; }
        set
        {
            isUp = value;
            if (isUp) animator.SetTrigger("Up");
        }
    }

    private bool IsClick
    {
        get { return isClick; }
        set
        {
            if (value) { animator.SetTrigger("Click"); }
            isClick = value;
            animator.SetBool("ClickBool", value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _tr = GetComponent<Transform>();
        Hide();
    }

    public void SetClick(Transform target)
    {
        if (target == null)
        {
            clickTarget = null;
            IsClick = false;
            Hide();
            return;
        }

        StopAll();
        clickTarget = target;
        _tr.position = (Vector2)clickTarget.position;
        Show();
        IsClick = true;
    }

    public void SetDrag(Transform start, Transform end)
    {
        if (start == null || end == null)
        {
            this.start = null;
            this.end = null;
            Hide();
            return;
        }

        StopAll();
        Show();
        _tr.position = (Vector2)start.position;
        this.start = start;
        this.end = end;
        IsDown = true;
        StartCoroutine(WaitCoroutine(0.2f));
    }

    public void StopAll()
    {
        isWait = false;
        IsDown = false;
        IsClick = false;
        IsUp = false;
        SetDrag(null, null);
        SetClick(null);
    }

    private void Update()
    {
        if (isWait) return;

        if (start != null && end != null)
        {
            if((Vector2)_tr.position != (Vector2)end.position)
            {
                _tr.position = Vector2.MoveTowards((Vector2)_tr.position, (Vector2)end.position, Time.deltaTime);
            }
            else
            {
                SetDrag(start, end);
            }
        }
    }

    IEnumerator WaitCoroutine(float seconds, Action next = null)
    {
        isWait = true;
        yield return new WaitForSeconds(seconds);
        isWait = false;
        if (next != null) next.Invoke();
        yield return null;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
