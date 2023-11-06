using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : PCameraComponent
{
    private static CameraShake _instance;
    private List<ShakeOffset> offsets = new ();
    private void Awake()
    {
        _instance = this;
    }
    public override Vector3 GetOffset()
    {
        Vector3 offset = Vector3.zero;
        foreach (ShakeOffset shakeOffset in offsets)
        {
            offset += shakeOffset.Offset;
        }
        return offset;
    }

    public static void MovementShake(float duration, float magnitude, float speed)
    {
        _instance.StartCoroutine(_instance.MovementShakeCoroutine(duration, magnitude, speed));
    }
    private IEnumerator MovementShakeCoroutine(float duration, float magnitude, float speed)
    {
        ShakeOffset shakeOffset = new();
        offsets.Add(shakeOffset);
        float time = 0;
        float randomSample = Random.Range(-10000, 10000);
        while (time < duration)
        {
            
            float x = Mathf.PerlinNoise(time * speed + randomSample, 0) * 2 - 1;
            float y = Mathf.PerlinNoise(0, time * speed + randomSample) * 2 - 1;
            
            shakeOffset.Offset = new Vector3(x, y, 0) * magnitude * (1 - time / duration);
            
            time += Time.deltaTime;
            yield return null;
        }
        offsets.Remove(shakeOffset);
    }
    internal class ShakeOffset
    {
        public Vector3 Offset = Vector3.zero;
    }
}
