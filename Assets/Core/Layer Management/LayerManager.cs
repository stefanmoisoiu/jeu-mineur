using System;
using System.Collections;
using SimpleAudioManager;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    [SerializeField] private Material[] layerMaterials;
    [SerializeField] private float transitionLength = 0.75f;
    [SerializeField] private Manager audioManager;
    
    
    private Coroutine _transitionCoroutine;

    private int _currentLayer = 0;
    private static readonly int Layer = Shader.PropertyToID("_Layer");

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetLayer(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetLayer(1);
    }

    public void SetLayer(int layer)
    {
        if (layer == _currentLayer) return;
        
        audioManager.PlaySong(layer);
        
        if(_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
        _transitionCoroutine = StartCoroutine(TransitionToLayer(_currentLayer,layer));
        
        _currentLayer = layer;
    }

    private IEnumerator TransitionToLayer(int previousLayer, int newLayer)
    {
        float advancement = 0;
        while (advancement < 1)
        {
            advancement += Time.deltaTime / transitionLength;

            foreach (Material layerMaterial in layerMaterials)    
            {
                layerMaterial.SetFloat(Layer, Mathf.Lerp(previousLayer, newLayer, advancement));
            }
            
            yield return null;
        }
    }
}