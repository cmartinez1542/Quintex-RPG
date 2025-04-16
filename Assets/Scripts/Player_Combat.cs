using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Combat : MonoBehaviour
{
    public Animator anim;
    public bool attackState;
    public AudioManager audiomanager;
    public float knockbackForce = 1f;
    public float attackRange = 1f;
    public int stunTime = 1;
    public int damage = 1;

    public void Attack()
    {   
        knockbackForce = 3f;
        attackRange = 1f;
        anim.SetBool("isAttacking", true);
        attackState = anim.GetBool("isAttacking");
        Debug.Log("Attack Started: isAttacking set to " + attackState);

        //audiomanager.PlayAttackSound();


            //}
       // }
    }

    public void DealDamage()
    {
        Debug.Log($"[{gameObject.name}] Checking for targets in range {attackRange}");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, stunTime);

        Debug.Log($"[{gameObject.name}] Detected {hits.Length} colliders");

        foreach (var hit in hits)
        {
            Debug.Log($"[{gameObject.name}] Hit: {hit.name}, Tag: {hit.tag}");

            if (hit.gameObject != gameObject && hit.CompareTag("Player"))
            {
                Debug.Log($"[{gameObject.name}] Target is a valid Player! Applying knockback.");

                PlayerMovement2 otherPlayer = hit.GetComponent<PlayerMovement2>();
                if (otherPlayer != null)
                {
                    otherPlayer.Knockback(transform, knockbackForce,stunTime);
                    hit.GetComponent<Player_Health>().ChangeHealth(-damage);
                    Debug.Log($"[{gameObject.name}] Knockback called on {hit.name}");
                }
                else
                {
                    Debug.LogWarning($"[{gameObject.name}] Hit object {hit.name} has no PlayerMovement2!");
                }


  
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void FinishAttacking()
    {   
        anim.SetBool("isAttacking", false);
        attackState = anim.GetBool("isAttacking");
        Debug.Log("First attack done, STATE: " + attackState);
    }

    public void SmashAttack()
{    
    knockbackForce = 5f;
    attackRange = 2f;  
    anim.SetBool("Attack2", true);
    attackState = anim.GetBool("Attack2");
    Debug.Log($"{gameObject.name} used Rock Smash!");

    // anim.SetTrigger("RockSmash"); only if you use a different animation trigger

    // Call your magic/aoe/damage logic here (like area hitbox, spawn rock effect, etc.)
}

    public void FinishSmashAttack()
    {
        anim.SetBool("Attack2", false);

        // Check if the animation is closing correctly
        attackState = anim.GetBool("Attack2");
        Debug.Log("Second Attack Ended: Continue_Attacking set to FALSE, Current Value: " + attackState);
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