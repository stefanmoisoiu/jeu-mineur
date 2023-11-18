using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    private static readonly int Value = Shader.PropertyToID("_Value");

    private void Update()
    {
        foreach (Material material in materials)
            material.SetFloat(Value,transform.position.x);
    }
}
