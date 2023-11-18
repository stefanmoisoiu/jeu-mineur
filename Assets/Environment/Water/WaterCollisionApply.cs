using UnityEngine;

public class WaterCollisionApply : MonoBehaviour
{
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private WaterSplash waterSplash;

    [Space] [SerializeField] [Range(0,1)] private float velocityForceMult = .5f;
    [SerializeField] private float defaultForce = 2.5f;
    [SerializeField] private int defaultInfluenceSize = 2;
    [SerializeField] private float influenceSizeMult = 2;
    
    private void Start()
    {
        colliderEvents.OnTriggerEnterValue += CollideWater;
    }

    private void CollideWater(Collider2D collidingObject)
    {
        Vector3 position = collidingObject.transform.position;
        if(collidingObject.TryGetComponent(out Rigidbody2D rb))
        {
            float force = rb.velocity.magnitude * velocityForceMult;
            int influenceSize = Mathf.CeilToInt(force * influenceSizeMult) + 1;
            waterSplash.Splash(position, -force, influenceSize);
        }
        else
        {
            waterSplash.Splash(position, -defaultForce, defaultInfluenceSize);
        }
    }
}
