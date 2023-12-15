using System;
using UnityEngine;

public class PPickaxeUse : MovementState
{
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PPickaxe pickaxe;

    private void Start()
    {
        grounded.OnGroundedChanged += TryResetPickaxe;
    }

    protected override void OnStateEnter()
    {
        inputManager.OnMainAction += TryUsePickaxe;
        
    }

    protected override void OnStateExit()
    {
        inputManager.OnMainAction -= TryUsePickaxe;
        // grounded.OnGroundedChanged -= TryResetPickaxe;
    }

    private void TryUsePickaxe()
    {
        if (!pickaxe.enabled) return;
        pickaxe.UsePickaxe();
    } 

    private void TryResetPickaxe(bool wasGrounded, bool isGrounded)
    {
        if (!wasGrounded && isGrounded)
            pickaxe.ResetPickaxe();
    }
}