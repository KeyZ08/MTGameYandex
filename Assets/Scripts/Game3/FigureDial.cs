using System;
using UnityEngine;

public class FigureDial : FigureBase
{
    
    private Rigidbody2D rb;
    public Transform BottomPosition;
    public Transform TopPosition;
    public float frictionForce = 1f;
    public float forceAmount = 100f;
    public bool IsMoved { get; private set; }
    public AudioSource flickSound;

    private int actualNumber;
    private float _dragOffsetY;
    private Vector2 topPosition;
    private Vector2 startPosition;
    private Vector2 lastMousePositon;
    private bool _isDragged;
    private bool _isStopped;

    public override int Number 
    { 
        get; 
        set; 
    }

    internal override void Awake()
    {
        //_text здесь нет
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.localPosition;
        topPosition = TopPosition.position;
    }

    private void OnMouseDown()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (BorderCheck(pos)) return;
        _isDragged = true;
        lastMousePositon = pos;
        _dragOffsetY = transform.position.y - lastMousePositon.y;
    }

    /// <summary>
    /// Если циферблат в месте нажатия перекрыт BorderForChest, то true
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private static bool BorderCheck(Vector3 pos)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(pos, Vector2.zero);
        var flag = false;
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.TryGetComponent<BorderForChest>(out var border))
            {
                flag = true;
                break;
            }
        }

        return flag;
    }

    private void OnMouseDrag()
    {
        if (_isStopped) return;
        if (!_isDragged) return;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastMousePositon = pos;
        rb.velocity = new Vector2(0, lastMousePositon.y + _dragOffsetY - transform.position.y) * Time.fixedDeltaTime * forceAmount;
    }

    private void OnMouseUp()
    {
        _isDragged = false;
    }

    private void FixedUpdate()
    {
        if (_isStopped) return;

        var step = (TopPosition.position.y - BottomPosition.position.y) / 10f;
        var num = (int)Math.Round((TopPosition.position.y - topPosition.y) / step);
        var newNum = num < 10 ? num : 0;
        if (newNum != actualNumber)
        {
            actualNumber = newNum;
            flickSound.Play();
        }

        //торможение
        if (rb.velocity.magnitude > 0.01f)
        {
            IsMoved = true;
            Number = int.MinValue;
        }
        else
        {
            IsMoved = false;
            Number = num < 10 ? num : 0;
        }
        
        rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, frictionForce * Time.fixedDeltaTime);
    }
    
    private void Update()
    {
        if (_isStopped) return;
        if (topPosition.y > TopPosition.position.y)
        {
            transform.localPosition = -startPosition - new Vector2(0, topPosition.y - TopPosition.position.y);
            _dragOffsetY = transform.position.y + lastMousePositon.y;
        }
        else if (topPosition.y < BottomPosition.position.y)
        {
            transform.localPosition = startPosition + new Vector2(0, BottomPosition.position.y - topPosition.y);
            _dragOffsetY = transform.position.y - lastMousePositon.y;
        }
    }

    public void Stop()
    {
        _isStopped = true;
        rb.velocity = Vector2.zero;
        enabled = false;
    }
}
