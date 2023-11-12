using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class TexturedSpline : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private SplineContainer spline;
    [Header("Properties")]
    [SerializeField] private float width = 1;
    [SerializeField] private int precision = 100;
    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private float sphereSize = 0.1f;

    float3[] leftVertices;
    float3[] rightVertices;

    private void OnEnable()
    {
        Spline.Changed += Rebuild;
    }

    private void OnDisable()
    {
        Spline.Changed -= Rebuild;
    }

    public void Rebuild(Spline spline1 = null, int i = 0, SplineModification arg3 = SplineModification.Default)
    {
        if (spline == null) return;
        if(spline.Splines.Count == 0) return;
        if(spline.Spline.Knots.Count() < 2) return;
        
        UpdatePoints();
        BuildMesh();
    }
    private void UpdatePoints()
    {
        leftVertices = new float3[precision];
        rightVertices = new float3[precision];
        
        for(int i = 0; i < precision; i++)
        {
            float t = (float)i / (precision - 1);
            spline.Evaluate(0, t, out float3 position, out float3 forward, out float3 up);
            float3 right = math.cross(forward, new float3(0,0,-1));
            right = new Vector3(right.x, right.y, right.z).normalized;
            float3 localPosition = transform.InverseTransformPoint(position);
            leftVertices[i] = localPosition + right * width;
            rightVertices[i] = localPosition - right * width;
        }
    }
    private void BuildMesh()
    {
        if (meshFilter == null) return;
        
        Mesh mesh = new Mesh();
        
        List<Vector3> verts = new();
        List<int> tris = new();
        List<Vector2> uvs = new();
        float uvOffset = 0;
        
        for(int i = 1; i < precision; i++)
        {
            Vector3 p1 = leftVertices[i-1];
            Vector3 p2 = rightVertices[i-1];
            Vector3 p3;
            Vector3 p4;

            if (i == precision && spline.Spline.Closed)
            {
                p3 = leftVertices[0];
                p4 = rightVertices[0];
            }
            else
            {
                p3 = leftVertices[i];
                p4 = rightVertices[i];
            }
            
            int offset = 4 * (i - 1);

            int t1 = offset;
            int t2 = offset + 2;
            int t3 = offset + 3;
            
            int t4 = offset + 3;
            int t5 = offset + 1;
            int t6 = offset;
            
            verts.AddRange(new []{p1,p2,p3,p4});
            tris.AddRange(new []{t1,t2,t3,t4,t5,t6});

            float distance = Vector3.Distance(p1, p3) / (width);
            float uvDistance = uvOffset + distance;
            
            uvs.AddRange(new []{new Vector2(uvOffset,0),new Vector2(uvOffset,1),new Vector2(uvDistance,0),new Vector2(uvDistance,1)});
            uvOffset += distance;
        }
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris,0);
        mesh.SetUVs(0,uvs);
        meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        if (spline == null) return;
        if (spline.Splines.Count == 0) return;
        if(spline.Spline.Knots.Count() < 2) return;
        
        Gizmos.color = Color.white;
        for (int i = 0; i < precision; i++)
        {
            Gizmos.DrawSphere(transform.position + (Vector3)leftVertices[i],sphereSize);
            Gizmos.DrawSphere(transform.position + (Vector3)rightVertices[i],sphereSize);
        }
    }
}
