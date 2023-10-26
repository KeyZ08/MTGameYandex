using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LearningTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTarget;
    [SerializeField] TextMeshProUGUI textCopy;

    void Update()
    {
        textCopy.text = textTarget.text;
    }
}
