#if UNITY_EDITOR
using UnityEngine;

public class ChooseRandomSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private void OnValidate()
    {
        if (spriteRenderer == null) return;
        if (sprites == null || sprites.Length == 0) return;
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
#endif