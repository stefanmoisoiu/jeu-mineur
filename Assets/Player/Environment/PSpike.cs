using System;
using UnityEngine;

public class PSpike : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PUncontrollableFall uncontrollableFall;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Check Properties")] [SerializeField]
    private float checkDistance = 0.5f;
    [SerializeField] private LayerMask checkMask;
    [SerializeField] private int checkRayCount = 10;
    [SerializeField] private float checkRayInterval = 0.1f;
    [SerializeField] [Range(0,1)] private float addedVelMult = 0.5f;
    
    private float _checkRayTimer;

    [Header("Bounce Properties")]
    [SerializeField] private Vector2 bounceForce;
    
    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Update()
    {
        if (_checkRayTimer > 0)
        {
            _checkRayTimer -= Time.deltaTime;
            return;
        }
        _checkRayTimer = checkRayInterval;
        CheckSpikes();
    }

    private void CheckSpikes()
    {
        for (int i = 0; i < checkRayCount; i++)
        {
            float angle = i * (360f / checkRayCount);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, checkDistance, checkMask);
            if (hit.collider == null) continue;
            
            rb.velocity *= addedVelMult;
            rb.velocity += new Vector2(
                hit.normal.x > 0 ? bounceForce.x : -bounceForce.x,
                hit.normal.y > -.25f ? bounceForce.y : -bounceForce.y);
            
            uncontrollableFall.UncontrollableFall();
            return;
        }
    }

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