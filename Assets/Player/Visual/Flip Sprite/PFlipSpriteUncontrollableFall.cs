using UnityEngine;

public class PFlipSpriteUncontrollableFall : MovementState
{
    [SerializeField] private PUncontrollableFall uncontrollableFall;
    [SerializeField] private PFlipSprite flipSprite;
    
    protected override void ActiveStateUpdate()
    {
        flipSprite.SetFlip(uncontrollableFall.GoingRight);
    }
}