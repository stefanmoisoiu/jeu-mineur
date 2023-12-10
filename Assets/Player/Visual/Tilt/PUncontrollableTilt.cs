using UnityEngine;

public class PUncontrollableTilt : MovementState
{
    [Header("References")]
    [SerializeField] private PUncontrollable uncontrollable;
    [SerializeField] private Transform graphicsCenterPivot;
    [SerializeField] private Transform graphicsBottomPivot;
    [SerializeField] private PGrounded grounded;
    
    
    
    [Header("Tilt Properties")] [SerializeField]
    private float tiltLerpSpeed = 10f;
    
    private enum TiltState
    {
        Fall,
        Slide,
    }
    private TiltState _tiltState = TiltState.Fall;

    protected override void OnStateEnter()
    {
        uncontrollable.OnStartUncontrollable += StartUncontrollable;
        uncontrollable.OnStartSlide += StartSlide;
    }

    protected override void ActiveStateUpdate()
    {
        if (_tiltState == TiltState.Fall)
        {
            graphicsBottomPivot.rotation = Quaternion.Lerp(graphicsBottomPivot.rotation, Quaternion.identity,
                tiltLerpSpeed * Time.deltaTime);
        }
        else if (_tiltState == TiltState.Slide) graphicsBottomPivot.up = grounded.GroundHit.normal;
    }

    protected override void OnStateExit()
    {
        uncontrollable.OnStartUncontrollable -= StartUncontrollable;
        uncontrollable.OnStartSlide -= StartSlide;
    }

    private void StartUncontrollable() => _tiltState = TiltState.Fall;
    private void StartSlide() => _tiltState = TiltState.Slide;

}
