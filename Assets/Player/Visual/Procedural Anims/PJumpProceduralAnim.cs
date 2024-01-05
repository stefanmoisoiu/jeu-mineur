using System;
using UnityEngine;

public class PJumpProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private ProceduralAnimation anim;
    [SerializeField] private PMovement movement;

    private void OnEnable()
    {
        movement.OnJump += PlayAnim;
    }

    private void OnDisable()
    {
        movement.OnJump -= PlayAnim;
    }

    private void PlayAnim()
    {
        StartAnimation(() => anim.StartAnimation(this));
    }

    internal override void StopAnimation()
    {
        anim.StopAnimation(this);
    }
}
