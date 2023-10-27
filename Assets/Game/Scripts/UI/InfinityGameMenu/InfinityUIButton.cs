using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class InfinityUIButton : MonoBehaviour
{
    public InfinityGameMode gameMode;
    protected ColorBlock unactiveColor;
    protected ColorBlock activeColor;
    protected bool _selected = true;
    protected Button btn;

    public virtual void Click()
    {
        Selected = !Selected;
        gameMode.StateUpdate(this);
    }


    public virtual bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            if (_selected)
                btn.colors = activeColor;
            else
                btn.colors = unactiveColor;
        }
    }

    protected virtual void Awake()
    {
        btn = GetComponent<Button>();
        var colorBlock = btn.colors;
        colorBlock.selectedColor = colorBlock.normalColor;
        colorBlock.highlightedColor = colorBlock.normalColor;
        activeColor = colorBlock;

        colorBlock.normalColor = colorBlock.disabledColor;
        colorBlock.selectedColor = colorBlock.disabledColor;
        colorBlock.highlightedColor = colorBlock.disabledColor;
        unactiveColor = colorBlock;

        btn.onClick.AddListener(Click);
    }
}
