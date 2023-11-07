using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayDisplay : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private Image money;
    [SerializeField] private Image ads;

    public Button Pay;

    public void SetItem(ShopItem item)
    {
        Price.text = item.Price.ToString();
        Image.sprite = item.Image;
        Pay.interactable = item.Price <= MoneyManager.Money;

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
    }
}
