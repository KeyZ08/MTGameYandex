using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayDisplay : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Price;
    public Button Pay;

    public void SetItem(ShopItem item)
    {
        Price.text = item.Price.ToString();
        Image.sprite = item.Image;
        Pay.interactable = item.Price <= MoneyManager.Money;
    }
}
