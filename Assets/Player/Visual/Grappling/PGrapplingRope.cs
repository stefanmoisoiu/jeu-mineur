using System;
using System.Collections;
using UnityEngine;

public class PGrapplingRope : MovementState
{
    [Header("References")]
    [SerializeField] private PGrappling grappling;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform ropeStartPoint;
    
    
    [Header("Wave Properties")]
    [SerializeField] private AnimationCurve waveAffectCurve;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float waveSpeed;
    [SerializeField] private float startWaveEffectLength = 0.25f, endWaveEffectLength = 0.1f;
    [SerializeField] private AnimationCurve ropeLengthCurve;

    
    
    [Header("Rope Properties")]
    [SerializeField] private int precision = 40;
    
    private Coroutine _ropeCoroutine;

    private void Start()
    {
        lineRenderer.enabled = false;
    }

    protected override void OnStateEnter()
    {
        if(_ropeCoroutine != null) StopCoroutine(_ropeCoroutine);
        _ropeCoroutine = StartCoroutine(StartRopeCoroutine());
    }

    protected override void OnStateExit()
    {
        if(_ropeCoroutine != null) StopCoroutine(_ropeCoroutine);
        _ropeCoroutine = StartCoroutine(EndRopeCoroutine());
    }

    protected override void ActiveStateUpdate()
    {
        lineRenderer.SetPosition(0, ropeStartPoint.position);
        lineRenderer.SetPosition(1, grappling.AttachedGrapplePoint.GrappleRopePoint.transform.position);
    }

    private IEnumerator StartRopeCoroutine()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = precision;
        float time = 0;
        while (time < startWaveEffectLength)
        {
            time += Time.deltaTime;
            float timeAdvancement = time / startWaveEffectLength;
            for (int i = 1; i < precision; i++)
            {
                float precisionAdvancement = (float)(i + 1) / precision;
                SetRopeWavePosition(i, time, timeAdvancement, precisionAdvancement);
            }
            lineRenderer.SetPosition(0, ropeStartPoint.position);
            yield return null;
        }
        lineRenderer.positionCount = 2;
    }

    private IEnumerator EndRopeCoroutine()
    {
        lineRenderer.positionCount = precision;
        float time = endWaveEffectLength;
        while (time > 0)
        {
            time -= Time.deltaTime;
            float timeAdvancement = time / endWaveEffectLength;
            for (int i = 1; i < precision; i++)
            {
                float precisionAdvancement = (float)(i + 1) / precision;
                SetRopeWavePosition(i,time, timeAdvancement, precisionAdvancement);
            }
            lineRenderer.SetPosition(0, ropeStartPoint.position);
            yield return null;
        }
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }
    
    private void SetRopeWavePosition(int index,float time, float timeAdvancement, float precisionAdvancement)
    {
        float waveValue = Mathf.Sin(2 * Mathf.PI * frequency * (precisionAdvancement * -waveSpeed) * timeAdvancement) * amplitude;
        waveValue *= waveAffectCurve.Evaluate(precisionAdvancement);
        waveValue *= 1 - timeAdvancement;

        Vector3 startPos = ropeStartPoint.position;
        Vector3 endPos = grappling.AttachedGrapplePoint.GrappleRopePoint.transform.position;
        Vector3 position = Vector3.Lerp(startPos, Vector3.Lerp(startPos, endPos, precisionAdvancement), ropeLengthCurve.Evaluate(timeAdvancement));
        position += (Vector3)grappling.GetPerpendicularDirectionToPoint() * waveValue;
                
        lineRenderer.SetPosition(index, position);
    }
}