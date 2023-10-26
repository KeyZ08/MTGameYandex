using UnityEngine;
using UnityEngine.UI;

public abstract class ShopItemView : MonoBehaviour
{
    protected ShopItem _item;
    [SerializeField] protected Image _image;

    public virtual ShopItem Item 
    { 
        get { return _item; } 
        protected set
        {
            _item = value;
            _image.sprite = _item.Image;
        }
    }

    public virtual void SetItem(ShopItem item)
    {
        Item = item;
    }
}
