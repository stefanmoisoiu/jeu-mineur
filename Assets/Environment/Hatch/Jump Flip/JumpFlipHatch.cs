using UnityEngine;

public class JumpFlipHatch : Hatch
{
    [Header("Hatch Properties")]
    [SerializeField] private bool startOpened;

    private void Start()
    {
        SetState(startOpened, false);
        PlayerMainEvents.OnPlayerJump += () => SetState(!IsOpened);
    }
}
