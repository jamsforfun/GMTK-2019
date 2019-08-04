using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControler : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetSpeed(float p_speed)
    {
        animator.SetFloat("Speed", p_speed);
    }

    [Button]
    public void SetCarrying()
    {
        animator.SetBool("Carrying", !animator.GetBool("Carrying"));
    }

    [Button]
    public void SetSearching()
    {
        animator.SetBool("Searching", !animator.GetBool("Searching"));
    }

    [Button]
    public void SetThrow()
    {
        animator.SetBool("Throw", !animator.GetBool("Throw"));
    }

    [Button]
    public void SetSuccess()
    {
        animator.SetBool("Success", !animator.GetBool("Success"));
    }

    [Button]
    public void SetVictory()
    {
        animator.SetBool("Victory", !animator.GetBool("Victory"));
    }
}
