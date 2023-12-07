using UnityEngine;

public class PDamageParticles : MonoBehaviour
{
    [SerializeField] private PDamage damage;
    [SerializeField] private ParticleSystem particles;

    private void Start()
    {
        damage.OnDamage += () => particles.Play();
    }
}