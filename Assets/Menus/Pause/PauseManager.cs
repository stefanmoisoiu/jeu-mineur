using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private string pauseSceneName = "Pause Menu",settingsSceneName = "Settings Menu", mainMenuSceneName = "Menu";
    
    public static Action OnResume;
    
    public void ResumeButton()
    {
        OnResume?.Invoke();
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
