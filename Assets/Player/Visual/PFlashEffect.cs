using System;
using System.Collections;
using UnityEngine;

public class PFlashEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private float flashPerSecond = 0.5f;
    
    private Material _mat;
    private static readonly int FlashHash = Shader.PropertyToID("_Flash");
    
    private Coroutine _flashCoroutine;

    private void Start()
    {
        // _mat = spriteRenderer.material;
        // _mat.SetFloat(FlashHash, 0);
        
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            _mat = spriteRenderer.material;
            _mat.SetFloat(FlashHash, 0);
        }
    }

    public void Flash(float duration)
    {
        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
        _flashCoroutine = StartCoroutine(FlashCoroutine(duration));
    }

    private IEnumerator FlashCoroutine(float duration)
    {
        float timer = 0;
        while (timer < duration - 0.1f)
        {
            timer += Time.deltaTime;
            _mat.SetFloat(FlashHash, 1 - Mathf.Round(timer * flashPerSecond % 1));
            yield return null;
        }
        _mat.SetFloat(FlashHash, 0);
    }
}
