using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CopyTiles : MonoBehaviour
{
    [SerializeField] private Tilemap origin, destination;
    [SerializeField] private TileBase[] tilesToInclude;
    
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
