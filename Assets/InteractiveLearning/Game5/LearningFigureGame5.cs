using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LearningFigureGame5 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Transform target;

    FigureGame5 _targetFigure;
    Transform _tr;
    TextMeshProUGUI _text;

    private void Start()
    {
        _tr = GetComponent<Transform>();
        _targetFigure = target.GetComponent<FigureGame5>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        _tr.position = target.position;
        _text.text = _targetFigure.Number.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _targetFigure.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _targetFigure.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _targetFigure.OnEndDrag(eventData);
        
    }
}
