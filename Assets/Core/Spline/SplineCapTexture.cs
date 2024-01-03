#if UNITY_EDITOR
using System;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineCapTexture : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject edgeObject;
    [SerializeField] private SplineContainer spline;
    private GameObject _capObject1, _capObject2;
    
    [Header("Properties")]
    [SerializeField] private bool rotateWithSpline = true;
    [SerializeField] private float addedRotation;
    [SerializeField] private float addedDistance;

    private void OnEnable()
    {
        Spline.Changed += UpdateCapSpline;
    }

    private void OnDisable()
    {
        Spline.Changed -= UpdateCapSpline;
    }

    private void UpdateCapSpline(Spline arg1, int arg2, SplineModification arg3) => UpdateCap();

    [Button("Manual Update")]
    private void UpdateCap()
    {
        if(_capObject1 != null) DestroyImmediate(_capObject1);
        if(_capObject2 != null) DestroyImmediate(_capObject2);
        
        if(spline == null) return;
        if(spline.Spline.Knots.Count() < 2) return;
        if(spline.Spline.Closed) return;
        
        spline.Evaluate(0, out float3 startPosition, out float3 startTangent, out float3 startNormal);
        spline.Evaluate(1, out float3 endPosition, out float3 endTangent, out float3 endNormal);
        
        _capObject1 = Instantiate(edgeObject, transform);
        _capObject1.name = "Start Cap";
        _capObject1.transform.position = startPosition - startTangent * addedDistance;
        if (rotateWithSpline)
        {
            _capObject1.transform.up = -startTangent;
            _capObject1.transform.Rotate(0, 0, addedRotation);
        }
        
        _capObject2 = Instantiate(edgeObject, transform);
        _capObject2.name = "End Cap";
        _capObject2.transform.position = endPosition + endTangent * addedDistance;
        if (rotateWithSpline)
        {
            _capObject2.transform.up = endTangent;
            _capObject2.transform.Rotate(0, 0, addedRotation);
        }
    }
}
#endif