using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class PixelCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    
    [SerializeField] private int pixelHeight = 360;
    [SerializeField] private int pixelsPerUnit = 16;

    private void OnValidate()
    {
        UpdateSize();
    }

    [Button]
    public void UpdateSize()
    {
        float orthographicSize = pixelHeight / (float) pixelsPerUnit / 2;
        
        if(cam != null) cam.orthographicSize = orthographicSize;
        if(virtualCamera != null) virtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
    
}
