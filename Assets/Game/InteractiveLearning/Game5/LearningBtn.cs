using UnityEngine;
using UnityEngine.UI;

public class LearningBtn : MonoBehaviour
{
    [SerializeField] Button target;
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(target.onClick.Invoke);
    }
}
