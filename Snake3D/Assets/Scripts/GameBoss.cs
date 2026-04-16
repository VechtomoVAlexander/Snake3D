using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameBoss : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Image blackPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button restartButton;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 2f;

    public static GameBoss Instance;
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    { 
        score = 0; 

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);

        if (blackPanel != null)
            blackPanel.color = new Color(0, 0, 0, 0);

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
            scoreText.text = ""; 
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    public void AddScore()
    {
        score++;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    public int GetScore()
    {
        return score;
    }

    private IEnumerator GameOverSequence()
    {
        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(true);

        if (blackPanel != null)
        {
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
                blackPanel.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        if (scoreText != null)
        {
            scoreText.text = $"GAME OVER\nScore: {score}";
            scoreText.gameObject.SetActive(true);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveAllListeners();
    }
}