using UnityEngine;

public class Demon2 : EntityGame4
{
    public float animTimeMove = 2f;

    public AudioSource step;
    public AudioSource death;

    private bool _isMoved;
    private Vector2 _target;
    private float _distance;

    private void Start()
    {
        type = EnemyesGame4.Demon2;
    }

    protected void Update()
    {
        if (_isMoved)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target, _distance / animTimeMove * Time.deltaTime);
            if (_target == (Vector2)transform.position)
            {
                _isMoved = false;
                _anim.SetBool("move", false);
            }
        }
    }

    public override void MoveTo(Vector2 target)
    {
        Vector2 offsetPos = transform.position - center.position;
        _anim.SetBool("move", true);
        _isMoved = true;
        _target = target + offsetPos;
        _distance = Vector2.Distance(transform.position, _target);
        _anim.SetFloat("speed", _distance / 2);
    }
    public override void Death()
    {
        _anim.SetTrigger("death");
        death.Play();
    }

    public void StepSound()
    {
        step.Play();
    }
}
