using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public Camera mainCamera;
    public Sprite[] playerOutfits;
    public Animator[] animator;
    public PlayerMovement movement;
    public RuntimeAnimatorController[] playerAnimatorControllers;

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;

        Debug.Log("Spawn Points Count: " + spawnPoints.Length);
        Debug.Log("Player Outfits Count: " + playerOutfits.Length);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Rigidbody2D rbCheck = playerInput.GetComponent<Rigidbody2D>();

        Debug.Log("Player joined: " + playerInput.playerIndex);

        int index = (PlayerInputManager.instance.playerCount - 1) % spawnPoints.Length;
        playerInput.transform.position = spawnPoints[index].position;

        SpriteRenderer spriteRenderer = playerInput.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && playerOutfits.Length > index)
        {
            Debug.Log("Assigning sprite to player " + index + ": " + playerOutfits[index].name);
            spriteRenderer.sprite = playerOutfits[index];
        }
        else
        {
            Debug.LogWarning("No sprite found or index out of range for player " + index);
        }

  
        if (mainCamera != null && PlayerInputManager.instance.playerCount == 1)
        {
            CameraController camController = mainCamera.GetComponent<CameraController>();
            if (camController != null)
            {
                camController.SetTarget(playerInput.transform);
            }
        }

        // Asigna Animator Controller por índice
        Animator animator = playerInput.GetComponentInChildren<Animator>(true);
        if (animator != null && playerAnimatorControllers.Length > index)
        {
            animator.runtimeAnimatorController = playerAnimatorControllers[index];
            Debug.Log($"Animator Controller '{playerAnimatorControllers[index].name}' successfully assigned to player {index}.");
        }
        else
        {
            Debug.LogError($"Animator assignment failed. Animator found: {animator != null}, Animator Controllers array length: {playerAnimatorControllers.Length}, Index: {index}");
        }
    }
}
