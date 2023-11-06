using System;
using UnityEngine;

public class PlayerWallStickEvents : MonoBehaviour
{
    public Action OnWallStick, OnWallUnStick;
    
    public void WallStick()
    {
        OnWallStick?.Invoke();
    }
    public void WallUnStick()
    {
        OnWallUnStick?.Invoke();
    }
}