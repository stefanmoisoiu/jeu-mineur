using UnityEngine;

public class PlayerTouchFallingPlatform : MonoBehaviour
{
    [SerializeField] private FallingPlatform fallingPlatform;
    [SerializeField] private PlayerGroundedEvents playerGroundedEvents;
    [SerializeField] private PlayerWallStickEvents playerWallStickEvents;
    
    

    private void Start()
    {
        playerGroundedEvents.OnGrounded += fallingPlatform.Fall;
        playerWallStickEvents.OnWallStick += fallingPlatform.Fall;
    }
}