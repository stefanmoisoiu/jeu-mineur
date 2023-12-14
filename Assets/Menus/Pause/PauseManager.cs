using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private string pauseSceneName = "Pause Menu",settingsSceneName = "Settings Menu", mainMenuSceneName = "Menu";
    
    public static Action OnOpenPause, OnClosePause;
    public static bool PauseOpen { get; private set; }
    
    private void OnEnable()
    {
        PauseOpen = true;
        OnOpenPause?.Invoke();
    }

    private void OnDisable()
    {
        PauseOpen = false;
        OnClosePause?.Invoke();
    }

    public void ResumeButton()
    {
        SceneManager.UnloadSceneAsync(pauseSceneName);
    }
    public void SettingsButton()
    {
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
