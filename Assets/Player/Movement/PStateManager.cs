using System;
using UnityEditor;
using UnityEngine;

public class PStateManager : MonoBehaviour
{
    public enum State
    {
        Normal,
        WallStick,
        PickaxeDash,
        Railing,
        Slide,
        Grappling,
        BouncyMushroom,
        SlipperyMovement,
        Dynamite,
        UncontrollableFall,
    }
    public State CurrentState { get; private set; }
    
    public Action<State> OnStateChanged;

    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    
    private void Start()
    {
        SetState(State.Normal);
        _debugText = () => $"Current State: {CurrentState}";
        debug.AddDebugText(_debugText);
    }
    public void SetState(State newState)
    {
        OnStateChanged?.Invoke(newState);
        CurrentState = newState;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PStateManager))]
public class PStateManagerEditor : Editor
{
    // show the current state in the inspector
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PStateManager stateManager = (PStateManager)target;
        EditorGUILayout.LabelField("Current State", stateManager.CurrentState.ToString());
    }

    private void OnEnable()
    {
        PStateManager stateManager = (PStateManager)target;
        stateManager.OnStateChanged += delegate
        {
            Repaint();
        };
    }
}
#endif