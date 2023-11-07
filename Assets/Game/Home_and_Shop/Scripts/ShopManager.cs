using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityJSON;
using YG;
using YG.Example;
using JSON = UnityJSON.JSON;

public class ShopManager : MonoBehaviour
{
    [Header("Items")]
    public ShopItem[] ShopItems;
    public GameObject ShopItemPrefabMagazin;
    public GameObject ShopItemPrefabKarman;

    [Header("Default Items")]
    public ShopItem[] DefaultItems;

    [Header("Displays")]
    public PayDisplay PayDisplay;
    public KarmanItemDisplay KarmanItemDisplay;

    [Header("Locations")]
    public Transform Home;
    public Transform Magazin;
    public Transform Karman;

    [Header("Money")]
    public TextMeshProUGUI Money;

    [Header("Sounds")]
    public AudioSource btnClickSound;

    private Dictionary<ShopItem, GameObject> placedObjects;
    private Dictionary<ShopItem, GameObject> magazinObjects;
    private Dictionary<ShopItem, GameObject> karmanObjects;

    private void Awake()
    {
        //YandexGame.savesData.Shop = null;
        //YandexGame.SaveProgress();
        //MoneyManager.Add(300);

        placedObjects = new Dictionary<ShopItem, GameObject>();
        magazinObjects = new Dictionary<ShopItem, GameObject>();
        karmanObjects = new Dictionary<ShopItem, GameObject>();

        ApplySave(GetSave());
        for (int i = 0; i < ShopItems.Length; i++)
        {
            ShopItemView prefab = ShopItems[i].IsBuy ? KarmanItemCreate() : MagazinItemCreate();
            prefab.SetItem(ShopItems[i]);
            if (prefab is KarmanItemView)
                karmanObjects.Add(prefab.Item, prefab.gameObject);
            else if(prefab is MagazinItemView)
                magazinObjects.Add(prefab.Item, prefab.gameObject);

            if (ShopItems[i].IsInstall && ShopItems[i].IsBuy)
                InstallToHome(prefab.Item, true);
        }

        Money.text = MoneyManager.Money.ToString();
    }

    private ShopItemView KarmanItemCreate()
    {
        KarmanItemView obj = Instantiate(ShopItemPrefabKarman, Karman).GetComponent<KarmanItemView>();
        obj.Button.onClick.AddListener(() =>
            {
                btnClickSound.Play();
                KarmanItemDisplay.SetItem(obj.Item);
                KarmanItemDisplay.gameObject.SetActive(true);

                KarmanItemDisplay.UnBuy.onClick.RemoveAllListeners();
                KarmanItemDisplay.UnBuy.onClick.AddListener(() => { UnBuy(obj.Item); KarmanItemDisplay.gameObject.SetActive(false); });
            });
        return obj;
    }

    private void KarmanItemRemove(ShopItem item)
    {
        if (karmanObjects.TryGetValue(item, out var obj))
        {
            if(placedObjects.TryGetValue(item, out var pObj)) 
            {
                item.SetInstall(false);
                Destroy(pObj);
                placedObjects.Remove(item);
            }
            item.SetBuy(false);
            karmanObjects.Remove(item);
            Destroy(obj);
        }
    }

    private ShopItemView MagazinItemCreate()
    {
        MagazinItemView obj = Instantiate(ShopItemPrefabMagazin, Magazin).GetComponent<MagazinItemView>();
        obj.Button.onClick.AddListener(() =>
            {
                btnClickSound.Play();
                PayDisplay.SetItem(obj.Item);
                PayDisplay.gameObject.SetActive(true);
                PayDisplay.Pay.onClick.RemoveAllListeners();
                if (!obj.Item.ForAds)
                    PayDisplay.Pay.onClick.AddListener(() => { Buy(obj.Item); PayDisplay.gameObject.SetActive(false); });
                else
                    PayDisplay.Pay.onClick.AddListener(() => { BuyForAds(obj.Item); PayDisplay.gameObject.SetActive(false); });
            });
        return obj;
    }

    private void MagazinItemRemove(ShopItem item)
    {
        if (magazinObjects.TryGetValue(item, out var obj))
        {
            item.SetBuy(true);
            magazinObjects.Remove(item);
            Destroy(obj);
        }
    }

