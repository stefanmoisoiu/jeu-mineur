using System;
using UnityEngine;

public class PlayerGroundedEvents : MonoBehaviour
{
    public Action<bool,bool> OnGroundedChanged, OnGroundCloseChanged;
    public Action OnGrounded, OnStopGrounded, OnGroundClose, OnStopGroundClose;

    public void GroundedChanged(bool wasGrounded, bool isGrounded)
    {
        OnGroundedChanged?.Invoke(wasGrounded, isGrounded);
        if (isGrounded)
            OnGrounded?.Invoke();
        else
            OnStopGrounded?.Invoke();
    }
    public void GroundCloseChanged(bool wasGroundClose, bool isGroundClose)
    {
        OnGroundCloseChanged?.Invoke(wasGroundClose, isGroundClose);
        if (isGroundClose)
            OnGroundClose?.Invoke();
        else
            OnStopGroundClose?.Invoke();
    }
}
