using UnityEngine;
using UnityEngine.SceneManagement;

public class PPauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PInputManager inputManager;
    
    [Header("Properties")]
    [SerializeField] private string pauseSceneName = "Pause Menu";
    

    private void Start()
    {
        inputManager.OnPause += OpenPauseMenu;
        PauseManager.OnClosePause += ClosePauseMenu;
    }

    private void OpenPauseMenu()
    {
        inputManager.SetInputActive(false);
        Time.timeScale = 0;
        SceneManager.LoadScene(pauseSceneName, LoadSceneMode.Additive);
    }
    private void ClosePauseMenu()
    {
        inputManager.SetInputActive(true);
        Time.timeScale = 1;
    }
}
