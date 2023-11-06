using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEdgeFill : MonoBehaviour
{
    [SerializeField] private Tilemap baseTilemap;
    [SerializeField] private Tilemap edgeTilemap;
    [SerializeField] private TilemapRenderer renderer;
    
    [SerializeField] private ScriptableEdgeConnector[] edgeConnectors;
    
    public void Connect()
    {
        edgeTilemap.ClearAllTiles();
        
        foreach (Vector3Int pos in baseTilemap.cellBounds.allPositionsWithin)
        {
            if (!baseTilemap.HasTile(pos)) continue;
            
            TileBase tile = baseTilemap.GetTile(pos);

            foreach (ScriptableEdgeConnector edgeConnector in edgeConnectors)
            {
                edgeConnector.TryPlaceCorrespondingEdgeTile(tile, edgeTilemap, pos, out bool placed);
                if (placed) break;
            }
        }
    }
    public void Show(bool show)
    {
        renderer.enabled = show;
    }
}