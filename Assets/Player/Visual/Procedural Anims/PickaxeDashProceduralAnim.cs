using UnityEngine;

public class PickaxeDashProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private ProceduralAnimation anim;
    [SerializeField] private PPickaxeDash pickaxeDash;


    private void OnEnable()
    {
        pickaxeDash.OnStartDash += PlayAnim;
    }

    private void OnDisable()
    {
        pickaxeDash.OnStartDash -= PlayAnim;
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
