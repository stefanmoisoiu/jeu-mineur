using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
[ExecuteInEditMode]
public class ColliderSpline : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer spline;
    [SerializeField] private EdgeCollider2D edgeCollider2D;
    
    [Header("Properties")]
    [SerializeField] private int precision = 100;
    [SerializeField] private float width = 1;

    private void OnEnable()
    {
        Spline.Changed += SplineUpdate;
    }
    private void OnDisable()
    {
        Spline.Changed -= SplineUpdate;
    }

    private void OnValidate()
    {
        UpdateCollider();
    }

    private void SplineUpdate(Spline spline1, int i, SplineModification arg3) => UpdateCollider();
    private void UpdateCollider()
    {
        if (spline == null) return;
        if(spline.Splines.Count == 0) return;
        if(spline.Spline.Knots.Count() < 2) return;
        
        List<Vector2> points = new List<Vector2>();
        for(int i = 0; i < precision; i++)
        {
            float t = (float)i / (precision - 1);
            spline.Evaluate(0, t, out float3 position, out float3 forward, out float3 up);
            float3 right = Vector3.Cross(forward, up);
            right = new Vector3(right.x, right.y, right.z).normalized;
            Vector3 localPosition = transform.InverseTransformPoint(position);
            points.Add(localPosition + (Vector3)right * width);
        }
        for(int i = precision - 1; i >= 0; i--)
        {
            float t = (float)i / (precision - 1);
            spline.Evaluate(0, t, out float3 position, out float3 forward, out float3 up);
            float3 right = Vector3.Cross(forward, up);
            right = new Vector3(right.x, right.y, right.z).normalized;
            Vector3 localPosition = transform.InverseTransformPoint(position);
            points.Add(localPosition - (Vector3)right * width);
        }
        edgeCollider2D.points = points.ToArray();
    }
}
