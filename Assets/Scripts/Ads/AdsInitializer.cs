using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidGameID = "5362663";
    [SerializeField] string iOSGameID = "5362662";
    [SerializeField] bool testMode = true;
    private string gameID;

    private void Awake()
    {
        var obj = GameObject.FindGameObjectsWithTag("Ads");
        if (obj.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        gameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSGameID : androidGameID;
        Advertisement.Initialize(gameID, testMode, this);
        DontDestroyOnLoad(gameObject);
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Инициализация прошла успешно.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Ошибка инициализации: {error.ToString()} - {message}");
    }
}
