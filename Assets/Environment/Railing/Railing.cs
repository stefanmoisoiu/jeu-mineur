using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Railing : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speedMultiplier = 1;
    [SerializeField] [Range(0,1)] private float slowingDownMult = .4f;
    [SerializeField] private bool detachOnEnd;
    public bool DetachOnEnd => detachOnEnd;
    

    public float GetSpeed(float t,float currentVelocity)
    {
        spline.Spline.Evaluate(t, out float3 position, out float3 forward, out float3 up);
        float speed = Vector3.Dot(Vector3.down, ((Vector3)forward).normalized) * speedMultiplier;
        // speed = Mathf.Clamp01(speed + t) - t;
        if ((int)Mathf.Sign(speed) != (int)Mathf.Sign(currentVelocity)) speed *= slowingDownMult;
        return speed / 50;
    }
    public float GetClosestPoint(Vector3 from,int precision = 100)
    {
        float closestPoint = 0;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < precision; i++)
        {
            float t = (float)i / precision;
            Vector3 position = GetPosition(t);
            float distance = Vector3.Distance(from, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = t;
            }
        }
        return closestPoint;
    }
    public float RailingToWorldVelocity(float vel)
    {
        return vel * spline.CalculateLength() * 400;
    }
    public float WorldToRailingVelocity(float vel)
    {
        return vel / spline.CalculateLength() / 400;
    }
    public Vector3 GetForward(float t)
    {
        spline.Spline.Evaluate(t, out float3 position, out float3 forward, out float3 up);
        return ((Vector3)forward).normalized;
    }
    public Vector3 GetPosition(float t)
    {
        spline.Spline.Evaluate(t, out float3 position, out float3 forward, out float3 up);
        return (Vector3)position + transform.position;
    }
}
