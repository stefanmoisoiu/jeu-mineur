using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Sketch", menuName = "Sketcher/Sketch", order = 1)]
public class ScriptableSketch : ScriptableObject
{
        [PreviewField(200,ObjectFieldAlignment.Center)] [LabelText("")]
        public Texture2D texture;
}

public struct SketchChunk
{
        
}