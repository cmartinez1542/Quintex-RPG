using System.Collections;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public GameObject arrowPrefab;
    public Transform firePoint;
    public Transform player;
    public float attackCooldown = 2f;
    public float detectionRange = 10f;
    public float idealRange = 6f;
    public float tooCloseRange = 3f;
    public float moveSpeed = 2f;
    public Transform pointA;
    public Transform pointB;

    private bool isAttacking = false;
    private bool isKnockedBack = false;
    private bool movingToPointA = true;
    private float cooldownTimer;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
                player = GetClosestPlayer(players).transform;
        }
    }

void Update()
{
    if (player == null)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
            player = GetClosestPlayer(players).transform;
    }

    if (player == null) return;

    float distance = Vector2.Distance(transform.position, player.position);
    cooldownTimer -= Time.deltaTime;

    if (distance <= detectionRange)
    {
        HandleCombatMovement(distance);
        FlipToFacePlayer();

        // 🔥 Dispara SIEMPRE que esté dentro del rango de detección
        if (!isAttacking && cooldownTimer <= 0f)
            StartCoroutine(ShootArrow());
    }
    else
    {
        Patrol();
    }

    if (anim != null)
        anim.SetFloat("Speed", rb.linearVelocity.magnitude);
}


    GameObject GetClosestPlayer(GameObject[] players)
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

void HandleCombatMovement(float distance)
{
    if (isKnockedBack) return;

    Vector2 direction;

    if (distance > idealRange)
    {
        // Acercarse
        direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
    else if (distance < tooCloseRange)
    {
        // Alejarse
        direction = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
    else
    {
        // Quieto en rango ideal
        rb.linearVelocity = Vector2.zero;
    }
}


    void Patrol()
    {
        Vector3 target = movingToPointA ? pointA.position : pointB.position;
        Vector2 direction = (target - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (Vector2.Distance(transform.position, target) < 0.2f)
            movingToPointA = !movingToPointA;
    }

    void FlipToFacePlayer()
    {
        if (player != null)
        {
            bool lookingRight = player.position.x > transform.position.x;
            transform.localScale = new Vector3(lookingRight ? 1 : -1, 1, 1);
        }
    }

IEnumerator ShootArrow()
{
    isAttacking = true;
    rb.linearVelocity = Vector2.zero;

    if (anim != null)
        anim.SetTrigger("Attack");

    cooldownTimer = attackCooldown;

    yield return new WaitForSeconds(0.5f); // tiempo de bloqueo opcional
    isAttacking = false;
}


    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (rb != null)
        {
            isKnockedBack = true;
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
            StartCoroutine(RecoverFromKnockback(duration));
        }
    }
    public void FireArrow()
{
    if (player == null) return;

    Vector2 dir = (player.position - firePoint.position).normalized;
    GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

    Arrow arrowScript = arrow.GetComponent<Arrow>();
    if (arrowScript != null)
        arrowScript.SetDirection(dir);
}


    IEnumerator RecoverFromKnockback(float time)
    {
        yield return new WaitForSeconds(time);
        isKnockedBack = false;
    }
}
