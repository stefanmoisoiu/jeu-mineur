using UnityEngine;

public class PPickaxeUse : MovementState
{
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PPickaxe pickaxe;

    protected override void OnStateEnter()
    {
        inputManager.OnMainAction += pickaxe.UsePickaxe;
    }

    protected override void OnStateExit()
    {
        inputManager.OnMainAction -= pickaxe.UsePickaxe;
    }

    protected override void ActiveStateUpdate()
    {
        if (grounded.IsGrounded) pickaxe.ResetPickaxe();
    }
}