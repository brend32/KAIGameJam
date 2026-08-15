using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 120f;
    [SerializeField] private bool timerIsRunning = true;
    [SerializeField] private float fadeStartTime = 20f;

    [Header("Game Over Settings")]
    [SerializeField] private float delayBeforeGameOver = 1f;   // пауза після затемнення
    [SerializeField] private float gameOverFadeDuration = 1f;  // тривалість появи Game Over

    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private Button restartButton;

    public float TimeRemaining => timeRemaining;
    public bool IsRunning => timerIsRunning;

    private int lastTickSecond;
    private bool gameOverStarted = false;

    void Start()
    {
        lastTickSecond = Mathf.FloorToInt(timeRemaining);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0f;
            gameOverCanvasGroup.interactable = false;
            gameOverCanvasGroup.blocksRaycasts = false;
        }

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        DisplayTime(timeRemaining);
    }

    void Update()
    {
        if (!timerIsRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0;

            DisplayTime(timeRemaining);
            UpdateFade();

            int currentSecond = Mathf.FloorToInt(timeRemaining);

            if (currentSecond < lastTickSecond)
            {
                lastTickSecond = currentSecond;

                if (timeRemaining <= 20f && AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.timerTick);
                }
            }
        }
        else
        {
            timeRemaining = 0;
            timerIsRunning = false;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.timerEnd, 1.5f);
            }

            DisplayTime(timeRemaining);
            UpdateFade();

            if (!gameOverStarted)
            {
                gameOverStarted = true;
                StartCoroutine(ShowGameOverRoutine());
            }
        }
    }

    private void UpdateFade()
    {
        if (fadeImage == null) return;

        float alpha;

        if (timeRemaining >= fadeStartTime)
        {
            alpha = 0f;
        }
        else
        {
            // Лінійне затемнення від 0 до 1
            alpha = 1f - (timeRemaining / fadeStartTime);
        }

        alpha = Mathf.Clamp01(alpha);

        Color currentColor = fadeImage.color;
        currentColor.a = alpha;
        fadeImage.color = currentColor;
    }

    private IEnumerator ShowGameOverRoutine()
    {
        // Заблокувати гравця одразу
        PlayerMovement.CanMove = false;

        // 1. Пауза після повного затемнення
        yield return new WaitForSeconds(delayBeforeGameOver);

        // 2. Активуємо панель
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverCanvasGroup == null)
            yield break;

        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;

        // 3. Плавна поява за gameOverFadeDuration
        float t = 0f;
        while (t < gameOverFadeDuration)
        {
            t += Time.deltaTime;
            gameOverCanvasGroup.alpha = Mathf.Clamp01(t / gameOverFadeDuration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}