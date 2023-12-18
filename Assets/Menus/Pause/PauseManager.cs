using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private string pauseSceneName = "Pause Menu",settingsSceneName = "Settings Menu", mainMenuSceneName = "Menu";
    [SerializeField] private Button[] buttonsToDisableOnSettings;
    [SerializeField] private Selectable mainFocus;
    
    
    
    public static Action OnOpenPause, OnClosePause, OnResumeButton, OnSettingsButton, OnMainMenuButton;
    public static bool PauseOpen { get; private set; }
    
    private void OnEnable()
    {
        PauseOpen = true;
        OnOpenPause?.Invoke();
        
        SettingsManager.OnCloseSettings += SettingsClosed;
        
        mainFocus.Select();
    }

    private void OnDisable()
    {
        PauseOpen = false;
        OnClosePause?.Invoke();
        
        SettingsManager.OnCloseSettings -= SettingsClosed;
    }

    public void ResumeButton()
    {
        SceneManager.UnloadSceneAsync(pauseSceneName);
        OnResumeButton?.Invoke();
    }
    public void SettingsButton()
    {
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
        foreach (Button button in buttonsToDisableOnSettings) button.interactable = false;
        
        SetButtons(false);
        
        OnSettingsButton?.Invoke();
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(mainMenuSceneName);
        OnMainMenuButton?.Invoke();
    }
    
    private void SettingsClosed()
    {
        SetButtons(true);
        mainFocus.Select();
    }
    private void SetButtons(bool enable)
    {
        foreach (Button button in buttonsToDisableOnSettings) button.interactable = enable;
    }
    
}
