using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private void Awake()
    {
        var obj = GameObject.FindWithTag("BackgroundMusic");
        if (obj != null && obj != gameObject)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }
}
