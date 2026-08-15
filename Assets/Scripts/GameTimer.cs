using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 120f; 
    [SerializeField] private bool timerIsRunning = true;
    [SerializeField] private float fadeStartTime = 20f;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image fadeImage; 

    public float TimeRemaining => timeRemaining; 
    public bool IsRunning => timerIsRunning;

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                UpdateFade(); 
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);
                UpdateFade(); 
            }
        }
    }

    private void UpdateFade()
    {
        if (fadeImage != null && timeRemaining <= fadeStartTime)
        {
            float alpha = 1f - (timeRemaining / fadeStartTime);

            Color currentColor = fadeImage.color;
            currentColor.a = alpha;
            fadeImage.color = currentColor;
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}