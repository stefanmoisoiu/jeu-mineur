using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Slider progressBarSlider;
    
    
    
    [Header("Properties")] [SerializeField]
    private string gameSceneName = "Game";
    [SerializeField] private Button[] buttonsToLock;
    
    

    public enum MenuState
    {
        Main,
        Options,
    }
    private MenuState _menuState = MenuState.Main;
    public MenuState CurrentMenuState => _menuState;

    
    public void PlayButton()
    {
        StartGame();
    }
    public void OptionsButton() => SetMenuState(MenuState.Options);
    public void QuitButton()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        SetLockedState(true);
        StartCoroutine(LoadSceneAsync(gameSceneName));
        SetLockedState(false);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        progressBar.SetActive(true);
        while (!asyncLoad.isDone)
        {
            progressBarSlider.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }
        progressBar.SetActive(false);
    }
    public void SetMenuState(MenuState newState)
    {
        if(newState == MenuState.Options) optionsMenu.SetActive(true);
        else optionsMenu.SetActive(false);
        
        _menuState = newState;
    }
    
    public void SetLockedState(bool locked)
    {
        foreach (Button button in buttonsToLock)
        {
            button.interactable = !locked;
        }
    }
}
