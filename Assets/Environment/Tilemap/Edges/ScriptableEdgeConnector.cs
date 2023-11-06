using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Edge Connector", menuName = "Tilemaps/Edge Connector", order = 1)]
public class ScriptableEdgeConnector : ScriptableObject
{
    public TileBase[] baseTiles;
    public TileBase[] edgeTiles;

    public TileBase GetEdgeFromBase(TileBase tileBase)
    {
        for(int i = 0; i < baseTiles.Length; i++)
        {
            if (baseTiles[i] != tileBase) continue;
            return edgeTiles[i];
        }
        return null;
    }
    public TileBase GetBaseFromEdge(TileBase tileEdge)
    {
        for (int i = 0; i < edgeTiles.Length; i++)
        {
            if (edgeTiles[i] != tileEdge) continue;
            Debug.Log("Found edge tile");
            Debug.Log("Base tile is " + baseTiles[i]);
            return baseTiles[i];
        }
        return null;
    }
    
    public void TryPlaceCorrespondingEdgeTile(TileBase tileBase, Tilemap tilemap, Vector3Int pos, out bool placed)
    {
        placed = false;
        
        TileBase tileEdge = GetEdgeFromBase(tileBase);
        if (tileEdge == null) return;
        
        tilemap.SetTile(pos, tileEdge);
        placed = true;
    }
}
