#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class TilemapGFX : EditorWindow
{
    public VisualTreeAsset layout;
    private Toggle _showEdgesToggle, _showBlendsToggle, _showAllToggle;
    private VisualElement _previewEdges, _previewBlends;
    [MenuItem("Window/Tilemap GFX")]
    public static void ShowWindow()
    {
        TilemapGFX window = GetWindow<TilemapGFX>();
        window.titleContent = new GUIContent("Tilemap GFX");
        window.minSize = new Vector2(0,0);
    }
    public void CreateGUI()
    {
        layout.CloneTree(rootVisualElement);
        VisualElement root = rootVisualElement;
        
        root.Q("CollisionsUpdate").RegisterCallback<MouseUpEvent>(evt => UpdateCopyTiles());
        
        root.Q("EdgesUpdate").RegisterCallback<MouseUpEvent>(evt => UpdateCopyTiles());
        _showEdgesToggle = root.Q("EdgesVisibleToggle") as Toggle;
        _showEdgesToggle.RegisterValueChangedCallback(evt => ShowEdges(evt.newValue));
        
        root.Q("BlendUpdate").RegisterCallback<MouseUpEvent>(evt => UpdateBlends());
        _showBlendsToggle = root.Q("BlendVisibleToggle") as Toggle;
        _showBlendsToggle.RegisterValueChangedCallback(evt => ShowBlends(evt.newValue));
        
        root.Q("AllUpdate").RegisterCallback<MouseUpEvent>(evt => UpdateAll());
        _showAllToggle = root.Q("AllVisibleToggle") as Toggle;
        _showAllToggle.RegisterValueChangedCallback(evt =>
        {
            ShowEdges(evt.newValue);
            ShowBlends(evt.newValue);
        });
        
        _previewEdges = root.Q("PreviewEdges");
        _previewBlends = root.Q("PreviewBlend");
    }

    private void UpdateAll()
    {
        UpdateCopyTiles();
        
        UpdateEdges();
        
        UpdateBlends();
        
        Debug.Log("Updated Tilemap \u2714\ufe0f️");
    }
    private void UpdateCopyTiles()
    {
        CopyTiles[] copyTiles = FindObjectsOfType<CopyTiles>();
        foreach (CopyTiles copyTile in copyTiles)
            copyTile.Copy();
        Debug.Log("Updated Copy Tiles \u2714\ufe0f️");
    }

    private void UpdateEdges()
    {
        TilemapEdgeFill[] tilemapEdgeFills = FindObjectsOfType<TilemapEdgeFill>();
        foreach (TilemapEdgeFill tilemapEdgeFill in tilemapEdgeFills)
            tilemapEdgeFill.Connect();
        Debug.Log("Updated Edges \u2714\ufe0f️");
    }
    private void UpdateBlends()
    {
        TilemapBlend[] tilemapBlends = FindObjectsOfType<TilemapBlend>();
        foreach (TilemapBlend tilemapBlend in tilemapBlends)
            tilemapBlend.Blend();
        Debug.Log("Updated Blends \u2714\ufe0f️");
    }
    private void ShowEdges(bool show)
    {
        _showEdgesToggle.SetValueWithoutNotify(show);
        _previewEdges.visible = show;
        TilemapEdgeFill[] tilemapEdgeFills = FindObjectsOfType<TilemapEdgeFill>();
        foreach (TilemapEdgeFill tilemapEdgeFill in tilemapEdgeFills)
            tilemapEdgeFill.Show(show);
    }
    private void ShowBlends(bool show)
    {
        _showBlendsToggle.SetValueWithoutNotify(show);
        _previewBlends.visible = show;
        TilemapBlend[] tilemapBlends = FindObjectsOfType<TilemapBlend>();
        foreach (TilemapBlend tilemapBlend in tilemapBlends)
            tilemapBlend.Show(show);
    }
}
#endif