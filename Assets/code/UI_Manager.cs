using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Button settings;
    public Button closeSettings;

    void Start()
    {
        settingsPanel.SetActive(false);
        settings.onClick.AddListener(OpenSettings);
        closeSettings.onClick.AddListener(CloseSettings);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    } 

    public void StartGame(string name)
    {
        SceneManager.LoadScene(name);
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
