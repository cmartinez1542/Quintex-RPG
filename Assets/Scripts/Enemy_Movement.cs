using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour     
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float stunTime = 1f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Player_Health>().ChangeHealth(-damage);
        GetComponent<PlayerMovement2>().Knockback(transform, knockbackForce, stunTime);
    }

    public void Attack()
    {
        //Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange,playerLayer);
    }

}
