using System;
using TMPro;
using UnityEngine;

public abstract class FigureBase : MonoBehaviour
{
    internal int _number = 0;
    internal TextMeshProUGUI _text;

    internal virtual void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        if (_text == null) throw new Exception("Не найден TextMeshProUGUI");
        _text.text = _number.ToString();
    }

    public virtual int Number
    {
        get { return _number; }
        set
        {
            _number = value;
            _text.text = _number.ToString();
        }
    }
}
