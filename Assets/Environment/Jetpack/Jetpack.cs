using System.Collections;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D collider2D;

    
    
    [Header("Properties")]
    [SerializeField] private float force = 5f;
    public float Force => force;
    [SerializeField] private float tankCapacitySeconds = 3;
    public float TankCapacitySeconds => tankCapacitySeconds;
    [SerializeField] private float respawnTime = 1;
    
    [Header("Audio")]
    [SerializeField] private ScriptableSFX pickupSFX;
    
    [Header("Animations")]
    [SerializeField] private ProceduralAnimation idleAnimation, pickupAnimation, respawnAnimation;
    
    private Coroutine _respawnCoroutine;

    private void OnEnable()
    {
        idleAnimation.StartAnimation(this);
    }

    public void Pickup()
    {
        pickupSFX.Play();
        idleAnimation.StopAnimation(this);
        respawnAnimation.StopAnimation(this);
        pickupAnimation.StartAnimation(this);
        collider2D.enabled = false;
        
        if (_respawnCoroutine != null) StopCoroutine(_respawnCoroutine);
        _respawnCoroutine = StartCoroutine(RespawnCoroutine());
    }
    
    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);
        collider2D.enabled = true;
        
        respawnAnimation.Reset();
        respawnAnimation.Then(() => idleAnimation.StartAnimation(this));
        respawnAnimation.Then(() => respawnAnimation.Reset());
        
        respawnAnimation.StartAnimation(this);
    }
}
