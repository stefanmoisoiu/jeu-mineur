using UnityEngine;

public class WallBounce : MonoBehaviour
{
    [SerializeField] private float bounceForceMult = 1f;
    public float BounceForceMult => bounceForceMult;
}