using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI restartButtonText;
    public TextMeshProUGUI quitButtonText;
    public TextMeshProUGUI scoreText; // TextMeshPro to show the score

    private void Start()
    {
        // Directly access the static player score
        scoreText.text = "Score: " + score.playerScore.ToString();
    }

    public void RestartGame()
    {
        Debug.Log("Restarting the game...");
        // Reset score to 0
        score.playerScore = 0;

        SceneManager.LoadScene("Map_1");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        // Quit the application
        Application.Quit();
    }
}
