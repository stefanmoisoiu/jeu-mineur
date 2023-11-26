using System;
using UnityEngine;

public class PPickaxe : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    [SerializeField] private PMovement movement;
    
    
    [Header("Pickaxe Properties")]
    [SerializeField] private int maxHits = 3;
    private int _hitsRemaining;
    public int MaxHits => maxHits;
    public int HitsRemaining => _hitsRemaining;

    public Action<int> OnPickaxeUsed;
    public Action OnPickaxeReset;
    private void Start()
    {
        _hitsRemaining = maxHits;
        _debugText = () => $"Pickaxe Hits Remaining: {_hitsRemaining}";
        debug.AddDebugText(_debugText);
    }
    public void UsePickaxe()
    {
        if (_hitsRemaining <= 0)
            return;
        if (movement.IsFullyOnGround)
            return;
        _hitsRemaining--;
        OnPickaxeUsed?.Invoke(_hitsRemaining);
    }
    
    public void ResetPickaxe()
    {
        _hitsRemaining = maxHits;
        OnPickaxeReset?.Invoke();
    }
}