    private void HomeItemCreate(ShopItem item)
    {
        if(placedObjects.ContainsKey(item)) return;
        var placed = Instantiate(item.Prefab, Home);
        placedObjects.Add(item, placed);
    }

    private void HomeItemDelete(ShopItem item)
    {
        item.SetInstall(false);
        Destroy(placedObjects[item]);
        placedObjects.Remove(item);
    }

    private void InstallToHome(ShopItem item, bool value)
    {
        item.SetInstall(value);
        if (value)
        {
            var objs = FindPlacedItemByType(item.Type);
            for (int i = 0; i < objs.Length; i++)
                HomeItemDelete(objs[i]);
            HomeItemCreate(item);
        }
        else
        {
            HomeItemDelete(item);
        }
    }

    public void ItemInstallToHome(ShopItem item, bool value)
    {
        InstallToHome(item, value);
        Save();
    }

    public ShopItem[] FindPlacedItemByType(ShopItemType type)
    {
        var keys = placedObjects.Keys.Where(x=> x.Type == type).ToArray();
        return keys;
    }

    public void Buy(ShopItem item)
    {
        if (item.Price <= MoneyManager.Money)
        {
            MoneyManager.Remove(item.Price);
            Money.text = MoneyManager.Money.ToString();

            MagazinItemRemove(item);
            var kObj = KarmanItemCreate();
            kObj.SetItem(item);
            karmanObjects.Add(item, kObj.gameObject);
        }
        Save();
    }

    public void BuyForAds(ShopItem item)
    {
        var ads = FindAnyObjectByType<AdsInitializer>().GetComponent<RewardedAds>();
        ads.ShowAd();
        StartCoroutine(ads.WaitAdsCoroutine(
            () => 
            {
                MagazinItemRemove(item);
                var kObj = KarmanItemCreate();
                kObj.SetItem(item);
                karmanObjects.Add(item, kObj.gameObject);
            },
            () =>
            {
                Debug.Log($"ќшибка, {item.Name} не даем тк c рекламой что-то не так");
            }
            ));
    }

    public void UnBuy(ShopItem item)
    {
        MoneyManager.Add(item.Price);
        Money.text = MoneyManager.Money.ToString();

        KarmanItemRemove(item);
        var mObj = MagazinItemCreate();
        mObj.SetItem(item);
        magazinObjects.Add(item, mObj.gameObject);
        Save();
    }

    private void Save()
    {
        var result = new Dictionary<string, (bool isBuy, bool isInstall, bool isDefault)>();
        for (int i = 0; i < ShopItems.Length; i++) 
        {
            var item = ShopItems[i];
            result.Add(item.WorkingName, (item.IsBuy, item.IsInstall, item.IsDefault));
        }

        var json = JSON.Serialize(result);
        YandexGame.savesData.Shop = json;
        YandexGame.SaveProgress();
    }

    private Dictionary<string, (bool isBuy, bool isInstall, bool isDefault)> GetSave()
    {
        var value = YandexGame.savesData.Shop;
        if (value == null || value == "")
        {
            var result = new Dictionary<string, (bool isBuy, bool isInstall, bool isDefault)>();
            for (int i = 0; i < DefaultItems.Length; i++)
            {
                var item = DefaultItems[i];
                item.SetBuy(true);
                item.SetInstall(true);
                item.SetDefault(true);
                result.Add(item.WorkingName, (true, true, true));
            }
            YandexGame.savesData.Shop = JSON.Serialize(result);
            YandexGame.SaveProgress();
            return result;
        }
        else
        {
            return JSON.Deserialize<Dictionary<string, (bool isBuy, bool isInstall, bool isDefault)>>(value);
        }
    }

    private void ApplySave(Dictionary<string, (bool isBuy, bool isInstall, bool isDefault)> save)
    {
        for (int i = 0; i < ShopItems.Length; i++)
        {
            if (save.TryGetValue(ShopItems[i].WorkingName, out var tuple))
            {
                ShopItems[i].SetBuy(tuple.isBuy);
                ShopItems[i].SetInstall(tuple.isInstall);
                ShopItems[i].SetDefault(tuple.isDefault);
            }
            else
            {
                ShopItems[i].SetBuy(false);
                ShopItems[i].SetInstall(false);
                ShopItems[i].SetDefault(false);
            }
        }
    }
}
