using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad = "NombreDeTuEscena";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // asegura que solo el jugador pueda activarlo
        {
            Debug.Log("🔁 Cambiando a escena: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
