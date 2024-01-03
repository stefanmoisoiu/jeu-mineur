using System;
using System.Collections;
using SimpleAudioManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LayerManager : MonoBehaviour
{
    [SerializeField] private Material[] layerMats;
    [SerializeField] private float transitionLength = 0.75f;
    [SerializeField] private Manager audioManager;

    [HorizontalGroup]
    [SerializeField] private int editorLayer = 0;
    [HorizontalGroup]
    [Button] private void SetEditorLayer() => SetLayer(editorLayer);
    
    
    
    private Coroutine _transitionCoroutine;

    private int _currentLayer = 0;
    private static readonly int Layer = Shader.PropertyToID("_Layer");

    private void Start()
    {
        foreach (Material layerMat in layerMats) layerMat.SetFloat(Layer, _currentLayer);
    }

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

            foreach (Material layerMat in layerMats) layerMat.SetFloat(Layer, Mathf.Lerp(previousLayer, newLayer, advancement));
            
            yield return null;
        }
    }
}