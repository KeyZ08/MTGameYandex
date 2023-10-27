using UnityEngine;

public class LearningGame4Manager : MonoBehaviour
{
    [SerializeField] LearningHandGame5 hand;

    [SerializeField] HeroGame4 hero;
    [SerializeField] EntityGame4 enemy;

    private Animator animator;
    private TrainingBook book;
    private bool firstOpened;


    private void Start()
    {
        book = FindAnyObjectByType<TrainingBook>();
    }

    private void Update()
    {
        if (firstOpened)
            return;

        if (!book.isActiveAndEnabled)
        {
            firstOpened = true;
            animator = GetComponent<Animator>();
            animator.SetTrigger("StartLearning");
        }
    }

    public void GameStop()
    {
        Time.timeScale = 0;
    }

    public void GameStart()
    {
        Time.timeScale = 1.0f;
        animator.SetTrigger("EndLearning");
    }

    public void HeroAttack()
    {
        hero.AttackAnim();
        enemy.Death();
    }
}
