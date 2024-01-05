using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines; // Ensure the Spline package is correctly imported.

public static class SplineMesh
{
    public static Mesh CreateMesh(this Spline spline, float width, int precision)
    {
        Mesh mesh = new();

        Vector3[] vertices = new Vector3[precision * 2];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[(precision - 1) * 6];

        for (int i = 0; i < precision; i++)
        {
            float t = i / (precision - 1f);
            
            if(t == 0) t = 0.001f;
            if(t == 1) t = 0.999f;
            
            spline.Evaluate(t, out float3 float3Position, out float3 float3Forward, out _);
            
            Vector3 position = (Vector3)float3Position;
            Vector3 forward = (Vector3)float3Forward;
            
            // Calculate the normal outward and perpendicular to the spline direction
            Vector3 normal = Vector3.Cross(forward, Vector3.forward).normalized;

            vertices[i * 2] = position - normal * width * 0.5f; // Left vertex
            vertices[i * 2 + 1] = position + normal * width * 0.5f; // Right vertex

            // Set UVs based on the width and texture size
            float u = (float)i / precision;
            uv[i * 2] = new Vector2(u, 0);
            uv[i * 2 + 1] = new Vector2(u, 1);
            
            // Set normals
            normals[i * 2] = -normal;
            normals[i * 2 + 1] = normal;

            // Set triangles
            if (i < precision - 1)
            {
                int baseIndex = i * 2;
                triangles[i * 6] = baseIndex;
                triangles[i * 6 + 1] = baseIndex + 3;
                triangles[i * 6 + 2] = baseIndex + 1;
                triangles[i * 6 + 3] = baseIndex;
                triangles[i * 6 + 4] = baseIndex + 2;
                triangles[i * 6 + 5] = baseIndex + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.normals = normals;

        return mesh;
    }
}