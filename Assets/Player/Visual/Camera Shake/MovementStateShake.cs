using UnityEngine;

public class MovementStateShake : MovementState
{
    [SerializeField] private ScriptableCameraShake enterShake, exitShake;

    protected override void OnStateEnter()
    {
        if (enterShake == null) return;
        enterShake.Shake();
    }

    protected override void OnStateExit()
    {
        if (exitShake == null) return;
        exitShake.Shake();
    }
}
