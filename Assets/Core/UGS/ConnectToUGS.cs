using System;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;

public class ConnectToUGS : MonoBehaviour
{
    [SerializeField] private bool connectOnStart;
    public Action OnConnected;

    private void Start()
    {
        if (connectOnStart && UnityServices.State == ServicesInitializationState.Uninitialized)
            Connect();
    }

    private async void Connect()
    {
        Debug.Log("Connecting to Unity Game Services...");
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Connected to Unity Game Services successfully!");
        OnConnected?.Invoke();
    }
}
