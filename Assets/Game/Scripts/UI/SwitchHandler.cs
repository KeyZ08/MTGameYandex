using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using static UnityEngine.UI.Button;

public class SwitchHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private RectTransform toggleIndicator;
    [SerializeField]
    private Image background;

    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color offColor;
    [SerializeField]
    private float tweenTime = 0.2f;

    public ButtonClickedEvent onClick;

    private bool _isOn;
    public bool IsOn
    {
        get { return _isOn; }
    }

    private float offX;
    private float onX;

    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;

    private void Start()
    {
        offX = -background.rectTransform.rect.width / 2 + toggleIndicator.rect.width / 2 - 2;
        onX = background.rectTransform.rect.width / 2 - toggleIndicator.rect.width / 2 + 2;
    }

    private void OnEnable()
    {
        Toggle(_isOn);
    }

    private void Toggle(bool value)
    {
        if (onX == offX) { Start(); }
        _isOn = value;

        ToggleColor(_isOn);
        MoveIndicator(_isOn);

        if (valueChanged != null)
            valueChanged(_isOn);
    }

    private void ToggleColor(bool value)
    {
        if (value)
            background.DOColor(onColor, tweenTime).SetUpdate(true);
        else
            background.DOColor(offColor, tweenTime).SetUpdate(true);
    }

    private void MoveIndicator(bool value)
    {
        if (value)
            toggleIndicator.DOAnchorPosX(onX, tweenTime).SetUpdate(true);
        else
            toggleIndicator.DOAnchorPosX(offX, tweenTime).SetUpdate(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick.Invoke();
        Toggle(!_isOn);
    }

    public void SetValue(bool value)
    {
        Toggle(value);
    }
}