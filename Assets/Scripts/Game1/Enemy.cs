using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [NonSerialized]
    public float MovementSpeed = 1.3f;
    public Transform Center; // для нацеливания магией
    public bool isTarget;

    public GameObject[] SpriteVariants;

    private Transform _tr;
    private Rigidbody2D _rb;
    private Animator _anim;
    private ManagerGame1 _managerGame1;


    void Start()
    {
        _tr = transform;
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _managerGame1 = FindAnyObjectByType<ManagerGame1>();
        if (SpriteVariants.Length > 0)
        {
            for (var i = 0; i < SpriteVariants.Length; i++)
                SpriteVariants[i].SetActive(false);
            SpriteVariants[Random.Range(0, SpriteVariants.Length)].SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        MoveForward();    
    }

    private void MoveForward()
    {
        _rb.velocity = Vector2.left * MovementSpeed;
    }

    public void Death()
    {
        _anim.SetTrigger("Death");
        _managerGame1.MagicHitSound();
    }

    //триггер для анимации
    public void Destroy()
    {
        _managerGame1.MonsterDeath();
        Destroy(gameObject);
    }
}
