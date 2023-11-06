using System.Collections;
using System.Linq;
using UnityEngine;

public class MovementState : MonoBehaviour
{
    [Header("Sate Properties")]
    [SerializeField] protected PStateManager stateManager;
    [SerializeField] protected PStateManager.State[] activeStates;
    public bool IsActiveState { get; private set; }
    private Coroutine _stateEnterCoroutine;
    protected void Awake()
    {
        stateManager.OnStateChanged += delegate(PStateManager.State newState)
        {
            bool willBeActiveState = activeStates.Contains(newState);
            if (willBeActiveState == IsActiveState) return;

            if (_stateEnterCoroutine != null)
                StopCoroutine(_stateEnterCoroutine);
            
            if (willBeActiveState && !IsActiveState)
                _stateEnterCoroutine = StartCoroutine(OnStateEnterWaitForEndOfFrame());
            else if (!willBeActiveState && IsActiveState)
                OnStateExit();
            
            IsActiveState = willBeActiveState;
        };
    }
    private IEnumerator OnStateEnterWaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        OnStateEnter();
    }
    protected void Update()
    {
        if (IsActiveState)
            ActiveStateUpdate();
    }
    protected void FixedUpdate()
    {
        if (IsActiveState)
            ActiveStateFixedUpdate();
    }
    protected void LateUpdate()
    {
        if (IsActiveState)
            ActiveStateLateUpdate();
    }
    protected void OnDrawGizmos()
    {
        if (IsActiveState)
            ActiveStateOnDrawGizmos();
    }
    protected virtual void OnStateEnter() {}
    protected virtual void OnStateExit() {}
    protected virtual void ActiveStateUpdate() {}
    protected virtual void ActiveStateFixedUpdate() {}
    protected virtual void ActiveStateLateUpdate() {}
    protected virtual void ActiveStateOnDrawGizmos() {}
}