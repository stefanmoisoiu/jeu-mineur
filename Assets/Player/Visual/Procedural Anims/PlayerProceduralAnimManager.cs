using UnityEngine;

public class PlayerProceduralAnimManager : MonoBehaviour
{
    [SerializeField] private PlayerProceduralAnim[] anims;
    
    public void StopOtherProceduralAnimations(PlayerProceduralAnim anim)
    {
        foreach (PlayerProceduralAnim proceduralAnim in anims)
        {
            if (proceduralAnim != anim)
                proceduralAnim.StopAnimation();
        }
    }
}