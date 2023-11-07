using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KarmanItemDisplay : MonoBehaviour
{
    public Image Image;
    public GameObject PriceObject;
    public TextMeshProUGUI Price;
    public Button UnBuy;
    public Button Install;
    public Button UnInstall;

    [SerializeField] private Image money;
    [SerializeField] private Image ads;

    private ShopManager _manager;

    private void Awake()
    {
        _manager = FindAnyObjectByType<ShopManager>();
    }

    public void SetItem(ShopItem item)
    {
        Price.text = item.Price.ToString();
        Image.sprite = item.Image;
        UnBuy.interactable = !item.IsDefault && !item.ForAds;
        PriceObject.SetActive(!item.IsDefault);

        Install.gameObject.SetActive(!item.IsInstall);
        UnInstall.gameObject.SetActive(item.IsInstall);

        if (item.ForAds)
        {
            money.gameObject.SetActive(false);
            ads.gameObject.SetActive(true);
        }
        else
        {
            money.gameObject.SetActive(true);
            ads.gameObject.SetActive(false);
        }

        Install.onClick.RemoveAllListeners();
        Install.onClick.AddListener(() =>
        {
            _manager.ItemInstallToHome(item, true);
            Install.gameObject.SetActive(false);
            UnInstall.gameObject.SetActive(true);
        });


        UnInstall.onClick.RemoveAllListeners();
        UnInstall.onClick.AddListener(() =>
        {
            _manager.ItemInstallToHome(item, false);
            Install.gameObject.SetActive(true);
            UnInstall.gameObject.SetActive(false);
        });
    }
}
