using System;
using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Pipe : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer spline;
    public Spline Spline => spline.Spline;
    [SerializeField] private PipeStretchMat pipeStretchMat;
    [SerializeField] private CinemachineVirtualCamera pipeCam;
    [SerializeField] private Transform pipeCameraTarget;
    
    
    
    private Coroutine _moveStretchPositionCoroutine;

    public Action<Vector2> OnPipePositionChanged; // position
    public Action<Vector2,Vector2> OnPipeExited; // exit position, exit direction, exit speed

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) MoveStretchPosition(true, 10);
        if (Input.GetKeyDown(KeyCode.I)) MoveStretchPosition(false, 10);
    }

    public void MoveStretchPosition(bool forward, float speed)
    {
        if (_moveStretchPositionCoroutine != null) StopCoroutine(_moveStretchPositionCoroutine);
        _moveStretchPositionCoroutine = StartCoroutine(MoveStretchPositionCoroutine(forward,speed));
    }
    public void StopMovingStretchPosition()
    {
        if (_moveStretchPositionCoroutine != null) StopCoroutine(_moveStretchPositionCoroutine);
        pipeStretchMat.SetStretchAdvancement(0);
        pipeCam.Priority = -1000;
    }
    private IEnumerator MoveStretchPositionCoroutine(bool forward, float speed)
    {
        pipeCam.Priority = 2000;
        float advancement = 0;
        while (advancement < 1)
        {
            advancement += Time.deltaTime * speed / spline.Spline.GetLength();
            float dirAdvancement = forward ? advancement : 1 - advancement;
            
            pipeStretchMat.SetStretchAdvancement(dirAdvancement);
            
            Spline.Evaluate(dirAdvancement, out float3 currentPosition, out _, out _);
            Vector2 worldPosition = (Vector3)currentPosition + transform.position;
            
            pipeCameraTarget.position = worldPosition;
            
            OnPipePositionChanged?.Invoke(worldPosition);
            yield return null;
        }
        Spline.Evaluate(forward ? 1 : 0, out float3 exitPosition, out float3 exitDirection, out _);
        
        Vector2 exitDirection2D = ((Vector3)exitDirection).normalized * (forward ? 1 : -1);
        Vector2 exitPosition2D = (Vector3)exitPosition + transform.position;
        exitPosition2D += exitDirection2D * 0.5f;
        
        pipeCam.Priority = -1000;
        OnPipeExited?.Invoke(exitPosition2D, exitDirection2D);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (spline != null)
        {
            spline.Spline.Evaluate(0, out float3 startPosition, out float3 startTangent, out _);
            spline.Spline.Evaluate(1, out float3 endPosition, out float3 endTangent, out _);

            Vector3 sDir = -((Vector3)startTangent).normalized;
            Vector3 sPos = (Vector3)startPosition + transform.position + sDir * 0.5f;
            Gizmos.DrawWireSphere(sPos, 0.1f);
            Gizmos.DrawLine(sPos, sPos + sDir * 0.3f);
            
            Vector3 eDir = ((Vector3)endTangent).normalized;
            Vector3 ePos = (Vector3)endPosition + transform.position + eDir * 0.5f;
            Gizmos.DrawWireSphere(ePos, 0.1f);
            Gizmos.DrawLine(ePos, ePos + eDir * 0.3f);
        }
    }
}
