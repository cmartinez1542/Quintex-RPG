using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2 : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    public int facingDirection = 1;

    public Joystick joystick; // Opcional para controles móviles
    public Player_Combat player_combat;

    private Vector2 movementInput = Vector2.zero;
    private bool slashed = false;

    public static int playerCount = 0; // Cuenta global de jugadores
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(" Rigidbody2D is missing on " + gameObject.name);
        }

        player_combat = GetComponent<Player_Combat>();

        if (player_combat == null)
        {
            Debug.LogError(" Player_Combat script is missing on " + gameObject.name);
        }

        playerCount++; // Incrementar conteo de jugadores para asignar atuendos
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log(" Move Input Received: " + movementInput);
    }

    public void OnSlash(InputAction.CallbackContext context)
    {
        slashed = context.action.triggered;
        if (context.performed)
        {
            Debug.Log($" {gameObject.name} performed a slash!");
            player_combat.Attack();
        }
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogError(" Rigidbody2D is NULL in FixedUpdate!");
            return;
        }

        // Entrada por teclado y joystick
        float horizontal = movementInput.x;
        float vertical = movementInput.y;

        if (joystick != null)
        {
            horizontal += joystick.Direction.x;
            vertical += joystick.Direction.y;
        }

        Vector2 move = new Vector2(horizontal, vertical).normalized;

        rb.linearVelocity = move * speed;

        Debug.Log($" Horizontal: {horizontal}");
        Debug.Log($" Vertical: {vertical}");
        Debug.Log($" Move direction: {move}");
        Debug.Log($" FixedUpdate - Applying velocity: {rb.linearVelocity}");

        // Flip del sprite basado en dirección
        if (horizontal > 0 && facingDirection < 0)
            Flip();
        else if (horizontal < 0 && facingDirection > 0)
            Flip();

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));


        // Animaciones (opcional)
        /*
        anim.SetFloat("Speed", move.sqrMagnitude);
        */
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // Inicializar posición y atuendo
    public void InitializePlayer(Vector3 spawnPosition, Sprite outfit)
    {
        transform.position = spawnPosition;

        if (spriteRenderer != null && outfit != null)
        {
            spriteRenderer.sprite = outfit;
            Debug.Log($"Player {playerCount} assigned outfit: {outfit.name}");
        }
        else
        {
            Debug.LogWarning($"No sprite assigned to Player {playerCount}");
        }
    }
}
