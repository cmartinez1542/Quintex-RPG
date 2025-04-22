using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public GameObject currentRoom; // La sala activa

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCurrentRoom(GameObject newRoom)
    {
        if (currentRoom != null)
        {
            currentRoom.SetActive(false); // Apaga la anterior
        }

        currentRoom = newRoom;
        currentRoom.SetActive(true); // Enciende la nueva
        Debug.Log("📦 Activando sala: " + currentRoom.name);
    }
}
