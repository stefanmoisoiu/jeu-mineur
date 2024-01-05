using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

public class PipeStretchMat : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material pipeMat;
    
    
    private static readonly int SplineLength = Shader.PropertyToID("_SplineLength");
    private static readonly int StretchAdvancement = Shader.PropertyToID("_StretchAdvancement");
    private static readonly int StretchWidth = Shader.PropertyToID("_StretchWidth");
    

    private void Start()
    {
        meshRenderer.material = new Material(pipeMat);
        UpdateLength();
        SetStretchAdvancement(0);
    }

    [Button("Manual Update")]
    private void UpdateLength() =>
        meshRenderer.material.SetFloat(SplineLength, splineContainer.Spline.GetLength());
    
    public void SetStretchAdvancement(float strechAdvancement) =>
        meshRenderer.material.SetFloat(StretchAdvancement, strechAdvancement);
}