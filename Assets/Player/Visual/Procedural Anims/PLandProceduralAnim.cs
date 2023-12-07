using UnityEngine;

public class PLandProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private ProceduralAnimation anim;
    [SerializeField] private PGrounded grounded;
    

    private void Start()
    {
        grounded.OnGroundedChanged += (bool wasGrounded, bool isGrounded) =>
        {
            if (!wasGrounded && isGrounded)
                StartAnimation(PlayAnim);
        };
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
