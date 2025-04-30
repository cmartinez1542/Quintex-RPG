using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Combat : MonoBehaviour

{

    private bool canAttack = true;
public float attackCooldown = 0.8f; // Tiempo de espera entre ataques

    public Transform attackPoint;

    public Animator anim;
    public bool attackState;
    public AudioManager audiomanager;
    public float knockbackForce = 3f;
    public float attackRange = 1f;
    public float stunTime = 0.2f;
    public int damage = 1;

    public void Attack()
    {
         if (!canAttack) return; // Bloquear si aún no ha pasado el cooldown

    canAttack = false; // Bloquear ataques nuevos
    StartCoroutine(ResetAttackCooldown()); // Empezar la espera

        knockbackForce = 3f;
        attackRange = 1f;
        anim.SetBool("isAttacking", true);
        attackState = anim.GetBool("isAttacking");
        Debug.Log("Attack Started: isAttacking set to " + attackState);
        
        
    // audiomanager.PlayAttackSound();
    }

    public void DealDamage()
    {
        Debug.Log($"[{gameObject.name}] Checking for targets in range {attackRange}");

        // Detectar colisiones con enemigos usando LayerMask "Enemy"
      Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Enemy"));

        Debug.Log($"[{gameObject.name}] Detected {hits.Length} colliders");
foreach (var hit in hits)
{
    Debug.Log($"🎯 Hit: {hit.name}");

    // Aplicar daño al enemigo
    Enemy_Health enemy = hit.GetComponent<Enemy_Health>();
    if (enemy != null)
    {
        Debug.Log("💥 Enemy_Health encontrado");
        enemy.TakeDamage(damage, transform);

    }

    // Aplicar knockback si tiene Rigidbody2D
Enemy_Movement enemyMove = hit.GetComponent<Enemy_Movement>();
if (enemyMove != null)
{
    Vector2 knockDir = (hit.transform.position - transform.position).normalized;
    enemyMove.ApplyKnockback(knockDir, knockbackForce, stunTime);
    Debug.Log($"🌀 Knockback con stun aplicado a {hit.name}");
}

}

    }

void OnDrawGizmosSelected()
{
    if (attackPoint == null) return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
        // anim.SetTrigger("RockSmash"); // sólo si tienes una animación separada
    }

    public void FinishSmashAttack()
    {
        anim.SetBool("Attack2", false);
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
        attackState = anim.GetBool("Continue_Attacking");
        Debug.Log("Second Attack Ended: Continue_Attacking set to FALSE, Current Value: " + attackState);
    }
    private IEnumerator ResetAttackCooldown()
{
    yield return new WaitForSeconds(attackCooldown);
    canAttack = true; // ✅ Ya puede volver a atacar
    Debug.Log("✅ Ataque desbloqueado");
}

    
}
