using TMPro;
using UnityEngine;

public class StatisticAdditionalRow : MonoBehaviour
{
    public int Number;
    public TextMeshProUGUI Right;
    public TextMeshProUGUI Incorrect;
    public TextMeshProUGUI All;

    public void SetValues(int right, int incorrect)
    {
        Right.text = right.ToString();
        Incorrect.text = incorrect.ToString();
        All.text = (right + incorrect).ToString();
    }
}
