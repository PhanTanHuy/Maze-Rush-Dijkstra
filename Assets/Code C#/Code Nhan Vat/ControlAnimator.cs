using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAnimator : MonoBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Move(float speed)
    {
        animator.SetFloat("VanToc", Mathf.Abs(speed));
    }
    public void PlayDash()
    {
        animator.Play("Dash");
    }
    public void PlayAttack(string attackName)
    {
        animator.Play(attackName);
    }
    public void PlayDeath()
    {
        animator.Play("Death");
    }
    public void PlayHurt()
    {
        animator.Play("Hurt");
    }
}
