using Cinemachine;
using UnityEngine;

public class PCameraFallLookAhead : PCameraComponent
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Camera Properties")]
    [SerializeField] private float fallLookAheadStart = 2f;
    [SerializeField] private float fallLookAheadEnd = 10f;
    [SerializeField] private float fallLookAhead = 5f;
    
    public override Vector3 GetOffset()
    {
        float lookAheadAdvancement =
            Mathf.Clamp01(Mathf.InverseLerp(fallLookAheadStart, fallLookAheadEnd, Mathf.Max(0, -rb.velocity.y)));
        return Vector3.Lerp(Vector3.zero, Vector3.down * fallLookAhead, lookAheadAdvancement);
    }
}
