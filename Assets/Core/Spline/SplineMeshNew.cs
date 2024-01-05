using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMeshNew : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float width = 1;
    [SerializeField] private int precision = 100;

    [Button]
    private void UpdateMesh()
    {
        meshFilter.mesh = spline.Spline.CreateMesh(width, precision);
    }
}