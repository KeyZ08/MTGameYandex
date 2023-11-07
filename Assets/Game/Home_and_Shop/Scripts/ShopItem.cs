using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "MyObjects/Shop", order = 51)]
public class ShopItem : ScriptableObject
{
    [SerializeField] private string _publicName;
    [SerializeField] private string _workingName;
    [SerializeField] private Sprite _image;
    [SerializeField] private int _price;
    [SerializeField] private bool _forAds;
    [SerializeField] private bool _isBuy;
    [SerializeField] private bool _isInstall;
    [SerializeField] private bool _isDefault;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private ShopItemType _type;

    public ShopItemType Type { get => _type; }
    public string Name { get => _publicName; }
    public string WorkingName { get => _workingName; }
    public Sprite Image { get => _image; }
    public int Price { get => _price; }
    public bool ForAds { get => _forAds; }
    public bool IsBuy { get => _isBuy; }
    public bool IsInstall { get => _isInstall; }
    public bool IsDefault { get => _isDefault; }
    public GameObject Prefab { get => _prefab; }

    public void SetBuy(bool isBuy)
    {
        _isBuy = isBuy;
        if (!_isBuy) SetInstall(false);
    }

    public void SetInstall(bool isInstall)
    {
        _isInstall = isInstall;
    }

    public void SetDefault(bool isDefault)
    {
        _isDefault = isDefault;
    }
}

public enum ShopItemType
{
    Window,             //окно
    Armchair,           //диван
    Carpet,             //ковер
    Chandelier,         //люстра
    RightDecor,         //декорация справа
    CenterDecor,        //декорация в центре
    RightWallDecor,     //стена справа
    CenterWallDecor,    //центральная стена
    AdditionalDecor,    //мелкий доп декор 
}