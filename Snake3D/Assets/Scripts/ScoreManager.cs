using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    private int score = 0;
    private int lastSpeedLevel = 0; 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateScoreText();
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        int newSpeedLevel = GetSpeedLevel(score);
        if (newSpeedLevel != lastSpeedLevel)
        {
            lastSpeedLevel = newSpeedLevel;
            if (PlayerMovement.Instance != null)
            {
                PlayerMovement.Instance.UpdateSpeedByScore(score);
            }
        }
    }
    private int GetSpeedLevel(int score)
    {
        if (score < 10) return 0;   
        if (score < 50) return 1;   
        return 2;                   
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}