using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject startGameProgressBarObject;
    [SerializeField] private Slider startGameProgressBar;
    [SerializeField] private Button[] buttonsToLock;
    [SerializeField] private Selectable mainFocus;
    
    
    [Header("Properties")]
    [SerializeField] private string settingsSceneName = "Settings Menu";
    [SerializeField] private string gameSceneName = "Game";

    
    public void PlayButton() => StartGame();
    public void SettingsButton()
    {
        SetButtons(false);
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
    }
    public void QuitButton() => Application.Quit();

    private void OnEnable()
    {
        SettingsManager.OnCloseSettings += SettingsClosed;
    }

    private void OnDisable()
    {
        SettingsManager.OnCloseSettings -= SettingsClosed;
    }

    private void StartGame()
    {
        SetButtons(false);
        StartCoroutine(StartGameLoadScene(gameSceneName));
        SetButtons(true);
    }
    private void SettingsClosed()
    {
        SetButtons(true);
        mainFocus.Select();
    }

    private IEnumerator StartGameLoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        startGameProgressBarObject.SetActive(true);
        while (!asyncLoad.isDone)
        {
            startGameProgressBar.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }
        startGameProgressBarObject.SetActive(false);
    }
    
    public void SetButtons(bool enable)
    {
        foreach (Button button in buttonsToLock) button.interactable = enable;
    }
}
