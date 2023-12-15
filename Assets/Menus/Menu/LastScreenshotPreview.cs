using UnityEngine;
using UnityEngine.UI;

public class LastScreenshotPreview : MonoBehaviour
{
    private static string LastScreenshotKey = "lastScreenshot";
    [SerializeField] private RawImage screenshotPreview;
    [SerializeField] private GameObject screenshotPreviewContainer;
    
    private void Awake()
    {
        SetScreenshotPreview();
    }
    
    private void SetScreenshotPreview()
    {
        try
        {
            screenshotPreview.texture = DecodeImage(ES3.Load<byte[]>(LastScreenshotKey));
            screenshotPreviewContainer.SetActive(true);
        }
        catch (System.Exception e)
        {
            screenshotPreviewContainer.SetActive(false);
        }
    }
    
    private Texture2D DecodeImage(byte[] imageData)
    {
        Texture2D texture = new(2, 2);
        if (texture.LoadImage(imageData))
        {
            return texture;
        }
        else
        {
            Debug.LogError("Failed to decode image");
            return null;
        }
    }
}