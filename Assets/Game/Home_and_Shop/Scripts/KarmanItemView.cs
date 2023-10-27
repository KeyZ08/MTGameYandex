using UnityEngine.UI;

public class KarmanItemView : ShopItemView
{
    public Button Button;
    public bool IsInstall;
    public bool IsDefault;

    public override ShopItem Item
    {
        get { return _item; }
        protected set
        {
            base.Item = value;
            IsInstall = _item.IsInstall;
            IsDefault = _item.IsDefault;
        }
    }
}
