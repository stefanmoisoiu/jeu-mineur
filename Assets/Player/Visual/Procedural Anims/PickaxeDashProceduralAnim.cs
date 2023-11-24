using UnityEngine;

public class PickaxeDashProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private ProceduralAnimation anim;
    [SerializeField] private PPickaxeDash pickaxeDash;
    

    private void Start()
    {
        pickaxeDash.OnStartDash += () => StartAnimation(PlayAnim);
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
