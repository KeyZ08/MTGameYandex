using UnityEngine;
using UnityEngine.UI;

public class MagazinItemView : ShopItemView
{
    [SerializeField] protected Text _price;
    [SerializeField] protected Image _ads;
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

    public override void SetItem(ShopItem item)
    {
        base.SetItem(item);
        if (item.ForAds)
        {
            _ads.gameObject.SetActive(true);
        }
    }
}
