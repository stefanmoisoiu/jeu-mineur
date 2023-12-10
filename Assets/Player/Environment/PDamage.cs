using System;
using UnityEngine;

public class PDamage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PUncontrollable uncontrollable;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PFlashEffect flashEffect;
    
    
    
    [Header("Check Properties")] [SerializeField]
    private float checkDistance = 0.5f;
    [SerializeField] private LayerMask checkMask;
    [SerializeField] private int checkRayCount = 10;
    [SerializeField] [Range(0,1)] private float addedVelMult = 0.5f;
    [SerializeField] private float damageCooldownTime = 0.1f;
    private bool _canDamage = true;
    

    [Header("Bounce Properties")]
    [SerializeField] private Vector2 bounceForce;
    
    [Header("Audio")]
    [SerializeField] private ScriptableSFX damageSFX;
    
    [Header("Camera Shake")]
    [SerializeField] private ScriptableCameraShake cameraShake;

    

    
    
    [Header("Debug")]
    [SerializeField] private bool debug;

    public Action OnDamage;
    private void Update()
    {
        RaycastHit2D? hit = CheckDamage(out bool success);
        if(success) ApplyDamage(hit.Value);
    }

    private RaycastHit2D? CheckDamage(out bool success)
    {
        success = false;
        // if (stateManager.CurrentState == PStateManager.State.UncontrollableFall) return null;
        for (int i = 0; i < checkRayCount; i++)
        {
            float angle = i * (360f / checkRayCount);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, checkDistance, checkMask);
            if (hit.collider == null) continue;
            success = true;
            return hit;
        }
        return null;
    }

    private void ApplyDamage(RaycastHit2D hit)
    {
        if(!_canDamage) return;
        float xVel = bounceForce.x;
        if(Mathf.Abs(hit.normal.x) > 0.1f) xVel *= Mathf.Sign(hit.normal.x);
        else xVel *= Mathf.Sign(rb.velocity.x);
        
        float yVel = hit.normal.y > -.25f ? bounceForce.y : -bounceForce.y;
        float mag = rb.velocity.magnitude * addedVelMult;

        Vector2 vel = new (xVel, yVel);
        
        rb.velocity = vel + vel.normalized * mag;
        
        uncontrollable.StartUncontrollable();
        flashEffect.Flash(1f);
        damageSFX.Play();

        _canDamage = false;
        Invoke(nameof(ResetCanDamage),damageCooldownTime);
        
        cameraShake.Shake();
        
        OnDamage?.Invoke();
    }

    private void ResetCanDamage() => _canDamage = true;

    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.red;
        for(int i = 0; i < checkRayCount; i++)
        {
            float angle = i * (360f / checkRayCount);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            Gizmos.DrawRay(transform.position, dir * checkDistance);
        }
    }
}