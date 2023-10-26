using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class FigureGame1 : FigureBase
{
    private Button _button;
    private bool _isMovable;

    public Sprite[] BackgroundVariants;
    public Transform startPosition;

    public bool isMovable 
    {
        get { return _isMovable; } 
        set 
        { 
            _isMovable = value;
            if (_isMovable) _button.interactable = true;
            else _button.interactable = false;
        } 
    }

    internal override void Awake()
    {
        base.Awake();

        var defaultButton = ColorBlock.defaultColorBlock;
        defaultButton.disabledColor = Color.white;

        if(BackgroundVariants.Length != 0)
        {
            GetComponent<Image>().sprite = BackgroundVariants[Random.Range(0, BackgroundVariants.Length)];
        }

        _button = this.AddComponent<Button>();
        _button.colors = defaultButton;
        _button.interactable = false;
        _button.onClick.AddListener(
            () => FindAnyObjectByType<MultiplierGame1>().FigureMove(this));
    }

    public void SetInteractable(bool IsInteractable)
    {
        _button.interactable = IsInteractable;
    }

    private void OnDestroy()
    {
        if(startPosition != null)
            Destroy(startPosition.gameObject);
    }
}
