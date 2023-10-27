using UnityEngine;

public class HeroGame1 : MonoBehaviour
{
    public GameObject magicCast;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            FindAnyObjectByType<LevelUIs>().Lose();
        }
    }

    public void Attack()
    {
        anim.SetTrigger("heroAttack");
    }
}
