using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FallingPlatform : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform platform;
    [SerializeField] private Collider2D[] platformColliders;
    
    
    [Header("Falling Platform Properties")]
    [SerializeField] private float fallPrepareDuration = 1;
    [SerializeField] private float fallDistance = 5;
    [SerializeField] private float fallDuration = 1;
    [SerializeField] private AnimationCurve fallCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [Space] [SerializeField] private bool fallCollisions;


    [SerializeField] private string groundLayer,movingGroundLayer;
    
    
    [Space]
    
    [SerializeField] private bool respawn = true;
    [SerializeField] private float platformRespawnTime = 2;

    private Coroutine _fallCoroutine;
    private bool _fallCoroutineActive = false;
    
    public Action OnFallPrepare, OnStartFall, OnEndFall, OnRespawn;
    public Action<float> OnFallPrepareProgress, OnFallProgress, OnRespawnProgress;
    public UnityEvent UnityOnFallPrepare, UnityOnStartFall, UnityOnEndFall, UnityOnRespawn;

    private void Start()
    {
        platform.gameObject.layer =  LayerMask.NameToLayer(groundLayer);
    }

    public void Fall()
    {
        if (_fallCoroutineActive) return;
        _fallCoroutine = StartCoroutine(FallCoroutine());
    }

    private IEnumerator FallCoroutine()
    {
        _fallCoroutineActive = true;
        
        OnFallPrepare?.Invoke();
        UnityOnFallPrepare?.Invoke();

        
        float fallPrepareTimer = 0;
        while (fallPrepareTimer < fallPrepareDuration)
        {
            fallPrepareTimer += Time.deltaTime;
            OnFallPrepareProgress?.Invoke(fallPrepareTimer / fallPrepareDuration);
            yield return null;
        }
        
        OnStartFall?.Invoke();
        UnityOnStartFall?.Invoke();
        
        platform.gameObject.layer = LayerMask.NameToLayer(movingGroundLayer);
        if(!fallCollisions) foreach (Collider2D col in platformColliders) col.enabled = false;
        
        float fallTimer = 0;
        while (fallTimer < fallDuration)
        {
            fallTimer += Time.deltaTime;
            platform.localPosition = Vector3.Lerp(Vector3.zero,Vector3.down * fallDistance,fallCurve.Evaluate(fallTimer / fallDuration));
            OnFallProgress?.Invoke(fallTimer / fallDuration);
            yield return null;
        }
        OnEndFall?.Invoke();
        UnityOnEndFall?.Invoke();

        platform.localPosition = Vector3.zero;
        if(!fallCollisions) foreach (Collider2D col in platformColliders) col.enabled = true;
        platform.gameObject.SetActive(false);
        
        
        if (respawn)
        {
            float respawnTimer = 0;
            while (respawnTimer < platformRespawnTime)
            {
                respawnTimer += Time.deltaTime;
                OnRespawnProgress?.Invoke(respawnTimer / platformRespawnTime);
                yield return null;
            }
            
            OnRespawn?.Invoke();
            UnityOnRespawn?.Invoke();
            platform.gameObject.SetActive(true);
            _fallCoroutineActive = false;
        }
        
        platform.gameObject.layer = LayerMask.NameToLayer(groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + Vector3.down * fallDistance);
    }
}
