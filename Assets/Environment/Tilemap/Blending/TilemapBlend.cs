using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBlend : MonoBehaviour
{
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap[] blendTilemaps;
    [Space]
    [SerializeField] private TileBlendGroup[] tileBlendGroups;

    public void Blend()
    {
        // reset blend tilemaps
        foreach (Tilemap blendTilemap in blendTilemaps)
            blendTilemap.ClearAllTiles();
        
        // neighbors
        Vector3Int[] neighbors =
        {
            new(-1, 1),
            new(0, 1),
            new(1, 1),
            new(-1, 0),
            new(1, 0),
            new(-1, -1),
            new(0, -1),
            new(1, -1),
        };
        
        // blend
        foreach (Vector3Int pos in mainTilemap.cellBounds.allPositionsWithin)
        {
            if (!mainTilemap.HasTile(pos)) continue;
            
            TileBase centerTile = mainTilemap.GetTile(pos);
            TileBlendGroup centerTileBlendGroup = GetTileBlendGroup(centerTile);
            
            if (centerTileBlendGroup == null) continue;

            for (int i = 0; i < neighbors.Length; i++)
            {
                Vector3Int neighbor = neighbors[i];

                if (neighbor.x != 0 && neighbor.y != 0)
                {
                    TileBase xNeighborTile = mainTilemap.GetTile(pos + new Vector3Int(neighbor.x, 0, 0));
                    TileBlendGroup xNeighborTileBlendGroup = GetTileBlendGroup(xNeighborTile);

                    if (xNeighborTileBlendGroup == null || centerTileBlendGroup == xNeighborTileBlendGroup) continue;
                    
                    TileBase yNeighborTile = mainTilemap.GetTile(pos + new Vector3Int(0, neighbor.y, 0));
                    TileBlendGroup yNeighborTileBlendGroup = GetTileBlendGroup(yNeighborTile);
                    
                    if (yNeighborTileBlendGroup == null || centerTileBlendGroup == yNeighborTileBlendGroup) continue;
                }
                
                Vector3Int neighborPos = pos + neighbor;
                if (!mainTilemap.HasTile(neighborPos)) continue;

                TileBase neighborTile = mainTilemap.GetTile(neighborPos);
                TileBlendGroup neighborTileBlendGroup = GetTileBlendGroup(neighborTile);
                
                if (neighborTileBlendGroup == null) continue;
                if (centerTileBlendGroup == neighborTileBlendGroup) continue;
                if(neighborTileBlendGroup.priority > centerTileBlendGroup.priority) continue;
                
                TileBase blendTile = centerTileBlendGroup.blendTiles[neighbors.Length - i - 1];
                
                blendTilemaps[i].SetTile(neighborPos, blendTile);
            }
        }
    }

    private TileBlendGroup GetTileBlendGroup(TileBase tile)
    {
        foreach (TileBlendGroup tileBlendGroup in tileBlendGroups)
            if (tileBlendGroup.tilesInGroup.Contains(tile))
                return tileBlendGroup;
        return null;
    }

    public void Show(bool show)
    {
        foreach (Tilemap blendTilemap in blendTilemaps)
            blendTilemap.gameObject.SetActive(show);
    }

    [System.Serializable]
    private class TileBlendGroup
    {
        [AssetSelector]public TileBase[] tilesInGroup;
        [AssetSelector]public TileBase[] blendTiles;
        
        public int priority;
    }
}
