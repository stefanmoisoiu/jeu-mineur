using UnityEngine;

public class PCheckpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PDeath pDeath;
    [SerializeField] private ColliderEvents colliderEvents;
    
    [Header("Checkpoint Properties")]
    [SerializeField] private LayerMask checkpointLayerMask;
    
    private Checkpoint _currentCheckpoint;
    public Checkpoint CurrentCheckpoint => _currentCheckpoint;
    
    private void Start()
    {
        pDeath.OnDeath += Respawn;
        colliderEvents.OnTriggerEnterValue += CheckCheckpoint;
    }
    private void Respawn()
    {
        if(_currentCheckpoint == null) return;
        rb.position = _currentCheckpoint.GetSpawnPoint();
        rb.velocity = Vector2.zero;
    }
    
    private void CheckCheckpoint(Collider2D other)
    {
        if (checkpointLayerMask != (checkpointLayerMask | (1 << other.gameObject.layer))) return;
        if(!other.TryGetComponent(out Checkpoint checkpoint)) return;
        _currentCheckpoint = checkpoint;
        checkpoint.CheckpointReached();
    }
}