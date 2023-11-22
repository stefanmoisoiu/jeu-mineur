#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class Sketcher : EditorWindow
{
    private string _layout = "Sketcher Layout.uxml";
    private VisualElement _drawButton, _eraseButton;
    private ScriptableSketch _selectedSketch;
    private enum State
    {
        Draw,Erase,None
    }
    private State _currentState = State.None;
    [MenuItem("Window/Sketcher")]
    public static void ShowWindow()
    {
        GetWindow<Sketcher>("Sketcher Tool");
    }

    public void CreateGUI()
    { 
        VisualTreeAsset layout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"Assets/Core/Sketcher/Visual/{_layout}");
        layout.CloneTree(rootVisualElement);
        _drawButton = rootVisualElement.Q<Button>("Draw");
        _eraseButton = rootVisualElement.Q<VisualElement>("Erase");
        
        _drawButton.RegisterCallback<MouseUpEvent>(evt => DrawButton());
        _eraseButton.RegisterCallback<MouseUpEvent>(evt => EraseButton());
        
        ObjectField sketchField = new ObjectField("Sketch");
        sketchField.objectType = typeof(ScriptableSketch);
        sketchField.RegisterValueChangedCallback(evt =>
        {
            ScriptableSketch sketch = (ScriptableSketch)evt.newValue;
            Debug.Log("Selected " + sketch);
            _selectedSketch = sketch;
        });
        rootVisualElement.Add(sketchField);
    }

    private void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

    private void OnSceneGUI(SceneView sceneView)
    {
        if (_selectedSketch == null) return;
        switch (_currentState)
        {
            case State.Draw:
                Draw(sceneView);
                break;
            case State.Erase:
                Erase(sceneView);
                break;
        }
    }
    private void Draw(SceneView sceneView)
    {
        _selectedSketch.texture.SetPixel(0,0,Color.red);
    }
    
    private void Erase(SceneView sceneView)
    {
        
    }
    private void DisplaySketch()
    {
        
    }
    

    private void DrawButton()
    {
        Debug.Log("Draw Button");

        if (_currentState == State.Draw)
        {
            _currentState = State.None;
            return;
        }
        _currentState = State.Draw;
    }
    private void EraseButton()
    {
        Debug.Log("Erase Button");
        if (_currentState == State.Erase)
        {
            _currentState = State.None;
            return;
        }
        _currentState = State.Erase;
    }
}

#endif