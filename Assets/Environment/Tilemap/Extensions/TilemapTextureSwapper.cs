#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class TilemapTextureSwapper : EditorWindow
{
    private TileBase _tileBase;
    private Texture2D previousTexture, newTexture;
    
    [MenuItem("Tools/Tilemap Texture Swapper")]
    public static void ShowWindow()
    {
        GetWindow<TilemapTextureSwapper>("Tilemap Texture Swapper");
    }
    
    private void OnGUI()
    {
        _tileBase = (TileBase) EditorGUILayout.ObjectField("Advanced Rule Tile", _tileBase, typeof(TileBase), false);
        previousTexture = (Texture2D) EditorGUILayout.ObjectField("previousTexture", previousTexture, typeof(Texture2D), false);
        newTexture = (Texture2D) EditorGUILayout.ObjectField("newTexture", newTexture, typeof(Texture2D), false);
        
        if (GUILayout.Button("Swap"))
        {
            Swap();
        }
    }

    private void Swap()
    {
        if (_tileBase == null || previousTexture == null || newTexture == null)
        {
            Debug.LogError("Advanced Rule Tile or Sprite Atlas is null");
            return;
        }
        
        Sprite[] previousSprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(previousTexture)).OfType<Sprite>().ToArray();
        Sprite[] newSprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(newTexture)).OfType<Sprite>().ToArray();

        for (int i = 0; i < _tileBase.m_TilingRules.Count; i++)
        {
            RuleTile.TilingRule rule = _tileBase.m_TilingRules[i];
            for (int j = 0; j < rule.m_Sprites.Length; j++)
            {
                Sprite sprite = rule.m_Sprites[j];
                for (int k = 0; k < previousSprites.Length; k++)
                {
                    if (sprite == previousSprites[k])
                    {
                        rule.m_Sprites[j] = newSprites[k];
                    }
                }
            }
        }
        
        EditorUtility.SetDirty(_tileBase);
    }
}

#endif