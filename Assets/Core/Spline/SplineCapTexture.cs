using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineCapTexture : MonoBehaviour
{
    public enum CapObjectType
    {
        Same,
        Different
    }
    [Header("References")]
    [SerializeField] private CapObjectType capObjectType;
    [ShowIf("@capObjectType == CapObjectType.Same")][SerializeField] private GameObject edgeObject;
    [ShowIf("@capObjectType == CapObjectType.Different")][SerializeField] private GameObject edgeObjectStart, edgeObjectEnd;
    [SerializeField] private SplineContainer spline;
    [SerializeField] [HideInInspector] private GameObject _capObjectStart, _capObjectEnd;
    
    [Header("Properties")]
    [SerializeField] private bool rotateWithSpline = true;
    [SerializeField] private float addedRotation;
    [SerializeField] private float addedDistance;

    #if UNITY_EDITOR
    private void OnEnable()
    {
        Spline.Changed += UpdateCapSpline;
    }

    private void OnDisable()
    {
        Spline.Changed -= UpdateCapSpline;
    }
    #endif

    private void UpdateCapSpline(Spline arg1, int arg2, SplineModification arg3) => UpdateCap();

    [Button("Manual Update")]
    private void UpdateCap()
    {
        if(_capObjectStart != null) DestroyImmediate(_capObjectStart);
        if(_capObjectEnd != null) DestroyImmediate(_capObjectEnd);
        
        if(spline == null) return;
        if(spline.Spline.Knots.Count() < 2) return;
        if(spline.Spline.Closed) return;
        
        spline.Evaluate(0, out float3 startPosition, out float3 startTangent, out _);
        spline.Evaluate(1, out float3 endPosition, out float3 endTangent, out _);

        _capObjectStart = Instantiate(capObjectType == CapObjectType.Same ? edgeObject : edgeObjectStart, transform);

        _capObjectStart.name = "Start Cap";
        _capObjectStart.transform.position = startPosition - startTangent * addedDistance;
        if (rotateWithSpline)
        {
            _capObjectStart.transform.up = -startTangent;
            _capObjectStart.transform.Rotate(0, 0, addedRotation);
        }
        _capObjectEnd = Instantiate(capObjectType == CapObjectType.Same ? edgeObject : edgeObjectEnd, transform);
        
        _capObjectEnd.name = "End Cap";
        _capObjectEnd.transform.position = endPosition + endTangent * addedDistance;
        if (rotateWithSpline)
        {
            _capObjectEnd.transform.up = endTangent;
            _capObjectEnd.transform.Rotate(0, 0, addedRotation);
        }
    }
}