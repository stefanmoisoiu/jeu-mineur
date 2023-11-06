using System;
using System.Collections;
using UnityEngine;

public class FallingPlatformChain : MonoBehaviour
{
    [SerializeField] private TimedPlatform[] timedPlatforms;
    [SerializeField] private float baseTimeOffset = 0.5f;
    
    private bool _falling = false;
    private Coroutine _fallCoroutine;
    [SerializeField] private FallingPlatform[] createFallingPlatforms;

    private void OnValidate()
    {
        foreach (FallingPlatform createFallingPlatform in createFallingPlatforms)
        {
            if (createFallingPlatform == null) continue;
            bool found = false;
            foreach (TimedPlatform timedPlatform in timedPlatforms)
            {
                if (timedPlatform.FallingPlatform == createFallingPlatform)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Array.Resize(ref timedPlatforms, timedPlatforms.Length + 1);
                timedPlatforms[timedPlatforms.Length - 1] = new TimedPlatform
                {
                    FallingPlatform = createFallingPlatform,
                    AddedTimeOffset = 0
                };
            }
        }
    }

    public void StartFallChain()
    {
        if (_falling) return;
        _fallCoroutine = StartCoroutine(FallChainCoroutine());
    }

    private IEnumerator FallChainCoroutine()
    {
        _falling = true;
        foreach (TimedPlatform timedPlatform in timedPlatforms)
        {
            timedPlatform.FallingPlatform.Fall();
            yield return new WaitForSeconds(baseTimeOffset + timedPlatform.AddedTimeOffset);
        }
        _falling = false;
    }
}

[System.Serializable]
public struct TimedPlatform
{
    public FallingPlatform FallingPlatform;
    public float AddedTimeOffset;
}