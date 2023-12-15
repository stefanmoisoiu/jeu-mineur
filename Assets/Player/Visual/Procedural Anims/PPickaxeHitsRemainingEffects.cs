using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PPickaxeHitsRemainingEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private SpriteRenderer circleLight;
    [SerializeField] private Light2D light2D;
    [Header("Lerp Properties")]
    [SerializeField] private float lerpDuration = 0.5f;
    [SerializeField] private AnimationCurve lerpCurve;
    
    private Coroutine _lerpCoroutine;
    private float _baseLightIntensity;
    private int _previousHitsRemaining;

    private void Start()
    {
        _previousHitsRemaining = pickaxe.MaxHits;
        _baseLightIntensity = light2D.intensity;
    }

    private void OnEnable()
    {
        pickaxe.OnPickaxeUsed += PlayHitsRemainingLerp;
        pickaxe.OnPickaxeReset += PlayLerpReset;
    }

    private void OnDisable()
    {
        pickaxe.OnPickaxeUsed -= PlayHitsRemainingLerp;
        pickaxe.OnPickaxeReset -= PlayLerpReset;
    }

    private void PlayLerpReset()
    {
        if (_lerpCoroutine != null)
            StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpCoroutine(pickaxe.MaxHits));
    }
    private void PlayHitsRemainingLerp(int hitsRemaining)
    {
        if (_lerpCoroutine != null)
            StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpCoroutine(hitsRemaining));
    }

    private IEnumerator LerpCoroutine(int hitsRemaining)
    {
        float previousHitsRemainingPercent = (float)_previousHitsRemaining / pickaxe.MaxHits;
        float hitsRemainingPercent = (float)hitsRemaining / pickaxe.MaxHits;
        
        _previousHitsRemaining = hitsRemaining;
        
        float advancement = 0f;
        while (advancement < 1f)
        {
            advancement += Time.deltaTime / lerpDuration;
            float lerpValue = lerpCurve.Evaluate(advancement);
            float currentHitsRemainingPercent = Mathf.Lerp(previousHitsRemainingPercent, hitsRemainingPercent, lerpValue);
            SetAtAdvancement(currentHitsRemainingPercent);
            yield return null;
        }
    }
    private void SetAtAdvancement(float advancement)
    {
        circleLight.color = new Color(circleLight.color.r, circleLight.color.g, circleLight.color.b, advancement);
        light2D.intensity = _baseLightIntensity * Mathf.Lerp(0.5f, 1f, advancement);
    }
}
