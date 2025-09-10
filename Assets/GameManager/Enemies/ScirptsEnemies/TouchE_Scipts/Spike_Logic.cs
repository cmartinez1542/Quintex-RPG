using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Logic : MonoBehaviour
{
    private bool isKnockedBack = false;

    [Header("Attack Settings")]
    public float attackCooldown = 1f;
    private bool isAttacking = false;

    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange = 1f;
    public float knockbackForce = 4f;
    public float stunTime = 1f;

    public bool missDashing = false;

    private Animator anim;
    private Rigidbody2D rb;

    private bool isTriggered = false; // To prevent multiple coroutine calls

    public AudioManager audiomanager;

    [Header("Follow Player Settings")]
    public Transform player;
    public float detectionRange = 5f;
    public float stopDistance = 1.5f;
    public float moveSpeed = 2f;

    [Header("Stun Settinngs")]
    public SpriteRenderer spriteRenderer; // Drag your enemy's SpriteRenderer in Inspector

public Vector3 attackPointOffset = new Vector3(1.5f, 0f, 0f);

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if(!missDashing) {
            anim.SetBool("AlwaysOut",true);
            anim.SetBool("PlayAnimation",true);
        }
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
                player = GetClosestPlayer(players).transform;
        }
    }

    public void FlashWhiteBlink(float totalDuration, float blinkSpeed = 10.5f)
    {
        StartCoroutine(BlinkCoroutine(totalDuration, blinkSpeed));
    }

    private IEnumerator BlinkCoroutine(float totalDuration, float blinkSpeed)
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < totalDuration)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkSpeed);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);

            elapsed += blinkSpeed * 2;
        }

        spriteRenderer.color = originalColor; // ensure it resets
    }



    private GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = p;
            }
        }

        return closest;
    }

 private void FixedUpdate()
    {
        float speed = rb.linearVelocity.magnitude;
        anim.SetFloat("Speed", speed);

        if (isKnockedBack) return; //  No moverse si está siendo empujado

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= detectionRange && distance > stopDistance)
            {
                Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;
            } else if (distance <= stopDistance) {
                rb.linearVelocity = Vector2.zero;

                if (!isAttacking)
                    StartCoroutine(AttackPlayer());
            }
            else
                rb.linearVelocity = Vector2.zero;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player_Health playerHealth = collision.gameObject.GetComponent<Player_Health>();
        PlayerMovement2 playerMovement = collision.gameObject.GetComponent<PlayerMovement2>();

        if (playerHealth != null)
            playerHealth.ChangeHealth(-damage);

        if (playerMovement != null)
            playerMovement.Knockback(transform, knockbackForce, stunTime);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Health playerHealth = collision.gameObject.GetComponent<Player_Health>();
        PlayerMovement2 playerMovement = collision.gameObject.GetComponent<PlayerMovement2>();
        
        if (playerHealth != null)
            playerHealth.ChangeHealth(-damage);

        if (playerMovement != null)
            playerMovement.Knockback(transform, knockbackForce, stunTime);
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!isTriggered) {
            Player_Health playerHealth = collision.gameObject.GetComponent<Player_Health>();
            PlayerMovement2 playerMovement = collision.gameObject.GetComponent<PlayerMovement2>();
            
            if(!(missDashing && playerMovement.isDashing)) {
                if (playerHealth != null)
                    playerHealth.ChangeHealth(-damage);

                if (playerMovement != null)
                    playerMovement.Knockback(transform, knockbackForce, stunTime);
            }
            StartCoroutine(TriggerWithDelayCoroutine());
        }
    }

    private IEnumerator TriggerWithDelayCoroutine()
    {
        isTriggered = true; // Prevent re-triggering
        anim.SetBool("PlayAnimation",true);
        yield return new WaitForSeconds(attackCooldown); // Delay by attackCooldown seconds
        Debug.Log("Triggered after delay!");
        isTriggered = false; // Allow re-triggering if needed
        anim.SetBool("PlayAnimation",false);
    }

    IEnumerator AttackPlayer() {
        isAttacking = true;

        while (true) {
            // Verificar si el jugador aún está dentro del rango de ataque
            if (player == null || Vector2.Distance(transform.position, player.position) > stopDistance) {
                isAttacking = false;
                yield break; // Salir de la corrutina si el jugador está fuera de rango
            }

            yield return new WaitForSeconds(attackCooldown);
        }
    }


    public void ApplyAttackDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position, weaponRange, LayerMask.GetMask("Player"));

        Debug.Log($"🎯 Jugadores detectados: {hits.Length}");

        foreach (Collider2D hit in hits)
        {
            Player_Health playerHealth = hit.GetComponent<Player_Health>();
            PlayerMovement2 playerMovement = hit.GetComponent<PlayerMovement2>();

            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
                Debug.Log("✅ Daño aplicado al jugador.");
            }

            if (playerMovement != null)
                playerMovement.Knockback(transform, knockbackForce, stunTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
        }
        
    }

    private void UpdateAttackPointDirection()
{
    if (player == null || attackPoint == null) return;

    Vector2 direction = (player.position - transform.position).normalized;

    // Decidir dirección dominante (horizontal o vertical)
    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
    {
        // Ataque horizontal (izquierda o derecha)
        attackPoint.localPosition = new Vector3(direction.x > 0 ? 1.5f : -1.5f, 0f, 0f);

        // Volteamos sprite horizontalmente
        transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
    }
    else
    {
        // Ataque vertical (arriba o abajo)
        attackPoint.localPosition = new Vector3(0f, direction.y > 0 ? 1.5f : -1.5f, 0f);
    }
    
}
IEnumerator RecoverFromKnockback(float time)
{
    yield return new WaitForSeconds(time);
    isKnockedBack = false;
}


public void ApplyKnockback(Vector2 direction, float force, float duration)
{
    if (rb != null)
    {
        isKnockedBack = true;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        FlashWhiteBlink((stunTime / 2), 0.3f);
        StartCoroutine(RecoverFromKnockback(duration));
    }
}


}

