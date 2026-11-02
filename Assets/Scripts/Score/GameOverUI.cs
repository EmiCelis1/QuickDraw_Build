using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Referencias")]
    private PlayerHealth playerHealth;
    public Transform playerCamera;
    public GameObject gameOverPanel;
    public float distanciaAlJugador = 2f;

    [Header("Canvas de HUD / Score a Ocultar")]
    [Tooltip("Arrastra aquí el GameObject del Canvas que muestra el score y las rondas mientras juegas")]
    [SerializeField] private GameObject hudCanvasToHide;

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

    private void Awake()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Busca automáticamente el componente PlayerHealth en la escena
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("GameOverUI no pudo encontrar ningún componente PlayerHealth en la escena.");
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath -= OnPlayerDied;
        }
    }

    private void OnPlayerDied()
    {
        // 1. Pausamos el juego por completo para que los enemigos se detengan
        Time.timeScale = 0f;

        // 2. Ocultamos el Canvas del HUD/Score en juego si está asignado
        if (hudCanvasToHide != null)
        {
            hudCanvasToHide.SetActive(false);
        }

        var stats = GameStats.Instance;

        if (stats != null)
        {
            MostrarGameOver(stats.Score, stats.Kills, stats.RoundsSurvived);
        }
        else
        {
            Debug.LogWarning("GameStats.Instance no encontrado en la escena. Mostrando valores en 0.");
            MostrarGameOver(0, 0, 0);
        }
    }

    public void MostrarGameOver(int finalScore, int totalKills, int rounds)
    {
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        if (playerCamera != null && gameOverPanel != null)
        {
            Vector3 forward = playerCamera.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 pos = playerCamera.position + forward * distanciaAlJugador;

            gameOverPanel.transform.position = pos;
            gameOverPanel.transform.rotation = Quaternion.LookRotation(pos - playerCamera.position);
            gameOverPanel.SetActive(true);
        }

        if (roundsText != null) roundsText.text = "RONDAS SUPERADAS: " + rounds;
        if (scoreText != null) scoreText.text = "PUNTUACIÓN: " + finalScore.ToString("N0");
        if (killsText != null) killsText.text = "ENEMIGOS ELIMINADOS: " + totalKills;

        CalcularRango(finalScore);
    }

    private void CalcularRango(int score)
    {
        if (rankText == null) return;

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
        // Importante restablecer el tiempo a 1 antes de recargar la escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BotonSalirOMenu(string nombreEscenaMenu)
    {
        // Restablecer el tiempo a 1 antes de cambiar de escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}