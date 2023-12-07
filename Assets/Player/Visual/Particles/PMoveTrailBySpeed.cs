using UnityEngine;

public class PMoveTrailBySpeed : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Rigidbody2D rb;
    
    
    [Header("Properties")]
    [SerializeField] private float minSize;
    [SerializeField] private float minLength;
    [SerializeField] private float maxSize;
    [SerializeField] private float maxLength;
    [SerializeField] private float maxSizeMagnitude;
    [Space] [SerializeField] private float lerpSpeed;
    private float _currentAdvancement;

    private void Update()
    {
        float newAdvancement = Mathf.Clamp01(rb.velocity.magnitude / maxSizeMagnitude);
        _currentAdvancement = Mathf.Lerp(_currentAdvancement, newAdvancement, lerpSpeed * Time.deltaTime);
        
        float size = Mathf.Lerp(minSize, maxSize, _currentAdvancement);
        float length = Mathf.Lerp(minLength, maxLength, _currentAdvancement);
        trailRenderer.widthCurve = AnimationCurve.Linear(0, size, 1, 0);
        trailRenderer.time = length;
    }
}
