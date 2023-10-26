using TMPro;
using UnityEngine;
using System;
using DG.Tweening;

public class StatisticRow : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshProUGUI Date;
    public TextMeshProUGUI RightAll;
    public TextMeshProUGUI IncorrectAll;
    public TextMeshProUGUI All;

    public StatisticAdditionalRow[] AdditionalRows;

    private RectTransform _rt;
    private bool _showAdditional = false;


    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
        if (AdditionalRows.Length != 11)
            throw new ArgumentNullException("Должно быть ровно 11 элементов");
    }

    public void SetValues(DateTime date, int[] right, int[] incorrect, int rightCount, int incorrectCount)
    {
        Date.text = date.ToShortDateString();
        for (int i = 0; i < AdditionalRows.Length; i++) 
        {
            AdditionalRows[i].SetValues(right[i], incorrect[i]);
        }
        RightAll.text = rightCount.ToString();
        IncorrectAll.text = incorrectCount.ToString();
        All.text = (rightCount + incorrectCount).ToString();
    }

    public void ShowOrHideAdditional()
    {
        if (_showAdditional) HideAdditional();
        else ShowAdditional();
    }

    private void ShowAdditional()
    {
        _showAdditional = true;
        DOTweenModuleUI.DOSizeDelta(_rt, new Vector2(1700, 1255), 1);
    }

    private void HideAdditional()
    {
        _showAdditional = false;
        DOTweenModuleUI.DOSizeDelta(_rt, new Vector2(1700, 100), 1);
    }
}
