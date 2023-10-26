using TMPro;
using UnityEngine;

public class LearningBagGame5 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetResult;
    [SerializeField] TextMeshProUGUI targetRatio;

    [SerializeField] TextMeshProUGUI result;
    [SerializeField] TextMeshProUGUI ratio;

    private void Update()
    {
        result.text = targetResult.text;
        ratio.text = targetRatio.text;
    }
}
