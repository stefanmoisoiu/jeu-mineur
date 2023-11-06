using Cinemachine;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private CinemachineVirtualCamera cam;
    public CinemachineVirtualCamera Cam => cam;
    

    private void OnDrawGizmos()
    {   
        if(boxCollider2D == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)boxCollider2D.offset, boxCollider2D.size);
    }
}
