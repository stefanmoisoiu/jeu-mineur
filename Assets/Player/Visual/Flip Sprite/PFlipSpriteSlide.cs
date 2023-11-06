using UnityEngine;

public class PFlipSpriteSlide : MovementState
{
    [SerializeField] private PGroundStick groundStick;
    [SerializeField] private PFlipSprite flipSprite;
    [SerializeField] private float flipThreshold = 0.1f;
    
    protected override void ActiveStateUpdate()
    {
        if (Mathf.Abs(groundStick.HorizontalVelocity) < flipThreshold) return;
        flipSprite.SetFlip(groundStick.HorizontalVelocity > 0);
    }
}