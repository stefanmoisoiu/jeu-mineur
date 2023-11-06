using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PDebug : MonoBehaviour
{
    
    
    [Header("References")]
    [SerializeField] private GUIStyle style;
    
    [Header("Properties")]
    [SerializeField] private bool debug;
    [SerializeField] private Vector2 labelStartOffset = new (50,50);
    [SerializeField] private float labelYOffset = 0.5f;
    [SerializeField] private Vector2 labelSize = new (500,20);
    
    public delegate string DebugText();
    private List<DebugText> _debugTexts = new();
    
    private Camera _cam;
    
    public void AddDebugText(DebugText debugText)
    {
        _debugTexts.Add(debugText);
    }
    public void RemoveDebugText(DebugText debugText)
    {
        _debugTexts.Remove(debugText);
    }

    private void OnGUI()
    {
        if (!debug) return;
        if (_cam == null) _cam = Camera.main;
        Vector2 screenLabelOffset = Vector2.up * labelYOffset;
        for (int i = 0; i < _debugTexts.Count; i++)
        {
            string debugText = _debugTexts[i]?.Invoke();
            
            Vector3 screenPosition = labelStartOffset + screenLabelOffset * i;
            GUI.Label(new Rect(screenPosition.x,screenPosition.y,labelSize.x,labelSize.y), debugText,style);
        }
    }
}