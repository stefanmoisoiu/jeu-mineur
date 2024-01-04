using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
public class PipeStretchMat : MonoBehaviour
{
    [SerializeField] private TexturedSpline texturedSpline;
    [SerializeField] private MeshRenderer meshRenderer;
    
    private static readonly int Length = Shader.PropertyToID("_Length");
    private static readonly int StretchPosition = Shader.PropertyToID("_StretchPosition");

    [Button("Manual Update")]
    private void UpdateLength() =>
        meshRenderer.sharedMaterial.SetFloat(Length, texturedSpline.MaxLength);
    
    public void SetStretchPosition(float stretchPosition) =>
        meshRenderer.sharedMaterial.SetFloat(StretchPosition, stretchPosition);
}