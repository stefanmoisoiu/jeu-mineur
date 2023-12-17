using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    public Texture2D TakeScreenshot()
    {
        Camera mainCam = Camera.main;
        
        RenderTexture renderTexture = new (Screen.width, Screen.height, 24);
        mainCam.targetTexture = renderTexture;
        Texture2D screenShot = new (Screen.width, Screen.height, TextureFormat.RGB24, false)
        {
            filterMode = FilterMode.Point,
        };

        mainCam.Render();
        
        RenderTexture.active = renderTexture;
        
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        
        
        
        mainCam.targetTexture = null;
        RenderTexture.active = null;
        
        Destroy(renderTexture);

        return screenShot;
    }
}