using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject roomToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomManager.Instance.SetCurrentRoom(roomToActivate);
        }
    }
}
