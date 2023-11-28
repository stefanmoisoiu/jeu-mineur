using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunRestart : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) Restart();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
