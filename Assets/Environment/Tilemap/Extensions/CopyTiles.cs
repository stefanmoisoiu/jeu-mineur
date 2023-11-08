using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CopyTiles : MonoBehaviour
{
    [SerializeField] private Tilemap origin, destination;
    [AssetSelector][SerializeField] private TileBase[] tilesToInclude;
    
    [Button]
    public void Copy()
    {
        if (origin == null || destination == null) return;
        destination.ClearAllTiles();
        foreach (Vector3Int pos in origin.cellBounds.allPositionsWithin)
        {
            if (!origin.HasTile(pos)) continue;
            
            TileBase tile = origin.GetTile(pos);
            if (tilesToInclude != null && tilesToInclude.Length > 0 && !tilesToInclude.Contains(tile)) continue;
            
            destination.SetTile(pos, tile);
        }
    }
}
