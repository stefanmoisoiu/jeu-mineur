using System;
using UnityEngine;

public abstract class SimpleMechanism : MonoBehaviour
{
    public Action OnActivated, OnDeactivated;
    public void Activate() => OnActivated?.Invoke();
    public void Deactivate() => OnDeactivated?.Invoke();
}
