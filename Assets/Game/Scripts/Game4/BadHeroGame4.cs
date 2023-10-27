using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class BadHeroGame4 : MonoBehaviour
{
    public Transform LeftTop;
    public Transform RightBottom;
    public Transform center;
    public float AttackMoveSpeed = 6;
    public float IdleMoveSpeed = 0.3f;
    public ManagerGame4 manager;

    private Animator _anim;
    private SortingGroup _row;
    private readonly Vector2 defTargetAttack = new Vector2(int.MinValue, int.MinValue);
    private Vector2 targetAttack;
    private Vector2 targetIdle;
    private Queue<Vector2> targetsAttack;

    public bool IsAnimated { get; private set; }
    public bool IsMoved { get; private set; }


    public int RowPos
    {
        get { return _row.sortingOrder; }
        set { _row.sortingOrder = value; }
    }

    protected void Awake()
    {
        _anim = GetComponent<Animator>();
        _row = GetComponent<SortingGroup>();
        targetAttack = defTargetAttack;
        targetsAttack = new Queue<Vector2>();
        SetRandomTargetIdle();
    }

    private void Update()
    {
        if (IsAnimated) return;
        if (targetAttack == defTargetAttack && targetsAttack.Count > 0)
            SetTargetWithOffset(targetsAttack.Peek(), ref targetAttack);

        if (targetAttack != defTargetAttack) //если не default значит есть цель
        {
            if (targetAttack == (Vector2)transform.position) //если мы достигли цели
            {
                _anim.SetTrigger("attack");
                //тут также происходит анимация и спавн монстра
                //благодаря триггеру на анимации
                StartCoroutine(Attack());
                SetRandomTargetIdle();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetAttack, AttackMoveSpeed * Time.deltaTime);
            RowPos = (int)((LeftTop.position.y - center.position.y) / 9 * 100 + 201);
        }
        else
        {
            if (targetIdle == (Vector2)transform.position)
                SetRandomTargetIdle();
            transform.position = Vector2.MoveTowards(transform.position, targetIdle, IdleMoveSpeed * Time.deltaTime);
            RowPos = (int)((LeftTop.position.y - center.position.y) / 9 * 100 + 201);
        }
    }

    private void SetTargetWithOffset(Vector2 target, ref Vector2 targetVector)
    {
        IsMoved = true;
        Vector2 offsetPos = transform.position - center.position;
        targetVector = target + offsetPos;
    }

    public void SetTargetAttack(Vector2 target)
    {
        targetsAttack.Enqueue(target);
    }

    public void SetRandomTargetIdle()
    {
        var ltPos = (Vector2)LeftTop.position;
        var rbPos = (Vector2)RightBottom.position;
        var target = new Vector2(Random.Range(ltPos.x, rbPos.x), Random.Range(rbPos.y, ltPos.y));
        SetTargetWithOffset(target, ref targetIdle);
    }

    IEnumerator Attack()
    {
        IsAnimated = true;
        yield return new WaitForSeconds(1.4f);
        IsAnimated = false;
        targetsAttack.Dequeue();
        targetAttack = defTargetAttack;
        IsMoved = false;
        if (targetsAttack.Count > 0)
            SetTargetWithOffset(targetsAttack.Peek(), ref targetAttack);
    }

    //триггер для анимации
    //монстр спавнится только в конкретный момент анимации
    public void EnemySpawn()
    {
        var target = targetsAttack.Peek();
        manager.BadHeroAttack(target);
    }
}
