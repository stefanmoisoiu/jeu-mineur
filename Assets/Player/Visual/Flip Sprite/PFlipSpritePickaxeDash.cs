using UnityEngine;

public class PFlipSpritePickaxeDash : MovementState
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PFlipSprite flipSprite;

    protected override void OnStateEnter()
    {
        flipSprite.SetFlip(rb.velocity.x > 0);
    }
}
