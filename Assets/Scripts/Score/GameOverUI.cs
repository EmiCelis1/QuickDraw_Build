using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerHealth playerHealth;
    public Transform playerCamera;          
    public GameObject gameOverPanel;
    public float distanciaAlJugador = 2f;

    [Header("Textos (TextMeshPro)")]
    public TextMeshProUGUI roundsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI rankText;

    [Header("Puntos mínimos por rango")]
    public int minS = 100000;
    public int minA = 75000;
    public int minB = 50000;
    public int minC = 40000;

    [Header("Colores de Rango")]
    public Color rankSColor = new Color(1f, 0.84f, 0f);
    public Color rankAColor = new Color(0.2f, 0.8f, 0.2f);
    public Color rankBColor = new Color(0.2f, 0.6f, 1f);
    public Color rankCColor = new Color(1f, 0.6f, 0f);
    public Color rankFColor = new Color(0.8f, 0.1f, 0.1f);

    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerHealth != null) playerHealth.OnPlayerDeath += OnPlayerDied;
    }

    private void OnDisable()
    {
        if (playerHealth != null) playerHealth.OnPlayerDeath -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        var stats = GameStats.Instance;
        MostrarGameOver(stats.Score, stats.Kills, stats.RoundsSurvived);
    }

    public void MostrarGameOver(int finalScore, int totalKills, int rounds)
    {
       
        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 pos = playerCamera.position + forward * distanciaAlJugador;

        gameOverPanel.transform.position = pos;
        gameOverPanel.transform.rotation = Quaternion.LookRotation(pos - playerCamera.position);
        gameOverPanel.SetActive(true);

        roundsText.text = "RONDAS SUPERADAS: " + rounds;
        scoreText.text = "PUNTUACIÓN: " + finalScore.ToString("N0");
        killsText.text = "ENEMIGOS ELIMINADOS: " + totalKills;

        CalcularRango(finalScore);
    }

    private void CalcularRango(int score)
    {
        string letra; Color color;

        if (score >= minS) { letra = "S"; color = rankSColor; }
        else if (score >= minA) { letra = "A"; color = rankAColor; }
        else if (score >= minB) { letra = "B"; color = rankBColor; }
        else if (score >= minC) { letra = "C"; color = rankCColor; }
        else { letra = "F"; color = rankFColor; }

        rankText.text = letra;
        rankText.color = color;
    }

    public void BotonReintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BotonSalirOMenu(string nombreEscenaMenu)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}