using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Button start;
    public Button settings;
    public Button closeSettings;

    void Start()
    {
        start.onClick.AddListener(StartGame);
        settingsPanel.SetActive(false);
        settings.onClick.AddListener(OpenSettings);
        closeSettings.onClick.AddListener(CloseSettings);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    } 

    public void StartGame()
    {
        SceneManager.LoadScene("Mapa");
        Debug.Log("apretado");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
