using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CameraFollowPlayerSpline : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer spline;
    [SerializeField] private Transform point;
    
    [Header("Properties")]
    [SerializeField] private float pointLerpSpeed = 5;
    [SerializeField] private int closestPointPrecision = 100;

    
    
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 closestPoint = GetClosestPointToPlayer();
        closestPoint.z = point.position.z;
        point.position = Vector3.Lerp(point.position, closestPoint, pointLerpSpeed * Time.deltaTime);
    }

    private Vector3 GetClosestPointToPlayer()
    {
        float minDistance = float.MaxValue;
        Vector2 closestPoint = (Vector3)spline.Spline.EvaluatePosition(0) + transform.position;

        for (int i = 1; i <= closestPointPrecision; i++)
        {
            float advancement = (float) i / closestPointPrecision;
            
            Vector2 newPosition = (Vector3)spline.Spline.EvaluatePosition(advancement) + transform.position;
            float distance = Vector3.Distance(newPosition, _player.position);
            
            if (!(distance < minDistance)) continue;
            
            minDistance = distance;
            closestPoint = newPosition;
        }

        return closestPoint;
    }
}
