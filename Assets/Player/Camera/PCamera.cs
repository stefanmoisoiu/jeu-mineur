using Cinemachine;
using UnityEngine;

public class PCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private PCameraComponent[] cameraComponents;
    [SerializeField] private PCameraPointOfInterest cameraPointOfInterest;
    private CinemachineVirtualCamera _currentCam;
    private CinemachineFramingTransposer _currentCamFramingTransposer;
    
    private Vector3 _startPlayerCamOffset;

    private void Start()
    {
        _startPlayerCamOffset = playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        cameraPointOfInterest.onPOIChanged += OnPOIChanged;
        OnPOIChanged();
    }

    private void LateUpdate()
    {
        Vector3 offset = Vector3.zero;
        foreach (PCameraComponent cameraComponent in cameraComponents)
        {
            offset += cameraComponent.GetOffset();
        }
        if(_currentCamFramingTransposer != null) _currentCamFramingTransposer.m_TrackedObjectOffset = offset + _startPlayerCamOffset;
    }

    private void OnPOIChanged()
    {
        PointOfInterest newPoi = cameraPointOfInterest.CurrentPOI;
        _currentCam = newPoi == null ? playerCam : newPoi.Cam;
        
        _currentCamFramingTransposer = _currentCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
}