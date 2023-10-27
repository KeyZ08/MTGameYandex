using System;
using UnityEngine;
using UnityEngine.UI;

public class LearningGame5Manager : MonoBehaviour
{
    [SerializeField] LearningHandGame5 hand;
    MultiplierGame5 multiplier;
    Animator animator;

    [Header("Reset")]
    public Transform resetBtn;

    [Header("Step1")]
    public Transform start1;
    public Transform end1;

    private bool dragStart;

    private void Start()
    {
        animator = GetComponent<Animator>();
        multiplier = FindAnyObjectByType<MultiplierGame5>();
        multiplier.OnMistaken += LearningResetStart;
        var reset = resetBtn.GetComponent<Button>();
        reset.onClick.AddListener(LearningResetEnd);

        LearningDragStart();
    }

    private void Update()
    {
        if(dragStart && Input.anyKeyDown)
        {
            LearningDragEnd();
        }
    }

    public void LearningResetStart()
    {
        hand.SetClick(resetBtn);
        animator.SetTrigger("ResetStart");
    }

    public void LearningResetEnd()
    {
        hand.SetClick(null);
        animator.SetTrigger("ResetEnd");
    }

    public void LearningDragStart()
    {
        hand.SetDrag(start1, end1);
        animator.SetTrigger("DragStart");
        dragStart = true;
    }

    public void LearningDragEnd()
    {
        hand.SetDrag(null, null);
        animator.SetTrigger("DragEnd");
        dragStart = false;
    }
}
