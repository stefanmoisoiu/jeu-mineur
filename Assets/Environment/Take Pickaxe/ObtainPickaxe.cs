using System;
using UnityEngine;

public class ObtainPickaxe : MonoBehaviour
{
    private static string PickaxeObtainedKey = "pickaxeObtained";
    public static Action OnPickaxeObtained;

    private void Awake()
    {
        bool pickaxeObtained = ES3.Load(PickaxeObtainedKey, false);
        if (pickaxeObtained)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        ES3.Save(PickaxeObtainedKey, true);
        OnPickaxeObtained?.Invoke();
        Destroy(gameObject);
    }
}
