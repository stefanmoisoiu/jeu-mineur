using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
public class MovingPlatformsOnSpline : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer spline;
    [SerializeField] private GameObject movingPlatform;
    private List<GameObject> platformsInstances = new ();
    
    [Header("Main Properties")]
    [SerializeField] private int platformCount = 1;
    [SerializeField] private AnimationCurve platformAdvancementCurve = AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private MoveType platformMoveType = MoveType.Speed;
    [Space]
    [SerializeField] private float platformMoveDuration = 1f;
    [SerializeField] private float platformMoveSpeed = 1f;
    private enum MoveType
    {
        Speed,
        Duration
    }
    [Space]
    [SerializeField] [Range(0,1)] private float platformAddedAdvancement = 0;
    [SerializeField] private float platformAddedPosition = 0;
    
    private float _advancement;
    
    [Header("Moving Platform Properties")]
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;
    [SerializeField] private float yOffset;
    
    [Header("Moving Platform Rotate Properties")]
    [SerializeField] private bool rotateAlongSpline;
    [SerializeField] [Range(-90,90)] private float platformAddedRotation = 90f;
    
    
    [Header("Moving Platform Visual Properties")]
    [SerializeField] private bool flipPlatformX;
    

    [Header("Debug")]
    [SerializeField] private bool debug;
    private float _debugAdvancement;

    private void Start()
    {
        _advancement = 0;
        _advancement += platformAddedAdvancement;
        _advancement += platformAddedPosition / spline.Spline.GetLength();
        
        _advancement %= 1f;
        
        for (int i = 0; i < platformCount; i++)
            platformsInstances.Add(Instantiate(movingPlatform, transform));
    }

    private void Update()
    {
        if (spline == null) return;
        MovePlatform();
    }
    private void MovePlatform()
    {
        _advancement = GetNewAdvancement(_advancement,Time.deltaTime);
        float animatedAdvancement = _advancement;
        if(invertX)
            animatedAdvancement = 1 - animatedAdvancement;

        for (int i = 0; i < platformsInstances.Count; i++)
        {
            float mineCartAdvancement = GetMineCartAdvancement(animatedAdvancement, i);
            mineCartAdvancement = platformAdvancementCurve.Evaluate(mineCartAdvancement);
            
            spline.Evaluate(mineCartAdvancement, out float3 position, out float3 tangent, out _);

            Quaternion tangentRot = Quaternion.LookRotation(tangent, -Vector3.forward) * Quaternion.Euler(-90, 0, 0);
            Vector3 up = tangentRot * -Vector3.right;
            Vector3 mineCartPosition = (Vector3)position + up * yOffset;
            Quaternion rotation = rotateAlongSpline ? tangentRot * Quaternion.Euler((flipPlatformX ? 180 : 0),0,platformAddedRotation + (invertY ? 180 : 0)) : Quaternion.Euler(0,0,platformAddedRotation);
            
            platformsInstances[i].transform.position = mineCartPosition;
            platformsInstances[i].transform.rotation = rotation;
        }
    }
#if UNITY_EDITOR
    internal void MovePlatformEditor(float deltaTime)
    {
        if (!debug) return;
        if (spline == null) return;
        _debugAdvancement = GetNewAdvancement(_debugAdvancement,deltaTime);
        float animatedDebugAdvancement = _debugAdvancement;
        if(invertX)
            animatedDebugAdvancement = 1 - animatedDebugAdvancement;
        
        for (int i = 0; i < platformCount; i++)
        {
            float mineCartAdvancement = GetMineCartAdvancement(animatedDebugAdvancement, i);
            mineCartAdvancement = platformAdvancementCurve.Evaluate(mineCartAdvancement);
            
            spline.Evaluate(mineCartAdvancement, out float3 position, out float3 tangent, out float3 up);
            
            tangent = math.normalize(tangent);
            Vector3 mineCartPosition = position;
            Quaternion rotation = rotateAlongSpline ? Quaternion.LookRotation(tangent,-Vector3.forward) * Quaternion.Euler(-90,flipPlatformX ? 180 : 0,90 + (invertY ? 180 : 0)) : Quaternion.identity;
            
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.CubeHandleCap(0, mineCartPosition, rotation, 0.5f, EventType.Repaint);
        }
    }
#endif


    private float GetNewAdvancement(float currentAdvancement, float deltaTime)
    {
        switch (platformMoveType)
        {
            case MoveType.Speed: return (currentAdvancement + deltaTime * platformMoveSpeed / spline.Spline.GetLength()) % 1f;
            case MoveType.Duration: return (currentAdvancement + deltaTime / platformMoveDuration) % 1f;
        }
        return 0;
    }
        
    
    private float GetMineCartAdvancement(float currentAdvancement,int mineCartIndex) =>
        (currentAdvancement + 1f / platformCount * mineCartIndex) % 1f;
}