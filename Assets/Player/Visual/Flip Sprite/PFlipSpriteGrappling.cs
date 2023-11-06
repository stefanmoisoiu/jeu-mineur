using UnityEngine;

public class PFlipSpriteGrappling : MovementState
{
    [Header("References")]
    [SerializeField] private PFlipSprite flipSprite;
    [SerializeField] private PGrappling grappling;
    [SerializeField] private float stopFlipVelocity = 1;
    
    protected override void ActiveStateUpdate()
    {
        flipSprite.SetFlip(grappling.HorizontalVelocity > 0 && Mathf.Abs(grappling.HorizontalVelocity) > stopFlipVelocity);
    }
}