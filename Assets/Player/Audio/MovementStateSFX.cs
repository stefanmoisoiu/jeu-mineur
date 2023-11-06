using UnityEngine;

public class MovementStateSFX : MovementState
{
    [SerializeField] private ScriptableSFX enterSfx, exitSfx;

    protected override void OnStateEnter()
    {
        if (enterSfx == null) return;
        enterSfx.Play();
    }

    protected override void OnStateExit()
    {
        if (exitSfx == null) return;
        exitSfx.Play();
    }
}