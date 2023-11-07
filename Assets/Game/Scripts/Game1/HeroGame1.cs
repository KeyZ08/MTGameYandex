using UnityEngine;

public class HeroGame1 : MonoBehaviour
{
    public GameObject magicCast;
    private Animator anim;
    private ManagerGame1 manager;

    private void Start()
    {
        anim = GetComponent<Animator>();
        manager = FindAnyObjectByType<ManagerGame1>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            var lose = FindAnyObjectByType<LevelUIs>().Lose(true);
            StartCoroutine(lose.WaitClickResetLoseCoroutine(() =>
            {
                manager.CorrectExample(new int[0]);
            }));
        }
    }

    public void Attack()
    {
        anim.SetTrigger("heroAttack");
    }
}
