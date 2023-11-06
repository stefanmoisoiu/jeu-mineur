using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PCameraPointOfInterest : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private LayerMask poiLayer;
    
    public PointOfInterest CurrentPOI => activePOIs.Count > 0 ? activePOIs[0] : null;
    private List<PointOfInterest> activePOIs = new();
    
    public Action onPOIChanged;
    public Action<PointOfInterest> onPOIAdded;
    public Action<PointOfInterest> onPOIRemoved;
    private void Start()
    {
        colliderEvents.OnTriggerEnterValue += CheckEnterPOI;
        colliderEvents.OnTriggerExitValue += CheckExitPOI;
        
        onPOIChanged += POIChanged;
    }

    private void POIChanged()
    {
        foreach (PointOfInterest poi in activePOIs)
            poi.Cam.Priority = -1000;
        
        if (CurrentPOI == null)
        {
            cam.Priority = 0;
        }
        else
        {
            cam.Priority = -1000;
            CurrentPOI.Cam.Priority = 0;
        }
    }
    private void CheckEnterPOI(Collider2D other)
    {
        if(poiLayer != (poiLayer | (1 << other.gameObject.layer))) return;
        if(!other.TryGetComponent(out PointOfInterest poi)) return;
        if(activePOIs.Contains(poi)) return;
        
        activePOIs.Add(poi);
        
        onPOIChanged?.Invoke();
        onPOIAdded?.Invoke(poi);
    }
    private void CheckExitPOI(Collider2D other)
    {
        if(poiLayer != (poiLayer | (1 << other.gameObject.layer))) return;
        if(!other.TryGetComponent(out PointOfInterest poi)) return;
        if(!activePOIs.Contains(poi)) return;
        
        poi.Cam.Priority = -1000;
        
        activePOIs.Remove(poi);
        
        onPOIChanged?.Invoke();
        onPOIRemoved?.Invoke(poi);
    }
    
}
