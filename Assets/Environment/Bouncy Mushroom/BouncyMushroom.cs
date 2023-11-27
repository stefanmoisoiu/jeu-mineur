using System;
using UnityEngine;

public class BouncyMushroom : MonoBehaviour
{
    [Header("Bounce Properties")]
    [SerializeField] private float force;

    public float Force => force;
    
    [Header("Visual")]
    [SerializeField] private ProceduralAnimation bounceAnimation;
    
    [Header("Audio")]
    [SerializeField] private ScriptableSFX bounceSFX;

    

    
    
    [Header("Preview")]
    [SerializeField] private bool preview;
    [SerializeField] private float previewDistance;
    [SerializeField] private int previewPointsPerUnit = 1;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Rigidbody2D rb)) return;
        // rb.velocity = transform.up * force;
        bounceAnimation.StopAnimation(this);
        bounceAnimation.StartAnimation(this);
        bounceSFX.Play();
    }

    private void OnDrawGizmos()
    {
        if (!preview) return;
        Vector3 position = transform.position + transform.up * 0.5f;
        Vector3 direction = transform.up;
        Gizmos.color = Color.green;
        
        // draw line showing trajectory using gravity
        Vector3 velocity = direction * force;
        Vector3 lastPoint = position;
        for (int i = 0; i < previewDistance * previewPointsPerUnit; i++)
        {
            Vector3 nextPoint = lastPoint + velocity / previewPointsPerUnit;
            Gizmos.DrawLine(lastPoint,nextPoint);
            lastPoint = nextPoint;
            velocity += Physics.gravity * 2 / previewPointsPerUnit;
        }
    }
}
