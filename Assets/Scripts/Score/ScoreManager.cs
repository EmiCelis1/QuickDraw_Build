using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Configuración del Combo (Estilo Call of Juarez)")]
    [SerializeField] private float comboTimeWindow = 4f; 
    [SerializeField] private int maxMultiplier = 10;     

    private int currentScore = 0;
    private int currentMultiplier = 1;
    private float comboTimer = 0f;
    private bool isComboActive = false;

    
    public event Action<int, int> OnScoreUpdated; 
    public event Action<float> OnTimerUpdated;   

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Update()
    {
        if (isComboActive)
        {
            comboTimer -= Time.deltaTime;

            
            OnTimerUpdated?.Invoke(Mathf.Clamp01(comboTimer / comboTimeWindow));

            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

   
    public void RegisterKill(IScoreable scoreableTarget)
    {
      
        if (isComboActive)
        {
            currentMultiplier = Mathf.Min(currentMultiplier + 1, maxMultiplier);
        }
        else
        {
            isComboActive = true;
            currentMultiplier = 1; 
        }

       
        int pointsGained = scoreableTarget.BasePoints * currentMultiplier;
        currentScore += pointsGained;

        comboTimer = comboTimeWindow;

      
        OnScoreUpdated?.Invoke(currentScore, currentMultiplier);
        OnTimerUpdated?.Invoke(1f);
    }

    private void ResetCombo()
    {
        isComboActive = false;
        currentMultiplier = 1;
        comboTimer = 0f;

        OnScoreUpdated?.Invoke(currentScore, currentMultiplier);
        OnTimerUpdated?.Invoke(0f);
    }
}

