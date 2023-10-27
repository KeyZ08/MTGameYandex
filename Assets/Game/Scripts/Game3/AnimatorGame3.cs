using System.Collections;
using TMPro;
using UnityEngine;

public class AnimatorGame3 : MonoBehaviour
{
    public Animator chest;

    public AudioSource codeFalls;

    public void Victory()
    {
        chest.SetTrigger("victory");
        codeFalls.PlayDelayed(3);
        StartCoroutine(Win());
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(6f);
        FindAnyObjectByType<MainManager>().Win();
    }
}
