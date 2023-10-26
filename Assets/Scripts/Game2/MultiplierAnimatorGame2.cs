using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class MultiplierAnimatorGame2 : MonoBehaviour
{
    private MultiplierGame2 _multiplier;

    public Animator hero;
    public Animator girl;
    public Animator angryMan;

    public AudioSource girl_sad;
    public AudioSource girl_happy;

    private void Awake()
    {
        _multiplier = GetComponent<MultiplierGame2>();
    }

    public void Victory()
    {
        girl_sad.Stop();
        girl_happy.PlayDelayed(1);
        var choices = _multiplier.GetChoiceFigures();
        foreach (var choice in choices) 
        {
            choice.GetComponent<Animator>().SetTrigger("burst");
            choice.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        girl.SetBool("win", true);
        angryMan.SetBool("win", true);
        hero.SetBool("win", true);
        StartCoroutine(Complete());
    }

    public void Lose(FigureBase figure)
    {
        figure.GetComponent<Animator>().SetTrigger("burst");
        figure.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    IEnumerator Complete()
    {
        yield return new WaitForSeconds(3f);
        _multiplier.Decided();
    }
}
