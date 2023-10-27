using UnityEngine;
using UnityEngine.UI;

public class MagazinItemView : ShopItemView
{
    [SerializeField] protected Text _price;
    public Button Button;

    public override ShopItem Item
    {
        get { return _item; }
        protected set
        {
            base.Item = value;
            _price.text = _item.Price.ToString();
        }
    }
}
