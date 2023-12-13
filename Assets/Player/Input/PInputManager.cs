using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PInputManager : MonoBehaviour
{
    private Controls _controls;

    public float MoveInput { get; private set; }
    public float Verticalnput { get; private set; }
    
    public Vector2 GamePadMoveDirection { get; private set; }
    public Vector2 GamePadLookDirection { get; private set; }
    
    
    public Action OnDownPress;
    
    public Action OnJump,OnStopJump;
    public bool Jump { get; private set; }
    
    
    public Action OnMainAction,OnStopMainAction;
    public bool MainAction { get; private set; }
    
    
    public Action OnSecondaryAction,OnStopSecondaryAction;
    public bool SecondaryAction { get; private set; }

    public Action OnPause;

    private Camera _cam;

    public enum ControlType
    {
        KeyboardAndMouse,
        Controller
    }
    public ControlType CurrentControlType
    {
        get
        {
            if (_controls.Movement.Move.activeControl == null) return ControlType.KeyboardAndMouse;
            if (_controls.Movement.Move.activeControl.device is Keyboard) return ControlType.KeyboardAndMouse;
            if (_controls.Movement.Move.activeControl.device is Gamepad) return ControlType.Controller;
            return ControlType.KeyboardAndMouse;
        }
    }
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Enable();
        
            _controls.Movement.Move.performed += ctx => MoveInput = ctx.ReadValue<float>();
            _controls.Movement.Vertical.performed += ctx => Verticalnput = ctx.ReadValue<float>();
            _controls.Movement.Vertical.performed += delegate(InputAction.CallbackContext ctx)
            {
                if(ctx.ReadValue<float>() < -.5f) OnDownPress?.Invoke();
            };
        
        
            _controls.Movement.Jump.started += _ => OnJump?.Invoke();
            _controls.Movement.Jump.canceled += _ => OnStopJump?.Invoke();
        
            _controls.Movement.Jump.started += _ => Jump = true;
            _controls.Movement.Jump.canceled += _ => Jump = false;
        
            _controls.Actions.Main.started += _ => OnMainAction?.Invoke();
            _controls.Actions.Main.canceled += _ => OnStopMainAction?.Invoke();
        
            _controls.Actions.Main.started += _ => MainAction = true;
            _controls.Actions.Main.canceled += _ => MainAction = false;
        
            _controls.Actions.Secondary.started += _ => OnSecondaryAction?.Invoke();
            _controls.Actions.Secondary.canceled += _ => OnStopSecondaryAction?.Invoke();
        
            _controls.Actions.Secondary.started += _ => SecondaryAction = true;
            _controls.Actions.Secondary.canceled += _ => SecondaryAction = false;
        
            _controls.Movement.GamePadMoveDirection.performed += ctx => GamePadMoveDirection = ctx.ReadValue<Vector2>();
            _controls.Actions.GamePadLookDirection.performed += ctx => GamePadLookDirection = ctx.ReadValue<Vector2>();
        
            _controls.Actions.Pause.started += _ => OnPause?.Invoke();
        }
        else
        {
            _controls.Enable();
        }
    }

    public Vector2 GetLookDirection()
    {
        switch (CurrentControlType)
        {
            case ControlType.KeyboardAndMouse:
                if(_cam == null) _cam = Camera.main;
                Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 lookDir = mousePos - (Vector2)transform.position;
                return lookDir.normalized;
                break;
            case ControlType.Controller:
                return GamePadLookDirection != Vector2.zero ? GamePadLookDirection.normalized : GamePadMoveDirection != Vector2.zero ? GamePadMoveDirection.normalized : Vector2.zero;
        }
        return Vector2.zero;
    }
    public void SetInputActive(bool active)
    {
        if (active)
        {
            _controls.Enable();
        }
        else
        {
            _controls.Disable();
        }
    }
    private void OnDisable()
    {
        _controls?.Disable();
    }
}
