using UnityEngine;

[ExecuteAlways]
public class CopyCameraSize : MonoBehaviour
{
    [SerializeField] private Camera otherCam,thisCam;

    private void Update()
    {
        thisCam.orthographicSize = otherCam.orthographicSize;
    }
}
