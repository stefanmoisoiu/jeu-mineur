using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private string closeHatchAnimationName = "CloseHatch";
    [SerializeField] private string openHatchAnimationName = "OpenHatch";
    [Space]
    [SerializeField] private Collider2D collider2D;
    
    private bool _isOpened;
    public bool IsOpened => _isOpened;

    public Action OnOpen, OnClose;
    public Action<bool> OnStateChanged;
    
    internal void SetState(bool opened,bool playAnimation = true)
    {
        if(_isOpened == opened) return;
        _isOpened = opened;
        
        string animToPlay = opened ? openHatchAnimationName : closeHatchAnimationName;
        if(playAnimation) animator.Play(animToPlay);
        
        collider2D.enabled = !opened;
        
        if (opened)
            OnOpen?.Invoke();
        else
            OnClose?.Invoke();
        OnStateChanged?.Invoke(opened);
    }
}
