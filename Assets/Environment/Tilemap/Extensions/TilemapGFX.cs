#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class TilemapGFX : EditorWindow
{
    [MenuItem("Window/Tilemap GFX")]
    public static void ShowWindow()
    {
        TilemapGFX window = GetWindow<TilemapGFX>();
        window.titleContent = new GUIContent("Tilemap GFX");
        window.minSize = new Vector2(0,0);
    }
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        
        Button updateCopyTilesButton = new();
        updateCopyTilesButton.name = "Update Colliders";
        updateCopyTilesButton.text = "Update Colliders";
        updateCopyTilesButton.clicked += UpdateCopyTiles;
        root.Add(updateCopyTilesButton);
        
        root.Add(new Label(" "));
        
        Button updateEdgesButton = new();
        updateEdgesButton.name = "Update Edges";
        updateEdgesButton.text = "Update Edges";
        updateEdgesButton.clicked += UpdateEdges;
        root.Add(updateEdgesButton);
        
        // toggle show
        Toggle showEdgesToggle = new();
        showEdgesToggle.name = "Show Edges";
        showEdgesToggle.text = "Show Edges";
        showEdgesToggle.value = true;
        showEdgesToggle.RegisterValueChangedCallback(evt => ShowEdges(evt.newValue));
        root.Add(showEdgesToggle);
        
        root.Add(new Label(" "));
        
        Button updateBlendsButton = new();
        updateBlendsButton.name = "Update Blends";
        updateBlendsButton.text = "Update Blends";
        updateBlendsButton.clicked += UpdateBlends;
        root.Add(updateBlendsButton);
        
        // toggle show
        Toggle showBlendsToggle = new();
        showBlendsToggle.name = "Show Blends";
        showBlendsToggle.text = "Show Blends";
        showBlendsToggle.value = true;
        showBlendsToggle.RegisterValueChangedCallback(evt => ShowBlends(evt.newValue));
        root.Add(showBlendsToggle);
        
        root.Add(new Label(" "));
        
        Button updateTilemapButton = new();
        updateTilemapButton.name = "Update Tilemap";
        updateTilemapButton.text = "Update Tilemap";
        updateTilemapButton.clicked += UpdateAll;
        root.Add(updateTilemapButton);
        
        // toggle show
        Toggle showAllToggle = new();
        showAllToggle.name = "Show All";
        showAllToggle.text = "Show All";
        showAllToggle.value = true;
        showAllToggle.RegisterValueChangedCallback(evt =>
        {
            ShowEdges(evt.newValue);
            ShowBlends(evt.newValue);
        });
        root.Add(showAllToggle);
    }

    private void UpdateAll()
    {
        Debug.Log("Updating Tilemaps...");
        UpdateCopyTiles();
        Debug.Log("Updated Copy Tiles \u2714\ufe0f️");
        UpdateEdges();
        Debug.Log("Updated Edges \u2714\ufe0f️");
        UpdateBlends();
        Debug.Log("Updated Blends \u2714\ufe0f️");
    }
    private void UpdateCopyTiles()
    {
        CopyTiles[] copyTiles = FindObjectsOfType<CopyTiles>();
        foreach (CopyTiles copyTile in copyTiles)
            copyTile.Copy();
    }

    private void UpdateEdges()
    {
        TilemapEdgeFill[] tilemapEdgeFills = FindObjectsOfType<TilemapEdgeFill>();
        foreach (TilemapEdgeFill tilemapEdgeFill in tilemapEdgeFills)
            tilemapEdgeFill.Connect();
    }
    private void ShowEdges(bool show)
    {
        TilemapEdgeFill[] tilemapEdgeFills = FindObjectsOfType<TilemapEdgeFill>();
        foreach (TilemapEdgeFill tilemapEdgeFill in tilemapEdgeFills)
            tilemapEdgeFill.Show(show);
    }

    private void UpdateBlends()
    {
        TilemapBlend[] tilemapBlends = FindObjectsOfType<TilemapBlend>();
        foreach (TilemapBlend tilemapBlend in tilemapBlends)
            tilemapBlend.Blend();
    }
    private void ShowBlends(bool show)
    {
        TilemapBlend[] tilemapBlends = FindObjectsOfType<TilemapBlend>();
        foreach (TilemapBlend tilemapBlend in tilemapBlends)
            tilemapBlend.Show(show);
    }
}
#endif