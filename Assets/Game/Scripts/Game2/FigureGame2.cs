using System;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class FigureGame2 : FigureBase
{
    public Vector2 startPosition;
    public Transform center;
    [NonSerialized]
    public Rigidbody2D rb;
    [NonSerialized]
    public AudioSource sound;

    private Vector2 _dragOffset;
    private bool _inTarget;
    private Transform _target;
    private MultiplierGame2 _multiplier;
    private bool _isDragged;
    

    internal override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        sound = GetComponent<AudioSource>();
        _multiplier = FindAnyObjectByType<MultiplierGame2>();
        startPosition = center.position;
    }

    private void OnMouseDown()
    {
        _isDragged = true;
        _dragOffset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        var forceAmount = 1000f;
        rb.velocity = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + _dragOffset - (Vector2)transform.position) * Time.deltaTime * forceAmount;
    }

    private void OnMouseUp()
    {
        _isDragged = false;
        _multiplier.FigureMouseUp(this);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_isDragged) return;
        if (_inTarget && center.position != _target.position)
        {
            var forceAmount = 100f;
            rb.velocity = ((Vector2)_target.position - (Vector2)center.position ) * Time.deltaTime * forceAmount;
        }
    }

    public void SetTarget(Transform t)
    {
        _target = t;
        _inTarget = true;
    }

    public void ResetTarget()
    {
        _inTarget = false;
    }

    public void ObjectDestroy()
    {
        Destroy(this.gameObject);
    }

    public void SoundPlay()
    {
        sound.Play();
    }
}
