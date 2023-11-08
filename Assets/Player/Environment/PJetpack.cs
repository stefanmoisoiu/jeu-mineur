using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PJetpack : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private Slider jetpackSlider;
    [SerializeField] private GameObject jetpackGraphics;
    [SerializeField] private AudioSource jetpackAudioSource;
    [SerializeField] private ParticleSystem jetpackParticles;
    
    
    
    
    [Header("Properties")]
    [SerializeField] private float maxYVelocity = 5f;
    [SerializeField] private float minYVelocity = -1f;
    
    
    
    private Jetpack _jetpack;
    private float _currentTankCapacitySeconds;

    protected override void OnStateEnter()
    {
        colliderEvents.OnTriggerEnterValue += CheckJetpackPickup;
    }

    protected override void OnStateExit()
    {
        colliderEvents.OnTriggerEnterValue -= CheckJetpackPickup;
        RemoveJetpack();
    }

    protected override void ActiveStateUpdate()
    {
        if (_jetpack == null) return;
        
        if (grounded.IsGrounded)
        {
            RemoveJetpack();
            return;
        }
        jetpackSlider.value = _currentTankCapacitySeconds;
        
        jetpackAudioSource.mute = !inputManager.Jump;
        if (inputManager.Jump && !jetpackParticles.isPlaying) jetpackParticles.Play();
        if (!inputManager.Jump && jetpackParticles.isPlaying) jetpackParticles.Stop();
        
        if (!inputManager.Jump) return;
        _currentTankCapacitySeconds -= Time.deltaTime;
        
        if (_currentTankCapacitySeconds <= 0) RemoveJetpack();
    }
    protected override void ActiveStateFixedUpdate()
    {
        if (_jetpack == null) return;
        if (!inputManager.Jump) return;
        
        rb.velocity = new Vector2(rb.velocity.x,Mathf.Clamp(rb.velocity.y,minYVelocity,maxYVelocity));
        rb.AddForce(Vector2.up * _jetpack.Force,ForceMode2D.Force);
    }

    private void CheckJetpackPickup(Collider2D obj)
    {
        if (!obj.TryGetComponent(out Jetpack newJetpack)) return;
        _jetpack = newJetpack;
        
        _currentTankCapacitySeconds = _jetpack.TankCapacitySeconds;
        
        jetpackSlider.maxValue = _jetpack.TankCapacitySeconds;
        jetpackSlider.value = _jetpack.TankCapacitySeconds;
        
        _jetpack.Pickup();
        
        jetpackSlider.gameObject.SetActive(true);
        jetpackGraphics.SetActive(true);
    }

    private void RemoveJetpack()
    {
        _jetpack = null;
            
        jetpackSlider.gameObject.SetActive(false);
        jetpackGraphics.SetActive(false);
        jetpackParticles.Stop();
        jetpackAudioSource.mute = true;
    }
}
