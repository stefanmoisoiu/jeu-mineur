using UnityEngine;

public class PFlipSpriteWallStick : MovementState
{
    [Header("References")]
    [SerializeField] private PFlipSprite flipSprite;
    [SerializeField] private PWallStick wallStick;
    
    protected override void ActiveStateUpdate()
    {
        flipSprite.Look(wallStick.WallStickLookDirection);
    }
}