using UnityEngine;

public class MultiplierAnimatorGame5 : MonoBehaviour
{
    private MultiplierGame5 _multiplier;
    public Animator hero;

    private void Awake()
    {
        _multiplier = GetComponent<MultiplierGame5>();
    }

    public void Victory()
    {
        hero.SetBool("happyJump", true);
        _multiplier.Decided();
    }
}
