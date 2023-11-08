using System.Collections;
using UnityEngine;

public class FallingPlatformGraphics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FallingPlatform fallingPlatform;
    [SerializeField] private SpriteRenderer graphics;

    [Header("Main Properties")]
    [SerializeField]
    private float respawnAnimationLength = 0.5f;

    
    
    [Header("Scaling")]
    [SerializeField] private bool prepareScaleFall = false;
    [SerializeField] private AnimationCurve prepareScaleFallCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Space]
    
    [SerializeField] private bool scaleFall = false;
    [SerializeField] private AnimationCurve scaleFallCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Space]
    
    [SerializeField] private bool scaleRespawn = false;
    [SerializeField] private AnimationCurve scaleRespawnCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Header("Rotation")]
    [SerializeField] private bool prepareRotateFall = false;
    [SerializeField] private float prepareRotateFallAmount = 90;
    [SerializeField] private AnimationCurve prepareRotateFallCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Space]
    
    [SerializeField] private bool rotateFall = false;
    [SerializeField] private float rotateFallAmount = 90;
    [SerializeField] private AnimationCurve rotateFallCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Space]
    
    [SerializeField] private bool rotateRespawn = false;
    [SerializeField] private float rotateRespawnAmount = 90;
    [SerializeField] private AnimationCurve rotateRespawnCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Header("Opacity")]
    
    [SerializeField] private bool prepareOpacityFall = false;
    [SerializeField] private AnimationCurve prepareOpacityFallCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Space]
    
    [SerializeField] private bool opacityFall = false;
    [SerializeField] private AnimationCurve opacityFallCurve = AnimationCurve.EaseInOut(0,1,1,0);
    
    [Space]
    
    [SerializeField] private bool opacityRespawn = false;
    [SerializeField] private AnimationCurve opacityRespawnCurve = AnimationCurve.EaseInOut(0,1,1,0);

    private void Start()
    {
        fallingPlatform.OnFallPrepareProgress += FallPrepareUpdate;
        fallingPlatform.OnFallProgress += FallUpdate;
        fallingPlatform.OnRespawn += Respawn;
    }

    private void FallPrepareUpdate(float advancement)
    {
        if (prepareScaleFall) graphics.transform.localScale = Vector3.one * prepareScaleFallCurve.Evaluate(advancement);
        
        if (prepareRotateFall) graphics.transform.localRotation = Quaternion.Euler(0,0,prepareRotateFallAmount * prepareRotateFallCurve.Evaluate(advancement));
        
        if (prepareOpacityFall) graphics.color = new Color(graphics.color.r, graphics.color.g, graphics.color.b, prepareOpacityFallCurve.Evaluate(advancement));
    }
    private void FallUpdate(float advancement)
    {
        if (scaleFall) graphics.transform.localScale = Vector3.one * scaleFallCurve.Evaluate(advancement);
        
        if (rotateFall) graphics.transform.localRotation = Quaternion.Euler(0,0,rotateFallAmount * rotateFallCurve.Evaluate(advancement));
        
        if (opacityFall) graphics.color = new Color(graphics.color.r, graphics.color.g, graphics.color.b, opacityFallCurve.Evaluate(advancement));
    }
    private void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }
    private IEnumerator RespawnCoroutine()
    {
        float timer = 0;
        while (timer < respawnAnimationLength)
        {
            timer += Time.deltaTime;
            float advancement = timer / respawnAnimationLength;
            
            if (scaleRespawn) graphics.transform.localScale = Vector3.one * scaleRespawnCurve.Evaluate(advancement);
        
            if (rotateRespawn) graphics.transform.localRotation = Quaternion.Euler(0,0,rotateFallAmount * rotateRespawnCurve.Evaluate(advancement));
        
            if (opacityRespawn) graphics.color = new Color(graphics.color.r, graphics.color.g, graphics.color.b, opacityRespawnCurve.Evaluate(advancement));
            yield return null;
        }
    }
}