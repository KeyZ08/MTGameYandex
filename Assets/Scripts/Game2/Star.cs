using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public AudioSource starSound;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        _anim.SetTrigger("end");
        starSound.Play();
    }
}
