using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
   public GameObject healthDropPrefab; // El prefab del steak

    public int currentHealth;

    private Animator anim;
    private Rigidbody2D rb;

    private void Start()
    {
        
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("🩸 Enemigo recibió daño. Vida actual: " + currentHealth);


        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("☠️ Enemigo eliminado.");

        if (anim != null)
            anim.SetTrigger("Death"); // Animación de muerte (opcional)

        // Desactiva colisión y movimiento
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Enemy_Movement movement = GetComponent<Enemy_Movement>();
        if (movement != null) movement.enabled = false;

        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (healthDropPrefab != null)
{
    Instantiate(healthDropPrefab, transform.position, Quaternion.identity);
}


        // Destruye el objeto después de 1.5 segundos (para dejar animación de muerte)
        Destroy(gameObject, 1.5f);
    }
}

