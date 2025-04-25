using UnityEngine;

public class BookPuzzleManager : MonoBehaviour
{

    public GameObject puzzleUIPanel;         // el panel de libros (Canvas)
public GameObject exclamationIcon;       // el signo de admiración
public GameObject puzzleTriggerObject;   // el GameObject con PuzzleTrigger.cs
public Transform enemySpawnPoint;        // lugar donde aparecerán los enemigos
public GameObject[] enemiesToSpawn;      // los enemigos (prefabs o ya en escena y desactivados)
public BookVisual[] sceneBooks;          // los libros fuera del puzzle que deben reflejar el orden


    public static BookPuzzleManager Instance;
    public BookPuzzleSlot[] slots;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckSolution()
    {
        string[] correctOrder = { "yellow","blue", "black", "green", "red" }; // ajusta según tu puzzle

     for (int i = 0; i < slots.Length; i++)
    {
        if (slots[i].bookID != correctOrder[i])
        {
            Debug.Log("❌ Orden incorrecto");
            return;
        }
    }

    Debug.Log("✅ Puzzle resuelto");

    // 🔴 1. Cerrar el canvas del puzzle
    puzzleUIPanel.SetActive(false);
    Time.timeScale = 1f;

    // 🔴 2. Desactivar el signo de admiración
    exclamationIcon.SetActive(false);

    // 🔴 3. Desactivar el trigger para no volver a entrar
    puzzleTriggerObject.SetActive(false);

    // 🔴 4. Instanciar enemigos
    foreach (GameObject enemy in enemiesToSpawn)
    {
        enemy.SetActive(true); // si ya están en escena desactivados
        // O instanciar: Instantiate(enemy, enemySpawnPoint.position, Quaternion.identity);
    }

    // 🔴 5. Reflejar los libros en la escena
    for (int i = 0; i < sceneBooks.Length; i++)
    {
        sceneBooks[i].SetBook(slots[i].bookID, slots[i].image.sprite);
    }
}
}
