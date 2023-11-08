using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "VinTools/Custom Tiles/Scriptable Rule Tiles")]
public class ScriptableRuleTiles : ScriptableObject
{
        [AssetSelector]public RuleTile[] ruleTiles;
}