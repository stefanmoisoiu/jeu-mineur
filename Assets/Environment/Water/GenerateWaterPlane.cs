using Sirenix.OdinInspector;
using UnityEngine;

public class GenerateWaterPlane : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private MeshFilter meshFilter;
    
    [Header("Settings")]
    [SerializeField] private int precision = 10;
    [SerializeField] private Vector2 size;
    public Vector2 Size => size;

    [Button]
    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        int vertexCount = (precision + 1) * 2;
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[precision * 6];

        for (int i = 0; i <= precision; i++)
        {
            float x = size.x * i / precision - size.x / 2;
            vertices[i * 2] = new Vector3(x, -size.y / 2, 0);
            vertices[i * 2 + 1] = new Vector3(x, size.y / 2, 0);

            float uvX = (float)i / precision;
            uv[i * 2] = new Vector2(uvX, 0);      // Adjusted to start from bottom
            uv[i * 2 + 1] = new Vector2(uvX, 1);  // Adjusted to end at top
        }

        for (int i = 0; i < precision; i++)
        {
            int baseIndex = i * 6;
            int vertexIndex = i * 2;

            triangles[baseIndex] = vertexIndex;
            triangles[baseIndex + 1] = vertexIndex + 1;
            triangles[baseIndex + 2] = vertexIndex + 2;
            triangles[baseIndex + 3] = vertexIndex + 1;
            triangles[baseIndex + 4] = vertexIndex + 3;
            triangles[baseIndex + 5] = vertexIndex + 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
        
        Debug.Log("Generated mesh");
    }
}