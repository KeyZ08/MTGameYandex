using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

public class FigureGame5 : FigureBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 startPosition;

    public AudioSource moneySound;

    private Vector2 _dragOffset;
    private MultiplierGame5 _multiplier;
    private bool _isDragged;
    private bool _isStatic;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragOffset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var forceAmount = 100f;
        transform.position = Vector2.MoveTowards(
            transform.position,
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + _dragOffset,
            Time.deltaTime * forceAmount);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragged = false;
        _multiplier.FigureEndDrag(this);
    }

    internal override void Awake()
    {
        base.Awake();
        _multiplier = FindAnyObjectByType<MultiplierGame5>();
        _isStatic = true;
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_isDragged) return;
        if ((Vector2)transform.localPosition != startPosition)
        {
            _isStatic = false;
            var forceAmount = 800f;
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, startPosition, Time.deltaTime * forceAmount);
        }
        else if (!_isStatic)
        {
            _isStatic = true;
            moneySound.Play();
        }
    }

    
}
