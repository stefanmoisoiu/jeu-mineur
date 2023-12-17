using System;
using UnityEngine;

public class ObtainPickaxe : MonoBehaviour
{
    private static string PickaxeObtainedKey = "pickaxeObtained";
    public static Action OnPickaxeObtained;

    private void Awake()
    {
        bool pickaxeObtained = false;

        try
        {
            pickaxeObtained = ES3.Load(PickaxeObtainedKey, false);
        }
        catch (Exception e)
        {
            // ignored
        }
        
        if (pickaxeObtained)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        try
        {
            ES3.Save(PickaxeObtainedKey, true);
            OnPickaxeObtained?.Invoke();
            Destroy(gameObject);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
