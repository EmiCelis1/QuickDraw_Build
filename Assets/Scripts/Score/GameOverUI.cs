using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Panel Principal")]
    [Tooltip("El objeto padre de la UI de Game Over que se activará al morir")]
    public GameObject gameOverPanel;

    [Header("Textos de UI (TextMeshPro)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI rankText;

    [Header("Colores de Rango (Estilo Resident Evil)")]
    public Color rankSColor = new Color(1f, 0.84f, 0f); 
    public Color rankAColor = new Color(0.2f, 0.8f, 0.2f); 
    public Color rankBColor = new Color(0.2f, 0.6f, 1f); 
    public Color rankCColor = new Color(1f, 0.6f, 0f); 
    public Color rankFColor = new Color(0.8f, 0.1f, 0.1f); 

    private void Start()
    {
      
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void MostrarGameOver(int finalScore, int totalKills)
    {
       
        gameOverPanel.SetActive(true);

      
        scoreText.text = "PUNTUACIÓN: " + finalScore.ToString("N0");
        killsText.text = "ENEMIGOS ELIMINADOS: " + totalKills.ToString();

     
        CalcularRango(finalScore);
    }

    private void CalcularRango(int score)
    {
        string letraRango = "F";
        Color colorRango = rankFColor;

       
        if (score >= 100000)
        {
            letraRango = "S";
            colorRango = rankSColor;
        }
        else if (score >= 75000) 
        {
            letraRango = "A";
            colorRango = rankAColor;
        }
        else if (score >= 50000)
        {
            letraRango = "B";
            colorRango = rankBColor;
        }
        else if (score >= 40000) 
        {
            letraRango = "C";
            colorRango = rankCColor;
        }
        else 
        {
            letraRango = "F";
            colorRango = rankFColor;
        }

        rankText.text = letraRango;
        rankText.color = colorRango;
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