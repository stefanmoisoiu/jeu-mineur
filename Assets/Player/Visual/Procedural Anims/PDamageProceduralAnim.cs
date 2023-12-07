using UnityEngine;

public class PDamageProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private PDamage damage;
    [SerializeField] private ProceduralAnimation proceduralAnim;

    private void Start()
    {
        damage.OnDamage += () => StartAnimation(() =>
        {
            proceduralAnim.StopAnimation(this);
            proceduralAnim.StartAnimation(this);
        });
    }

    internal override void StopAnimation()
    {
        proceduralAnim.StopAnimation(this);
    }
}
