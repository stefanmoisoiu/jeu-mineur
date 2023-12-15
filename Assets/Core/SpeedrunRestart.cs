using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunRestart : MonoBehaviour
{
    private static string PlayerPositionKey = "playerPosition";
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) Restart();
    }

    private void Restart()
    {
        ES3.Save(PlayerPositionKey, new Vector2(1.5f,-2.5f));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
