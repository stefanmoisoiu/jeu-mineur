using UnityEngine;

public class PJumpProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private ProceduralAnimation anim;
    [SerializeField] private PMovement movement;

    private void Start()
    {
        movement.OnJump += () => StartAnimation(PlayAnim);;
    }

    private void PlayAnim()
    {
        anim.StopAnimation(this);
        anim.StartAnimation(this);
    }

    internal override void StopAnimation()
    {
        anim.StopAnimation(this);
    }
}
