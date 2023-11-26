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
        inputManager.OnMainAction += pickaxe.UsePickaxe;
        
    }

    protected override void OnStateExit()
    {
        inputManager.OnMainAction -= pickaxe.UsePickaxe;
        // grounded.OnGroundedChanged -= TryResetPickaxe;
    }

    private void TryResetPickaxe(bool wasGrounded, bool isGrounded)
    {
        if (!wasGrounded && isGrounded)
            pickaxe.ResetPickaxe();
    }
}