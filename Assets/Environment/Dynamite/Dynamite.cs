using System;
using System.Collections;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField] private GameObject graphics;
    
    [SerializeField] private Transform attachPoint;
    public Transform AttachPoint => attachPoint;
    
    [SerializeField] private ParticleSystem moveParticles;
    [SerializeField] private ParticleSystem endParticles;

    [SerializeField] private BoxCollider2D collider2D;
    
    
    private Vector3 _startPosition;
    
    [Header("Jetpack Properties")]
    [SerializeField] private float distance = 10;
    [SerializeField] private float moveLength = 1;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float respawnTime = 1;
    [SerializeField] private LayerMask collisionLayer;
    
    
    
    [Header("Audio")]
    [SerializeField] private ScriptableSFX startSFX;
    [SerializeField] private ScriptableSFX endSFX;
    
    [Header("Animations")]
    [SerializeField] private ProceduralAnimation moveAnimation, endAnimation, respawnAnimation;

    public Action OnExplode;
    
    private Coroutine _moveCoroutine;

    private float advancement = 0;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void StartJetpack()
    {
        moveParticles.Play();
        startSFX.Play();
        respawnAnimation.StopAnimation(this);
        endAnimation.StopAnimation(this);
        moveAnimation.StartAnimation(this);
    }
    private void End()
    {
        moveParticles.Stop();
        endParticles.Play();
        endSFX.Play();
        
        moveAnimation.StopAnimation(this);
        respawnAnimation.StopAnimation(this);
        endAnimation.StartAnimation(this);
    }
    private void Respawn()
    {
        endParticles.Stop();
        
        moveAnimation.StopAnimation(this);
        endAnimation.StopAnimation(this);
        respawnAnimation.StartAnimation(this);
    }

    public void Use()
    {
        if(_moveCoroutine != null)
        {
            if (advancement < 1) return;
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(UseCoroutine());
    }
    private IEnumerator UseCoroutine()
    {
        StartJetpack();
        advancement = 0;
        while (advancement < 1)
        {
            advancement += Time.deltaTime / moveLength;

            if (Physics2D.BoxCast(transform.TransformPoint(collider2D.offset), collider2D.size,
                    transform.eulerAngles.z, transform.right, 0, collisionLayer).collider != null)
                break;
            
            transform.position = _startPosition + transform.up * curve.Evaluate(advancement) * distance;
            yield return null;
        }
        advancement = 1;
        End();
        
        endAnimation.OnAnimationEnd += () => graphics.SetActive(false);
        collider2D.enabled = false;
        
        OnExplode?.Invoke();
        
        yield return new WaitForSeconds(respawnTime);
        
        endAnimation.Reset();
        collider2D.enabled = true;
        graphics.SetActive(true);
        
        transform.position = _startPosition;
        
        Respawn();
    }

    public Vector2 GetCurrentVelocity()
    {
        return transform.right * curve.Evaluate(advancement) * distance / moveLength;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * distance);
    }
}
