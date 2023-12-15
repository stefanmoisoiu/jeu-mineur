using System;
using UnityEngine;

public class SaveLastScreenshot : MonoBehaviour
{
    [SerializeField] private Screenshotter screenshotter;
    
    private static string LastScreenshotKey = "lastScreenshot";
    private void OnEnable()
    {
        Application.quitting += SaveScreenshot;
        PauseManager.OnMainMenuButton += SaveScreenshot;
    }

    private void OnDisable()
    {
        Application.quitting -= SaveScreenshot;
        PauseManager.OnMainMenuButton -= SaveScreenshot;
    
    }


    private void SaveScreenshot()
    {
        ES3.Save(LastScreenshotKey, screenshotter.TakeScreenshot());
        Debug.Log("Saved screenshot");
    }
}