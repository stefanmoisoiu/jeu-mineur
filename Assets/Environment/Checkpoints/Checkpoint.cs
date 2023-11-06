using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Preview")]
    [SerializeField] private BoxCollider2D previewCollider;
    [SerializeField] private bool showPreview = true;
    

    [Header("Checkpoint Properties")]
    [SerializeField] private bool canBeReachedMultipleTimes;
    private bool _hasAlreadyBeenReached;
    public bool HasAlreadyBeenReached => _hasAlreadyBeenReached;
    [SerializeField] private Vector3 spawnPointOffset;
    
    public Action OnCheckpointReached;
    
    public Vector3 GetSpawnPoint() => transform.position + spawnPointOffset;
    
    public void CheckpointReached()
    {
        if (_hasAlreadyBeenReached && !canBeReachedMultipleTimes) return;
        _hasAlreadyBeenReached = true;
        OnCheckpointReached?.Invoke();
    }
    private void OnDrawGizmos()
    {
        if (!showPreview) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + spawnPointOffset, 0.25f);

        if (previewCollider == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)previewCollider.offset, previewCollider.size);
    }

    private void OnValidate()
    {
        if (previewCollider == null) previewCollider = GetComponent<BoxCollider2D>();
        if (gameObject.layer != LayerMask.NameToLayer("Checkpoint")) gameObject.layer = LayerMask.NameToLayer("Checkpoint");
    }
}