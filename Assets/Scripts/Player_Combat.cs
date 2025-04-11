using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Combat : MonoBehaviour
{
    public Animator anim;
    public bool attackState;
    public AudioManager audiomanager;

    public void Attack()
    {
        anim.SetBool("isAttacking", true);
        attackState = anim.GetBool("isAttacking");
        Debug.Log("Attack Started: isAttacking set to " + attackState);
        // Play the attack sound
        audiomanager.PlayAttackSound();
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
        attackState = anim.GetBool("isAttacking");
        Debug.Log("First attack done, STATE: " + attackState);

    }

    public void SecondAttack()
    {
        anim.SetBool("Continue_Attacking", true);
        attackState = anim.GetBool("Continue_Attacking");
        Debug.Log("Second Attack Ended Current Value: " + attackState);
        audiomanager.PlayAttackSound2();
    }

    public void FinishSecondAttack()
    {
        anim.SetBool("Continue_Attacking", false);

        // Check if the animation is closing correctly
        attackState = anim.GetBool("Continue_Attacking");
        Debug.Log("Second Attack Ended: Continue_Attacking set to FALSE, Current Value: " + attackState);
    }
}