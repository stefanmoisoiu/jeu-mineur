using UnityEngine;

public class PDamageProceduralAnim : PlayerProceduralAnim
{
    [SerializeField] private PDamage damage;
    [SerializeField] private ProceduralAnimation proceduralAnim;

    private void OnEnable()
    {
        damage.OnDamage += PlayAnim;
    }

    private void OnDisable()
    {
        damage.OnDamage -= PlayAnim;
    }

    private void PlayAnim()
    {
        StartAnimation(() => proceduralAnim.StartAnimation(this));
    }
    internal override void StopAnimation()
    {
        proceduralAnim.StopAnimation(this);
    }
}
