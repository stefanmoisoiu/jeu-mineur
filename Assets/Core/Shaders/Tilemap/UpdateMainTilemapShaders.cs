using UnityEngine;


[ExecuteAlways]
public class UpdateMainTilemapShaders : MonoBehaviour
{
    [SerializeField] private Material[] mats;
    private static readonly int PlayerPosition = Shader.PropertyToID("_PlayerPosition");

    private void Update()
    {
        foreach(Material mat in mats) mat.SetVector(PlayerPosition, transform.position);
    }
}
