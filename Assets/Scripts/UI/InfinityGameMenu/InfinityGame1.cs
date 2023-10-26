using System.Collections.Generic;
using UnityEngine;

public class InfinityGame1 : InfinityGameMode
{
    [Header("Numbers")]
    public GameObject Nums;
    public InfinityUIButton[] Numbers;

    protected override void Awake()
    {
        base.Awake();
        for(var i = 0; i < Numbers.Length; i++)
        {
            Numbers[i].Selected = false;
        }
    }

    public override bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            if (_selected)
            {
                btn.colors = activeColor;
                Nums.SetActive(true);
            }
            else
            {
                btn.colors = unactiveColor;
                Nums.SetActive(false);
            }
        }
    }

    public override void Click()
    {
        base.Click();
        if(Selected) Nums.SetActive(true);
        else Nums.SetActive(false);
        infinityGameMenu.StateUpdate(this);
    }

    public override void StateUpdate(InfinityUIButton button)
    {
        for (var i = 0; i < Numbers.Length; i++)
        {
            if (Numbers[i].Selected) 
            { 
                IsValid = true;
                infinityGameMenu.StateUpdate(this);
                return;
            }
        }
        IsValid = false;
        infinityGameMenu.StateUpdate(this);
    }

    public int[] GetSettings()
    {
        var usedNums = new List<int>();
        for (var i = 0; i < Numbers.Length; i++)
        {
            if (Numbers[i].Selected)
            {
                usedNums.Add(i);
            }
        }
        return usedNums.ToArray();
    }
}
