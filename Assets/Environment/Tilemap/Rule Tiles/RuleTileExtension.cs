#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(RuleTileExtension))]
public class RuleTileExtension : EditorWindow
{
    [SerializeField] private RuleTile ruleTile;
    [SerializeField] private TextAsset file;
    [SerializeField] private Texture2D texture;

    [MenuItem("Tools/RuleTile Extension")]
    public static void ShowWindow()
    {
        GetWindow<RuleTileExtension>("RuleTile Extension");
    }

    private void OnGUI()
    {
        ruleTile = (RuleTile)EditorGUILayout.ObjectField("RuleTile", ruleTile, typeof(RuleTile), false);
        if (ruleTile == null) return;
        if (GUILayout.Button("Save sprite indices"))
        {
            string path = AssetDatabase.GetAssetPath(ruleTile);
            string text = "";
            
            if(ruleTile.m_DefaultSprite != null) text += ruleTile.m_DefaultSprite.name.Split("_")[1] + "\n";
            else text += "\n";
            
            for (var i = 0; i < ruleTile.m_TilingRules.Count; i++)
            {
                var tilingRule = ruleTile.m_TilingRules[i];
                for (var j = 0; j < tilingRule.m_Sprites.Length; j++)
                {
                    var sprite = tilingRule.m_Sprites[j];
                    if (sprite == null) continue;
                    text += sprite.name.Split("_")[1];
                    if (j < tilingRule.m_Sprites.Length - 1) text += ",";
                }
                if (i < ruleTile.m_TilingRules.Count - 1) text += "\n";
            }

            File.WriteAllText(path.Replace(".asset", ".txt"), text);
            AssetDatabase.Refresh();
            Debug.Log("Saved sprite indices to " + path.Replace(".asset", ".txt"));
        }
        file = (TextAsset)EditorGUILayout.ObjectField("File", file, typeof(TextAsset), false);
        texture = (Texture2D)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false);
        if (file == null) return;
        if (texture == null) return;
        if (GUILayout.Button("Load sprite indices"))
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture)).OfType<Sprite>().ToArray();
            
            string[] lines = file.text.Split('\n');
            if (lines[0] != "")
                ruleTile.m_DefaultSprite = sprites[int.Parse(lines[0])];
            lines = lines.Skip(1).ToArray();
            
            for (var i = 0; i < lines.Length; i++)
            {
                string[] spriteIndices = lines[i].Split(',');
                for (var j = 0; j < spriteIndices.Length; j++)
                {
                    string spriteIndex = spriteIndices[j];
                    if (spriteIndex == "") continue;
                    ruleTile.m_TilingRules[i].m_Sprites[j] = sprites[int.Parse(spriteIndex)];
                }
            }
            EditorUtility.SetDirty(ruleTile);
        }
    }
}
#endif