using UnityEngine;
using TMPro;

public class score : MonoBehaviour
{
    public TextMeshProUGUI scoreText; //text for score
    public TextMeshProUGUI timerText; //text for the timer
    public static int playerScore = 0; //score starts at 0
    private float timer = 0f;    //timer starts at 0
    

    void Start()
    {
        //init UI text
        UpdateScoreUI();
        UpdateTimerUI();
    }

    void Update()
    {
        //increment the timer
        timer += Time.deltaTime;
        UpdateTimerUI();
    }

    public void AddScore(int points)
    {
        playerScore += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + playerScore;
    }

    private void UpdateTimerUI()
    {
        timerText.text = "Time Survived: " + Mathf.FloorToInt(timer).ToString();
    }
}
