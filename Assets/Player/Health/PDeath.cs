using System;
using UnityEngine;

public class PDeath : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ColliderEvents colliderEvents;
    
    [Header("Death Properties")]
    [SerializeField] private LayerMask deathLayerMask;
    
    
    public Action OnDeath;
    private void Start()
    {
        colliderEvents.OnTriggerEnterValue += CheckDie;
    }
    private void CheckDie(Collider2D other)
    {
        if (deathLayerMask != (deathLayerMask | (1 << other.gameObject.layer))) return;
        OnDeath?.Invoke();
    }
}
