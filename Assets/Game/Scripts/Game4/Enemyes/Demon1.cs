using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon1 : EntityGame4
{
    public float animTimeMove = 4f;
    private bool _isMoved;
    private Vector2 _target;
    private float _distance;

    public AudioSource wing;
    public AudioSource death;

    private void Start()
    {
        type = EnemyesGame4.Demon1;
    }

    protected void Update()
    {
        if (_isMoved)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target, _distance / animTimeMove * Time.deltaTime);
            if (_target == (Vector2)transform.position)
            {
                _isMoved = false;
                _anim.SetTrigger("moveEnd");
            }
        }
    }

    public override void MoveTo(Vector2 target)
    {
        Vector2 offsetPos = transform.position - center.position;
        _anim.SetTrigger("moveStart");
        _isMoved = true;
        _target = target + offsetPos;
        _distance = Vector2.Distance(transform.position, _target);
        _anim.SetFloat("speed", _distance);
    }

    public override void Death()
    {
        _anim.SetTrigger("death");
        death.Play();
    }

    public void WingSound() 
    {
        wing.Play();
    }
}
