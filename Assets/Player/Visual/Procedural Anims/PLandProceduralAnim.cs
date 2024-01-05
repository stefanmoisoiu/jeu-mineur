using UnityEngine;

public class PLandProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private ProceduralAnimation anim;
    [SerializeField] private PGrounded grounded;


    private void OnEnable()
    {
        grounded.OnGroundedChanged += TryPlayAnim;
    }

    private void OnDisable()
    {
        grounded.OnGroundedChanged -= TryPlayAnim;
    }

    private void TryPlayAnim(bool wasGrounded, bool isGrounded)
    {
        if (!wasGrounded && isGrounded)
            StartAnimation(() => anim.StartAnimation(this));
    }

    internal override void StopAnimation()
    {
        anim.StopAnimation(this);
    }
}
