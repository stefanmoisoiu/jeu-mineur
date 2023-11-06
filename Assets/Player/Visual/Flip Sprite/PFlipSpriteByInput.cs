using UnityEngine;

public class PFlipSpriteByInput : MovementState
{
    [Header("References")]
    [SerializeField] private PFlipSprite flipSprite;
    [SerializeField] private PInputManager inputManager;
    
    protected override void ActiveStateUpdate()
    {
        if (inputManager.MoveInput != 0)
            flipSprite.Look(inputManager.MoveInput);
    }
}
