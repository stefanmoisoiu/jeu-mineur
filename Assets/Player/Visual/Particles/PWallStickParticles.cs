using UnityEngine;

public class PWallStickParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private PWallStick wallStick;

    private void Start()
    {
        wallStick.OnWallStick += (Transform _, Vector3 _) => particles.Play();
    }
}
