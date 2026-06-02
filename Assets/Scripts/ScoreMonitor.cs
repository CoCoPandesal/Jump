using UnityEngine;
using UnityEngine.UI;

public class ScoreMonitor : MonoBehaviour
{
    public Text scoreText;
    public Text highscoreText;

    public static ScoreMonitor Instance;

    private int score = 0;
    private int highscore = 0;

    void Awake()
    {
        Instance = this;
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        // Auto-find Text components if not assigned in Inspector
        if (scoreText == null)
        {
            GameObject scoreObj = GameObject.Find("Score");
            if (scoreObj != null) scoreText = scoreObj.GetComponent<Text>();
        }

        if (highscoreText == null)
        {
            GameObject highscoreObj = GameObject.Find("Hignscore");
            if (highscoreObj == null) highscoreObj = GameObject.Find("Highscore");
            if (highscoreObj != null) highscoreText = highscoreObj.GetComponent<Text>();
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore()
    {
        score++;

        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score.ToString();
        if (highscoreText != null)
            highscoreText.text = "BEST: " + highscore.ToString();
    }
}
