using UnityEngine;

public class PFlipSpriteUncontrollableFall : MovementState
{
    [SerializeField] private PUncontrollable uncontrollable;
    [SerializeField] private PFlipSprite flipSprite;
    
    protected override void ActiveStateUpdate()
    {
        flipSprite.SetFlip(uncontrollable.GoingRight);
    }
}