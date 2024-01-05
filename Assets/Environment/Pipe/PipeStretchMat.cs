using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
public class PipeStretchMat : MonoBehaviour
{
    [SerializeField] private TexturedSpline texturedSpline;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material pipeMat;
    
    
    private static readonly int Length = Shader.PropertyToID("_Length");
    private static readonly int StretchPosition = Shader.PropertyToID("_StretchPosition");
    private static readonly int StretchWidth = Shader.PropertyToID("_StretchWidth");
    

    private void Start()
    {
        meshRenderer.material = new Material(pipeMat);
        UpdateLength();
        SetStretchPosition(0);
    }

    [Button("Manual Update")]
    private void UpdateLength() =>
        meshRenderer.material.SetFloat(Length, texturedSpline.MaxUVOffset);
    
    public void SetStretchPosition(float stretchPosition)
    {
        float stretchAdvancement = Mathf.Clamp01(stretchPosition / texturedSpline.MaxUVOffset);
        float addedPosition = (stretchAdvancement - 0.5f) * meshRenderer.material.GetFloat(StretchWidth) * 2;
        
        meshRenderer.material.SetFloat(StretchPosition, stretchPosition + addedPosition);
    }
}