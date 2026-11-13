using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Image comboTimerBar; 

    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreUpdated += UpdateScoreDisplay;
            ScoreManager.Instance.OnTimerUpdated += UpdateTimerBar;
        }

       
        UpdateScoreDisplay(0, 1);
        UpdateTimerBar(0f);
    }

    void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreUpdated -= UpdateScoreDisplay;
            ScoreManager.Instance.OnTimerUpdated -= UpdateTimerBar;
        }
    }

    private void UpdateScoreDisplay(int score, int multiplier)
    {
        scoreText.text = $"SCORE: {score:N0}";

    
        if (multiplier > 1)
        {
            multiplierText.gameObject.SetActive(true);
            multiplierText.text = $"x{multiplier}";
        }
        else
        {
            multiplierText.gameObject.SetActive(false);
        }
    }

    private void UpdateTimerBar(float fillPercentage)
    {
        if (comboTimerBar != null)
        {
            comboTimerBar.fillAmount = fillPercentage;
            comboTimerBar.gameObject.SetActive(fillPercentage > 0f);
        }
    }
}