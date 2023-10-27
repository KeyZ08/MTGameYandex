using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapLevel : MonoBehaviour
{
    public Sprite closed;
    public Sprite opened;

    [NonSerialized]
    public Button btn;

    public bool IsOpened 
    { 
        get { return _isOpened; } 
        set
        {
            if (value)
                _img.sprite = opened;
            else
                _img.sprite = closed;
            _isOpened = value;
            _text.enabled = value;
        }
    }
    
    private bool _isOpened;
    private Image _img;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _img = GetComponent<Image>();
        btn = GetComponent<Button>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        IsOpened = false;
    }
}
