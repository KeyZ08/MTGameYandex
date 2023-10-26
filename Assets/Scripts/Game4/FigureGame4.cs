using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class FigureGame4 : FigureBase
{
    [NonSerialized]
    public Button button;
    private bool _isMovable;

    public bool isMovable 
    {
        get { return _isMovable; } 
        set 
        { 
            _isMovable = value;
        } 
    }

    internal override void Awake()
    {
        base.Awake();

        var defaultButton = ColorBlock.defaultColorBlock;
        defaultButton.disabledColor = Color.white;
        button = this.AddComponent<Button>();
        button.colors = defaultButton;
        button.onClick.AddListener(
            () => FindAnyObjectByType<MultiplierGame4>().FigureMove(this));
        _isMovable = true;
    }

    private void Update()
    {
        if (_isMovable) 
        {
            var pos = (Vector2)transform.position;
            transform.position = Vector2.MoveTowards(pos, pos + Vector2.down, 1f * Time.deltaTime);
        }
    }

    public void SetPosition(Vector2 newPos)
    {
        transform.position = newPos;
    }
}
