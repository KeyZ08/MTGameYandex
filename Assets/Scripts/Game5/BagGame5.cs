using System;
using TMPro;
using UnityEngine;

public class BagGame5 : MonoBehaviour
{
    public TextMeshProUGUI textRatio;
    public TextMeshProUGUI textResult;

    public AudioSource moneySound;

    private int _ratio;
    private int _result;

    public int Ratio
    {
        get { return _ratio; } 
        private set
        {
            if (_ratio < 0) throw new ArgumentException();
            _ratio = value;
            textRatio.text = $"x{_ratio}";
        }
    }

    public void SetRatio(int ratio)
    {
        Ratio = ratio;
    }

    public int Result
    {
        get { return _result; }
        private set
        {
            _result = value;
            textResult.text = $"{_result}";
        }
    }

    public void Add(FigureGame5 f)
    {
        Result += f.Number * Ratio;
        moneySound.Play();
    }

    public void Drop()
    {
        Result = 0;
    }
}
